using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour {

	public LookJoystickScript joystickLook;
	public Slider flightSlider;
	public Slider bgmSlider;
	public Slider sfxSlider;
	public Canvas pauseMenu;

	public bool isPaused = false;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(isPaused)
		{
			SoundManagerScript.Instance.SetBGMVolume(bgmSlider.value);
			SoundManagerScript.Instance.SetSFXVolume(sfxSlider.value);
		}
	}

	public void Pause()
	{
		SoundManagerScript.Instance.PlaySFX(SoundManagerScript.Instance.audioClipInfoList[1].audioClipID);

		if(!isPaused)
		{
			pauseMenu.enabled = true;
			isPaused = !isPaused;
			flightSlider.value = joystickLook.rotateSpeed / 40;
			bgmSlider.value = SoundManagerScript.Instance.bgmVolume;
			sfxSlider.value = SoundManagerScript.Instance.sfxVolume;

			Time.timeScale = 0f;
		}
		else
		{
			pauseMenu.enabled = false;
			isPaused = !isPaused;
			joystickLook.rotateSpeed = flightSlider.value * 40;
			Time.timeScale = 1f;
		}
	}
}
