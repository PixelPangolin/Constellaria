using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class MainMenu : MonoBehaviour {
	

	public Slider masterSlider;
	public Slider effectsSlider;
	public Slider ambienceSlider;
	public Slider musicSlider;
	public bool cutsceneDebug = false;
    public GameObject loop;
    public GameObject cutscene;

    public GameObject mainMenu;

	void Start () {
		masterSlider.value = PlayerPrefsManager.GetMasterVolume();
		effectsSlider.value = PlayerPrefsManager.GetSoundEffectVolume();
		ambienceSlider.value = PlayerPrefsManager.GetAmbienceVolume();
		musicSlider.value = PlayerPrefsManager.GetMusicVolume();
	}

	public void MMStartGame ()//TODO should be set via savefile, and default to a level by string name
	{
		
		if (cutsceneDebug)
			SceneManager.LoadScene (PlayerPrefsManager.GetLastLevelPlayed());
		else {
			cutscene.SetActive (true);
			loop.SetActive (false);
			GameObject.FindGameObjectWithTag ("GameController").GetComponent<AudioSource> ().volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetMusicVolume ();
			Destroy (mainMenu);
		}

	}
	public void MMOpenCodex ()
	{
		SceneManager.LoadScene("CodexDisplay");
	}
	public void CDOpenMainMenu ()
	{
		SceneManager.LoadScene(0);
		//print (PlayerPrefsManager.GetMusicVolume());
	}
	public void CDOpenSceneByName (string sceneName)
	{
		SceneManager.LoadScene(sceneName);
	}
	public void OnMasterSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetMasterVolume (masterSlider.value);
		//print (masterSlider.value +" "+ PlayerPrefsManager.GetMasterVolume());
		PlayerPrefs.Save ();
	}
	public void OnEffectsSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetSoundEffectVolume (effectsSlider.value);
		//print (effectsSlider.value +" "+ PlayerPrefsManager.GetSoundEffectVolume());
		PlayerPrefs.Save ();
	}
	public void OnAmbienceSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetAmbienceVolume (ambienceSlider.value);
		//print (ambienceSlider.value +" "+ PlayerPrefsManager.GetAmbienceVolume());
		PlayerPrefs.Save ();
	}
	public void OnMusicSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetMusicVolume (musicSlider.value);
		//print (musicSlider.value +" "+ PlayerPrefsManager.GetMusicVolume());
		PlayerPrefs.Save ();
	}

	public void QuitGame ()
	{
		Application.Quit ();
	}
}
