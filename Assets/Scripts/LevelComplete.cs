using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelComplete : MonoBehaviour {

	public AudioSource audio;
	public AudioClip endLevelSound;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void EndLevel(){
		print ("Level complete");
		//SOUND: Completing the puzzle
		audio.PlayOneShot(endLevelSound ,0.5f);//TODO get volume from something
		// Write script here for what occurs when the level is complete
	}
}
