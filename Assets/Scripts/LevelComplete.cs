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
	private bool zoomOut=false;
	private bool fadeIn=false;
	public Vector3 cameraEndPosition;
    public float cameraZoom = 20.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (debugEndLevel) {
			debugEndLevel = false;
			EndLevel ();
		}
	}

	void FixedUpdate(){
	//zoom camera out, fade in, then animate once
		if (zoomOut) {
			cameraFocus.transform.position = Vector3.MoveTowards (cameraFocus.transform.position,cameraEndPosition, 15.0f*Time.deltaTime);
			Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, cameraZoom, 6.0f * Time.deltaTime);
			if (cameraFocus.transform.position == cameraEndPosition) {
				zoomOut = false;
				fadeIn = true;
			}
		}
		if (fadeIn) 
		{
			constellation.GetComponent<SpriteRenderer>().color =  new Color (255f, 255f, 255f, constellation.GetComponent<SpriteRenderer>().color.a+1.25f*Time.deltaTime);
			print (constellation.GetComponent<SpriteRenderer> ().color.a);
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
		cameraFocus.GetComponent<CameraFocusUpdated>().control =false;//stop the camera from following the player
		zoomOut = true;
		//x25 y 15
	}
}
