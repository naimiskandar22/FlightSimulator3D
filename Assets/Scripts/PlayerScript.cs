using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour {

	public MenuScript pauseMenu;

	public float minSpeed = 10f;
	public float maxSpeed = 20f;
	public float defaultSpeed = 15f;
	public float currSpeed;
	public bool canMove = true;

	public Camera fpsCam;
	public float fireRange = 100f;
	public float fireRate = 50f;
	public float fireDamage = 10f;
	public ParticleSystem[] muzzleFlash = new ParticleSystem[4];
	int flashTurn = 0;
	public GameObject impactEffect;
	public float maxGunHeat = 100f;
	public float currGunHeat = 0f;
	bool gunOverheat = false;

	public MoveJoystickScript moveJoystick;
	private float nextTimeToFire = 0f;
	private float nextTimeToFireMissile = 0f;

	public bool fireMissile = false;
	public float maxCharge = 100f;
	public float currCharge = 0f;
	public GameObject missilePrefab;
	//LineRenderer line;

	//Swipe Input
	Vector2 firstPressPos;
	Vector2 secondPressPos;
	Vector2 currentSwipe;

	// Use this for initialization
	void Start () 
	{
		currSpeed = defaultSpeed;
		currGunHeat = 0f;
		gunOverheat = false;

		//line = GetComponent<LineRenderer>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(!pauseMenu.isPaused)
		{
			MoveForward();

			Shoot();

			Swipe();
		}

	}

	void MoveForward()
	{
		if(!moveJoystick.isDragging)
		{
			if(currSpeed < defaultSpeed)
			{
				currSpeed += Time.deltaTime * defaultSpeed;
			}
			else if(currSpeed > defaultSpeed)
			{
				currSpeed -= Time.deltaTime * defaultSpeed;
			}
		}

		//float moveSpeed = defaultSpeed + speedBoost;

//		if(currSpeed > maxSpeed)
//		{
//			currSpeed = maxSpeed;
//		}
//		else if(currSpeed < minSpeed)
//		{
//			currSpeed = minSpeed;
//		}

		transform.Translate(Vector3.forward * currSpeed * Time.deltaTime);
	}

	public void Shoot()
	{
		RaycastHit hit;

		if(gunOverheat)
		{
			currGunHeat -= Time.deltaTime * 25f;

			if(currGunHeat <= 0f)
			{
				gunOverheat = false;
			}

		}
		else if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, fireRange) && !gunOverheat)
		{
			TargetScript target = hit.transform.GetComponent<TargetScript>();

			if(target != null && target.canShoot)
			{
				if(Time.time >= nextTimeToFire)
				{
					target.TakeDamage(fireDamage);

					SoundManagerScript.Instance.PlaySFX(SoundManagerScript.Instance.audioClipInfoList[2].audioClipID);

					muzzleFlash[flashTurn].Play();
					muzzleFlash[flashTurn+2].Play();

					nextTimeToFire = Time.time + 1f/ fireRate;
					Debug.Log(hit.transform.name);

					flashTurn++;

					if(flashTurn >= 2)
					{
						flashTurn = 0;
					}

					if(hit.rigidbody != null) hit.rigidbody.AddForce(-hit.normal * 50f);

					GameObject impactGO = Instantiate(impactEffect, hit.point, Quaternion.LookRotation(hit.normal));
					Destroy(impactGO, 0.5f);

					currGunHeat += 15f;

					if(currGunHeat >= maxGunHeat)
					{
						gunOverheat = true;
					}

					if(currCharge < maxCharge && !fireMissile)
					{
						currCharge += 5f;
					}
				}
			}
		}

		if(currGunHeat > 0f)
		{
			currGunHeat -= Time.deltaTime * 35f;
		}
	}

	public void ShootLaser()
	{
		if(currCharge >= maxCharge && !fireMissile && !pauseMenu.isPaused)
		{
			StopCoroutine("FireMissiles");
			StartCoroutine("FireMissiles");
		}
	}

	IEnumerator FireMissiles()
	{
		//line.enabled = true;
		fireMissile = true;

		while(fireMissile)
		{
			//line.material.mainTextureOffset = new Vector2(0, Time.time);

			if(Time.time >= nextTimeToFireMissile)
			{
				//line.SetPosition(0, transform.position);


				nextTimeToFire = Time.time + 1f / 500;

				Ray ray = new Ray(transform.position, transform.forward);

				//RaycastHit hit;

				Vector3 rot = transform.rotation.eulerAngles;

				rot = new Vector3(rot.x, rot.y, rot.z);

				GameObject go = Instantiate(missilePrefab, transform.position - new Vector3(0, 2, 0), Quaternion.Euler(rot));
				MissileScript missileScript = go.GetComponent<MissileScript>();

//				if(Physics.Raycast(fpsCam.transform.position, fpsCam.transform.forward, out hit, 200f))
//				{
//					//line.SetPosition(1, hit.point);
//					
////					if(hit.transform.gameObject != null)
////					{
////						missileScript.target = hit.point;
////					}
//				}
//				else
//				{
//					//line.SetPosition(1, ray.GetPoint(200f));
//					//missileScript.target = ray.GetPoint(missileRange);
//				}

				currCharge -= 20f;

				if(currCharge <= 0)
				{
					currCharge = 0f;
					fireMissile = false;
				}
			}

			yield return null;
			yield return new WaitForSeconds(0.5f);
		}

		//line.enabled = false;
	}

	public void StartRollUpward()
	{
		if(canMove && !pauseMenu.isPaused)
		{
			StopCoroutine("RollUp");
			StartCoroutine("RollUp");
		}
	}

	public void StartRollDownward()
	{
		if(canMove && !pauseMenu.isPaused)
		{
			StopCoroutine("RollDown");
			StartCoroutine("RollDown");
		}
	}

	public void StartRollRight()
	{
		if(canMove)
		{
			StopCoroutine("RollRight");
			StartCoroutine("RollRight");
		}
	}

	public void StartRollLeft()
	{
		if(canMove)
		{
			StopCoroutine("RollLeft");
			StartCoroutine("RollLeft");
		}
	}

	IEnumerator RollUp()
	{
		float angle = 0f;
		canMove = false;

		while(!canMove)
		{
			//transform.Rotate(Vector3.back * Time.deltaTime * 200f);
			transform.Rotate(Vector3.left * Time.deltaTime * 200f);

			angle += Time.deltaTime * 200f;

			if(angle >= 180f)
			{
				canMove = true;
			}

			yield return null;
			//yield return new WaitForSeconds(0.5f);
		}

		canMove = false;
		angle = 0f;

		while(!canMove)
		{
			transform.Rotate(Vector3.back * Time.deltaTime * 200f);
			//transform.Rotate(Vector3.right * Time.deltaTime * 200f);

			angle += Time.deltaTime * 200f;

			if(angle >= 180f)
			{
				canMove = true;
			}

			yield return null;
			//yield return new WaitForSeconds(0.5f);

		}
	}

	IEnumerator RollDown()
	{
		float angle = 0f;
		canMove = false;

		while(!canMove)
		{
			//transform.Rotate(Vector3.back * Time.deltaTime * 200f);
			transform.Rotate(Vector3.right * Time.deltaTime * 200f);

			angle += Time.deltaTime * 200f;

			if(angle >= 180f)
			{
				canMove = true;
			}

			yield return null;
			//yield return new WaitForSeconds(0.5f);

		}

		canMove = false;
		angle = 0f;

		while(!canMove)
		{
			transform.Rotate(Vector3.forward * Time.deltaTime * 200f);
			//transform.Rotate(Vector3.right * Time.deltaTime * 200f);

			angle += Time.deltaTime * 200f;

			if(angle >= 180f)
			{
				canMove = true;
			}

			yield return null;
			//yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator RollRight()
	{
		float angle = 0f;
		canMove = false;

		while(!canMove)
		{
			transform.position += new Vector3(1, 0, 0);
			transform.Rotate(Vector3.back * Time.deltaTime * 200f);

			angle += Time.deltaTime * 200f;

			if(angle >= 360f)
			{
				canMove = true;
			}

			yield return null;
			//yield return new WaitForSeconds(0.5f);
		}
	}

	IEnumerator RollLeft()
	{
		float angle = 0f;
		canMove = false;

		while(!canMove)
		{
			transform.position -= new Vector3(1, 0, 0);
			transform.Rotate(Vector3.forward * Time.deltaTime * 200f);

			angle += Time.deltaTime * 200f;

			if(angle >= 360f)
			{
				canMove = true;
			}

			yield return null;
			//yield return new WaitForSeconds(0.5f);
		}
	}

	public void Swipe()
	{
		if(Input.touches.Length > 0)
		{
			Touch t = Input.GetTouch(0);
			if(t.phase == TouchPhase.Began)
			{
				firstPressPos = new Vector2(t.position.x,t.position.y);
			}
			if(t.phase == TouchPhase.Ended)
			{
				secondPressPos = new Vector2(t.position.x,t.position.y);

				currentSwipe = new Vector3(secondPressPos.x - firstPressPos.x, secondPressPos.y - firstPressPos.y);

				//normalize the 2d vector
				currentSwipe.Normalize();

				if(firstPressPos.y > 300 && Vector2.Distance(firstPressPos, secondPressPos) > 15f)
				{
					//swipe upwards
					if(currentSwipe.y > 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
					{
						Debug.Log("up swipe");
					}
					//swipe down
					if(currentSwipe.y < 0 && currentSwipe.x > -0.5f && currentSwipe.x < 0.5f)
					{
						Debug.Log("down swipe");
					}
					//swipe left
					if(currentSwipe.x < 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
					{
						Debug.Log("left swipe");

						StartRollLeft();
					}
					//swipe right
					if(currentSwipe.x > 0 && currentSwipe.y > -0.5f && currentSwipe.y < 0.5f)
					{
						Debug.Log("right swipe");

						StartRollRight();
					}
				}
			}
		}
	}
}
