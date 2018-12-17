using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GunHeatBarFillerScript : MonoBehaviour {

	public Image bar;
	public PlayerScript player;

	// Use this for initialization
	void Start () 
	{
		bar.fillAmount = (player.currGunHeat / player.maxGunHeat) / 2;
	}
	
	// Update is called once per frame
	void Update () 
	{
		bar.fillAmount = (player.currGunHeat / player.maxGunHeat) / 2;
	}
}
