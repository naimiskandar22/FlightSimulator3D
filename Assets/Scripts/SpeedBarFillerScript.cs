using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SpeedBarFillerScript : MonoBehaviour {

	public Image bar;
	public PlayerScript player;

	// Use this for initialization
	void Start () 
	{
		bar.fillAmount = (player.currSpeed / player.maxSpeed) / 2;
	}
	
	// Update is called once per frame
	void Update () {
		bar.fillAmount = (player.currSpeed / player.maxSpeed) / 2;
	}
}
