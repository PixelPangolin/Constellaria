using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	//Audio Sources 
	public AudioSource playerAudio;
	public AudioSource cameraAudio;

	//Ambient Sound 
	public AudioClip backgroundTrack;

	//Sound Effects
	public AudioClip makeLine;
	public AudioClip breakLine;
	public AudioClip deathSound;
	public AudioClip pullRope;
	public AudioClip tautRope;
	public AudioClip endLevelSound;
	public AudioClip walkingOnCave;
	public AudioClip walkingOnLine;
	public AudioClip jumpSound;
	public AudioClip fallSound;
	public AudioClip landOnLineSound;
	public AudioClip landOnGroundSound;

	//AudioLevels
	public float defaultSXVolume;
	public float defaultMusicVolume;
	public float defaultMasterVolume;



	// Use this for initialization
	void Start () {
		playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
		cameraAudio = GameObject.FindGameObjectWithTag("CameraFocus").GetComponent<AudioSource>();
		PlayerPrefsManager.SetMasterVolume (defaultMasterVolume);
		PlayerPrefsManager.SetMusicVolume (defaultMusicVolume);
		PlayerPrefsManager.SetSoundEffectVolume (defaultSXVolume);


		cameraAudio.volume = PlayerPrefsManager.GetMusicVolume ();
		cameraAudio.clip = backgroundTrack;
		cameraAudio.Play ();

	}
	
	// Update is called once per frame
	void Update () {
		cameraAudio.volume = PlayerPrefsManager.GetMusicVolume ();
		//debug , remove after done debugging
		PlayerPrefsManager.SetMasterVolume (defaultMasterVolume);
		PlayerPrefsManager.SetMusicVolume (defaultMusicVolume);
		PlayerPrefsManager.SetSoundEffectVolume (defaultSXVolume);
	}

	public void playMakeLine(){
		playerAudio.PlayOneShot(makeLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playBreakLine(){
		playerAudio.PlayOneShot(breakLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playDeathSound(){
		playerAudio.PlayOneShot(deathSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playPullRope(){
		playerAudio.PlayOneShot(pullRope ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playTautRope(){
		playerAudio.PlayOneShot(tautRope ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playEndLevelSound(){
		playerAudio.PlayOneShot(endLevelSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playWalkCave(){
		playerAudio.PlayOneShot(walkingOnCave ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playWalkLine(){
		playerAudio.PlayOneShot(walkingOnLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playJump(){
		playerAudio.PlayOneShot(jumpSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playFall(){
		playerAudio.PlayOneShot(fallSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playLandLine(){
		playerAudio.PlayOneShot(landOnLineSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}

	public void playLandGround(){
		playerAudio.PlayOneShot(landOnGroundSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());
	}
}
