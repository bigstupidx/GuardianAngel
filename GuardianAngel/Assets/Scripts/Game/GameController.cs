﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class GameController : MonoBehaviour 
{
	public SaveAndLoadScript saveLoadScript;
	public Terrain terrain;
	public WeatherSystem weatherSystem;
	public int thisDifficulty;
	public bool isCutsceneComplete;

	public int SwitchesActivated;
	public int MaximumSwitchesToActivate = 6;
	public Transform RadioTowerLink;

	[Header ("Switches")]
	public GameObject[] SwitchObjects;
	public SwitchScript[] Switches;
	public Gradient endGradient;
	public AudioSource MasterAmbience;

	[Header ("On all awitches activated")]
	public Collider CellarDoorTrigger;
	public GameObject CellarDoorsClosed;
	public GameObject CellarDoorsOpened;

	[Header ("Background audio")]
	public GameObject BackgroundAmbienceParent;
	public AudioSource[] BackgroundAmbience;

	[Header ("Level timer")]
	public bool isCountingTime;
	public float LevelTimerElapsed;
	public string thisLevelTime;
	public TextMeshProUGUI LevelTimerEndText;

	void Start ()
	{
		saveLoadScript = GameObject.Find ("SaveAndLoad").GetComponent<SaveAndLoadScript> ();
		saveLoadScript.gameControllerScript = this;

		Invoke ("LoadStuff", 18);

		thisDifficulty = saveLoadScript.levelOneDifficulty;

		AudioListener.volume = saveLoadScript.MasterVolume;

		BackgroundAmbience [0].Play ();

		CellarDoorTrigger.enabled = false;
		CellarDoorsClosed.SetActive (true);
		CellarDoorsOpened.SetActive (false);

		isCountingTime = true; // Set this to false when we have a cutscene ready.
		InvokeRepeating ("LevelTimer", 0, 1);

		SetSwitchState (false);
		weatherSystem.enabled = false;
	}

	void LoadStuff ()
	{
		saveLoadScript.LoadPlayerData ();
		saveLoadScript.LoadSettingsData ();
	}

	public void UpdateBackgroundAmbience ()
	{
		BackgroundAmbience [SwitchesActivated].Play ();
	}

	public void RefreshSwitchesLines ()
	{
		if (SwitchesActivated == MaximumSwitchesToActivate) 
		{
			for (int i = 0; i < Switches.Length; i++) 
			{
				Switches[i].line.colorGradient = endGradient;
			}

			OpenCellarDoor ();
			isCountingTime = false;
			CancelInvoke ("LevelTimer");
		}
	}

	void OpenCellarDoor ()
	{
		CellarDoorsClosed.SetActive (false);
		CellarDoorsOpened.SetActive (true);
		CellarDoorTrigger.enabled = true;
	}

	void LevelTimer ()
	{
		if (isCountingTime == true) 
		{
			LevelTimerElapsed += 1;
			GetLevelTime ();
		}
	}

	public void GetLevelTime ()
	{
		string thisLevelTime = Mathf.Floor ((LevelTimerElapsed / 60)) + ":" + ((LevelTimerElapsed % 60).ToString ("00"));
		LevelTimerEndText.text = thisLevelTime;
	}

	public void SetSwitchState (bool switchIsInteractable)
	{
		foreach (GameObject switchObject in SwitchObjects) 
		{
			switchObject.SetActive (switchIsInteractable);
		}
	}
}