using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LaserChargeBarFillerScript : MonoBehaviour {

	public Image bar;
	public PlayerScript player;

	// Use this for initialization
	void Start () {
		bar.fillAmount = player.currCharge / player.maxCharge;
	}
	
	// Update is called once per frame
	void Update () {
		bar.fillAmount = player.currCharge / player.maxCharge;
	}
}
