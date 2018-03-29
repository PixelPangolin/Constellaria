using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioController : MonoBehaviour {

	//Audio Sources 
	public AudioSource playerAudio;
	public AudioSource cameraAudio;
    public AudioSource ambienceAudio;

    //AudioLevels
    public float defaultSXVolume;
    public float defaultMusicVolume;
    public float defaultAmbience;
    public float defaultMasterVolume;

	//Level Music
	public AudioClip backgroundTrack;

    //Level Ambience
    public AudioClip ambienceTrack;

	//Sound Effects
	public AudioClip makeLine;
    public float makeLineVolume = 1;
	public AudioClip breakLine;
    public float breakLineVolume = 1;
	public AudioClip deathSound;
    public float deathSoundVolume = 1;
	public AudioClip pullRope;
    public float pullRopeVolume = 1;
	public AudioClip tautRope;
    public float tautRopeVolume = 1;
	public AudioClip endLevelSound;
    public float endLevelVolume= 1;
	public AudioClip walkingOnCave;
    public float walkCaveVolume = 1;
	public AudioClip walkingOnLine;
    public float walkLineVolume = 1;
	public AudioClip jumpSound;
    public float jumpVolume = 1;
	public AudioClip fallSound;
    public float fallSoundVolume = 1;
	public AudioClip landOnLineSound;
    public float landOnLineVolume = 1;
	public AudioClip landOnGroundSound;
    public float landOnGroundVolume = 1;



	// Use this for initialization
	void Start () {
		playerAudio = GameObject.FindGameObjectWithTag("Player").GetComponent<AudioSource>();
		cameraAudio = GameObject.FindGameObjectWithTag("CameraFocus").GetComponent<AudioSource>();
        ambienceAudio = gameObject.GetComponent<AudioSource>();

		PlayerPrefsManager.SetMasterVolume (defaultMasterVolume);
		PlayerPrefsManager.SetMusicVolume (defaultMusicVolume);
		PlayerPrefsManager.SetSoundEffectVolume (defaultSXVolume);
        PlayerPrefsManager.SetAmbienceVolume(defaultAmbience);


        cameraAudio.volume = PlayerPrefsManager.GetMasterVolume() * PlayerPrefsManager.GetMusicVolume ();
		cameraAudio.clip = backgroundTrack;
        cameraAudio.loop = true;
		cameraAudio.Play ();

        ambienceAudio.volume = PlayerPrefsManager.GetMasterVolume() * PlayerPrefsManager.GetAmbienceVolume();
        ambienceAudio.loop = true;
        ambienceAudio.clip = ambienceTrack;
        ambienceAudio.Play();

	}
	
	// Update is called once per frame
	void Update () {
		//debug , remove after done debugging
		PlayerPrefsManager.SetMasterVolume (defaultMasterVolume);
		PlayerPrefsManager.SetMusicVolume (defaultMusicVolume);
		PlayerPrefsManager.SetSoundEffectVolume (defaultSXVolume);

        cameraAudio.volume = PlayerPrefsManager.GetMasterVolume() * PlayerPrefsManager.GetMusicVolume();
        ambienceAudio.volume = PlayerPrefsManager.GetMasterVolume() * PlayerPrefsManager.GetAmbienceVolume();

	}

	public void playMakeLine(){
        playerAudio.PlayOneShot(makeLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*makeLineVolume);
	}

	public void playBreakLine(){
        playerAudio.PlayOneShot(breakLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*breakLineVolume);
	}

	public void playDeathSound(){
        playerAudio.PlayOneShot(deathSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*deathSoundVolume);
	}

	public void playPullRope(){
        playerAudio.PlayOneShot(pullRope ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*pullRopeVolume);
	}

	public void playTautRope(){
        playerAudio.PlayOneShot(tautRope ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*tautRopeVolume);
	}

	public void playEndLevelSound(){
        playerAudio.PlayOneShot(endLevelSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*endLevelVolume);
	}

	public void playWalkCave(){
        playerAudio.PlayOneShot(walkingOnCave ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*walkCaveVolume);
	}

	public void playWalkLine(){
        playerAudio.PlayOneShot(walkingOnLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*walkLineVolume);
	}

	public void playJump(){
        playerAudio.PlayOneShot(jumpSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*jumpVolume);
	}

	public void playFall(){
        playerAudio.PlayOneShot(fallSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*fallSoundVolume);
	}

	public void playLandLine(){
        playerAudio.PlayOneShot(landOnLineSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*landOnLineVolume);
	}

	public void playLandGround(){
        playerAudio.PlayOneShot(landOnGroundSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*landOnGroundVolume);
	}
}
