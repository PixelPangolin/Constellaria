using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class CodexManager : MonoBehaviour {

	public GameObject selectionMenu;
	public GameObject entry;
	public GameObject button;
	public GameObject UIaudio;
	public AudioSource audioSource;
	public int levelNumber;
	private bool paused = false;

	public void Start(){
		if (PlayerPrefsManager.GetLastLevelPlayed () <= levelNumber) {
			button.SetActive (false);
		}
		audioSource.Pause();
		entry.SetActive (false);

		Component[] list = UIaudio.GetComponentsInChildren<AudioSource> ();
		foreach (AudioSource source in list) {
			source.volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetSoundEffectVolume ();
		}

	}

	public void LoadEntry()
	{
		selectionMenu.SetActive(false);
		entry.SetActive(true);
	}

	public void LoadSelection(){
		selectionMenu.SetActive(true);
		entry.SetActive(false);
	}

	public void LoadLevel(string scene){
		selectionMenu.SetActive(true);
		entry.SetActive(false);
		SceneManager.LoadScene (scene);
	}

	public void PauseVoice()
	{
		if (audioSource != null) {
			audioSource.Pause ();
			paused = true;
		}
	}

	public void UnPauseVoice()
	{
		if (audioSource != null) {
			if (audioSource.isPlaying && !paused) {
				audioSource.time = 0.0f;
				paused = false;
			} else {
				audioSource.Play ();
				paused = false;
			}
		}
	}
}
