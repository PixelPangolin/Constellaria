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
    public float makeLineVolumne = 1;
	public AudioClip breakLine;
    public float breakLineVolumne = 1;
	public AudioClip deathSound;
    public float deathSoundVolumne = 1;
	public AudioClip pullRope;
    public float pullRopeVolumne = 1;
	public AudioClip tautRope;
    public float tautRopeVolumne = 1;
	public AudioClip endLevelSound;
    public float endLevelVolumne= 1;
	public AudioClip walkingOnCave;
    public float walkCaveVolumne = 1;
	public AudioClip walkingOnLine;
    public float walkLineVolumne = 1;
	public AudioClip jumpSound;
    public float jumpVolumne = 1;
	public AudioClip fallSound;
    public float fallSoundVolumne = 1;
	public AudioClip landOnLineSound;
    public float landOnLineVolumne = 1;
	public AudioClip landOnGroundSound;
    public float landOnGroundVolumne = 1;



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
        playerAudio.PlayOneShot(makeLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*makeLineVolumne);
	}

	public void playBreakLine(){
        playerAudio.PlayOneShot(breakLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*breakLineVolumne);
	}

	public void playDeathSound(){
        playerAudio.PlayOneShot(deathSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*deathSoundVolumne);
	}

	public void playPullRope(){
        playerAudio.PlayOneShot(pullRope ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*pullRopeVolumne);
	}

	public void playTautRope(){
        playerAudio.PlayOneShot(tautRope ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*tautRopeVolumne);
	}

	public void playEndLevelSound(){
        playerAudio.PlayOneShot(endLevelSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*endLevelVolumne);
	}

	public void playWalkCave(){
        playerAudio.PlayOneShot(walkingOnCave ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*walkCaveVolumne);
	}

	public void playWalkLine(){
        playerAudio.PlayOneShot(walkingOnLine ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*walkLineVolumne);
	}

	public void playJump(){
        playerAudio.PlayOneShot(jumpSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*jumpVolumne);
	}

	public void playFall(){
        playerAudio.PlayOneShot(fallSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*fallSoundVolumne);
	}

	public void playLandLine(){
        playerAudio.PlayOneShot(landOnLineSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*landOnLineVolumne);
	}

	public void playLandGround(){
        playerAudio.PlayOneShot(landOnGroundSound ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume()*landOnGroundVolumne);
	}
}
