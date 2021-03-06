﻿/* Author: Guangpeng Li
 * University of Liverpool
 * Date: 01/05/2015
 * 
 * The purpose of this class is to controll the game play UI
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;

public class GamePlayUI : MonoBehaviour {
	/*
	 * Properties of the UI
	 */
	public Text score;
	public Slider healthSlider;
	public Text time;
    public CursorLockMode wantedMode;
	public Canvas pausedUI;
	public CanvasGroup pausedUICanvasGroup;
	public RectTransform instructionPanel;
	public GameObject weapon;
	public AudioMixerSnapshot pausedAudio;
	public AudioMixerSnapshot unpausedAudio;

	PlayerMovement playerMovement;
	bool instructionFate = false;

	// Use this for initialization
	void Start () {
		pausedUI.enabled = false;
		pausedUICanvasGroup.interactable = false;
		pausedUICanvasGroup.blocksRaycasts = false;
		playerMovement = GameObject.Find ("_Player").GetComponent <PlayerMovement> ();
		if(GameMaster.newPlayer){
			Resume();
		}
		Invoke("StartFate", 3);
	}
	
	// Update is called once per frame
	void Update () {
		// Update the status value
		score.text = "Score: "+GameMaster.currentScore;
		healthSlider.value = PlayerHealth.currentHealth;
		time.text = GameMaster.GetTime (GameMaster.playedTime);
		// Escape key to pause
		if (Input.GetKeyDown(KeyCode.Escape)){
			if(!pausedUI.enabled)
				Resume();
		}

		if(instructionFate){
			instructionPanel.sizeDelta = new Vector2(instructionPanel.sizeDelta.x, instructionPanel.sizeDelta.y - Time.deltaTime * 150f);
			if(instructionPanel.sizeDelta.y <= 0)
				instructionFate = false;
		}
	}

	void StartFate(){
		instructionFate = true;
	}

	/*
	 * Pause and unpause the game
	 */
	public void Resume(){
		// Show paused menu
		pausedUI.enabled = !pausedUI.enabled;
		pausedUICanvasGroup.interactable = !pausedUICanvasGroup.interactable;
		pausedUICanvasGroup.blocksRaycasts = !pausedUICanvasGroup.blocksRaycasts;
		// Hide the weapons
		playerMovement.enabled = !playerMovement.enabled;
		weapon.SetActive (!weapon.activeSelf);
		Pause ();
	}
	/*
	 * Return back to main menu
	 */
	public void BackToMenu(){
		Pause ();
		Application.LoadLevel("MainMenu");
	}
	/*
	 * Pause the time in the game
	 */
	void Pause(){
		Time.timeScale = Mathf.Approximately(Time.timeScale, 0) ? 1 : 0;
		// Change the music
		if (Mathf.Approximately(Time.timeScale, 0))
			pausedAudio.TransitionTo (0.01f);
		else
			unpausedAudio.TransitionTo (0.01f);
	}
}
