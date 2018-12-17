using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour {

	SpringJoint spring;
	Material myMat;

	public float health;
	float maxHealth;
	public bool canShoot = true;
	bool regen = false;

	// Use this for initialization
	void Start () {
		spring = GetComponent<SpringJoint>();
		myMat = GetComponent<MeshRenderer>().material;
		maxHealth = health;

		//if(spring != null) spring.connectedAnchor = transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if(regen) Regenerate();
	}

	public void TakeDamage(float damage)
	{
		health -= damage;
		float alpha = (155 + health) / 255;
		Color newColor;

		newColor.r = myMat.color.r;
		newColor.g = myMat.color.g;
		newColor.b = myMat.color.b;
		newColor.a = alpha;


		myMat.color = newColor;

		if(health <= 0)
		{
			canShoot = false;
			regen = true;
		}
	}

	void Regenerate()
	{
		if(health <= maxHealth)
		{
			health += Time.deltaTime * 20f;
		}
		else
		{
			regen = false;
			canShoot = true;
		}

		float alpha = (155 + health) / 255;
		Color newColor;

		newColor.r = myMat.color.r;
		newColor.g = myMat.color.g;
		newColor.b = myMat.color.b;
		newColor.a = alpha;


		myMat.color = newColor;
			
	}
}
