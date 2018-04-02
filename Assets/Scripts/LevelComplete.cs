using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour {

	public string nextSceneName;
	public AudioSource audio;
	public AudioClip endLevelSound;
	public GameObject cameraFocus;
	public GameObject constellation;
	public bool debugEndLevel =false; 
	public bool debugResetSaveGame =false; 
	private bool zoomOut=false;
	private bool moveCamera=false;
	private bool fadeIn=false;
	private bool endLevel=false;
	public float fadeInSpeed = 1.25f;
	public Vector3 cameraEndPosition;
	public float cameraMoveSpeed = 15.0f;
	public float cameraZoom = 5.0f;
	public float cameraZoomSpeed = 6.0f;
	// Use this for initialization
	void Start () { 
		//TODO: uncomment these for after the demo
		//Note that this version will be easy to mess up by replaying old levels through codex
		//maybe use build index instead, and only take higher numbers? Could also use that to return to codex after
		//PlayerPrefsManager.SetLastLevelPlayed(SceneManager.GetActiveScene().name);
		//PlayerPrefs.Save;
	}
	
	// Update is called once per frame
	void Update () {
		if (debugEndLevel) {
			debugEndLevel = false;
			EndLevel ();
		}
		if (debugResetSaveGame) {
			PlayerPrefsManager.SetLastLevelPlayed("Demo3-TutorialLevel");
			PlayerPrefs.Save();
			debugEndLevel = false;
		}
	}

	void FixedUpdate(){
	//zoom camera out, fade in, then animate once
		if (zoomOut) {
			Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, cameraZoom, cameraZoomSpeed * Time.deltaTime);
			if (Camera.main.orthographicSize == cameraZoom) {
				zoomOut = false;
			}
		}
		if (moveCamera) {
			cameraFocus.transform.position = Vector3.MoveTowards (cameraFocus.transform.position,cameraEndPosition, cameraMoveSpeed*Time.deltaTime);
			if (cameraFocus.transform.position == cameraEndPosition) {
				moveCamera = false;

			}
		}
		if (!moveCamera && !zoomOut && endLevel) { fadeIn = true;
		} 
		if (fadeIn) 
		{
			constellation.GetComponent<SpriteRenderer>().color =  new Color (255f, 255f, 255f, constellation.GetComponent<SpriteRenderer>().color.a+fadeInSpeed*Time.deltaTime);
			//print (constellation.GetComponent<SpriteRenderer> ().color.a);
			if (constellation.GetComponent<SpriteRenderer> ().color.a >= 6) {
				fadeIn = false;
				SceneManager.LoadScene(nextSceneName);
			}
		}
	}


	public void EndLevel(){
		print ("Level complete");
		//SOUND: Completing the puzzle
		GameObject.Find("GameController").GetComponent<AudioController>().playEndLevelSound();
		// Write script here for what occurs when the level is complete

		cameraFocus.GetComponent<CameraFollow>().control = false; //stop the camera from following the player
		zoomOut = true;
		moveCamera = true;
		endLevel = true;
	}
}
