using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileScript : MonoBehaviour {
	
	public Vector3 target;
	float missileDamage = 40f;
	float currLife = 0f;
	float lifetime = 8f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		transform.Translate(Vector3.forward * Time.deltaTime * 200f);

		currLife += Time.deltaTime;

		if(currLife >= lifetime)
		{
			TriggerExplosion();
		}

	}

	void OnCollisionEnter(Collision col)
	{
		TriggerExplosion();
	}

	void TriggerExplosion()
	{
		Vector3 explosionPos = transform.position;

		Collider[] colliders = Physics.OverlapSphere(explosionPos, 50f);

		foreach(Collider hit in colliders)
		{
			Debug.Log("Collided with : " + hit.gameObject.name);

			TargetScript target = hit.GetComponent<TargetScript>();

			if(target != null && target.canShoot)
			{
				Rigidbody rb = hit.GetComponent<Rigidbody>();

				Debug.Log("Missile Hit : " + rb.gameObject.name);

				target.TakeDamage(missileDamage);
				rb.AddExplosionForce(500f, explosionPos, 50f, 10f);

			}
		}

		SoundManagerScript.Instance.PlaySFX(SoundManagerScript.Instance.audioClipInfoList[3].audioClipID);

		Destroy(this.gameObject);
	}
}
