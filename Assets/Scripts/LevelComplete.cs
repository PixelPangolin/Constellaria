using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour {

	public AudioSource audio;
	public AudioClip endLevelSound;
	public GameObject cameraFocus;
	public GameObject constellation;
	public bool debugEndLevel =false; 
	private bool zoomOut=false;
	private bool fadeIn=false;
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
			cameraFocus.transform.position = Vector3.MoveTowards (cameraFocus.transform.position,new Vector3(15,16,-10), 15.0f*Time.deltaTime);
			Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, 20.0f, 6.0f * Time.deltaTime);
			if (cameraFocus.transform.position == new Vector3 (15, 16, -10)) {
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
				SceneManager.LoadScene(0);
			}
		}
	}


	public void EndLevel(){
		print ("Level complete");
		//SOUND: Completing the puzzle
		audio.PlayOneShot(endLevelSound ,0.5f);//TODO get volume from something
		// Write script here for what occurs when the level is complete
		cameraFocus.GetComponent<CameraFocus>().control =false;//stop the camera from following the player
		zoomOut = true;
		//x25 y 15
	}
}
