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
		PlayerPrefsManager.DefaultPlayerPrefs ();//sets prefs to default if they've never been set
		masterSlider.value = PlayerPrefsManager.GetMasterVolume();
		effectsSlider.value = PlayerPrefsManager.GetSoundEffectVolume();
		ambienceSlider.value = PlayerPrefsManager.GetAmbienceVolume();
		musicSlider.value = PlayerPrefsManager.GetMusicVolume();
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<AudioSource> ().volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetMusicVolume ();
		GameObject.Find ("WavesSounds").GetComponent<AudioSource> ().volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetAmbienceVolume ();
		GameObject.Find ("FireCrackleSounds").GetComponent<AudioSource> ().volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetSoundEffectVolume ();
		GameObject.Find ("Misc Sounds").GetComponent<AudioSource> ().volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetSoundEffectVolume ();

	}

	public void MMStartGame ()//TODO should be set via savefile, and default to a level by string name
	{
		
		if (cutsceneDebug || PlayerPrefsManager.GetLastLevelPlayed() > 3)
			SceneManager.LoadScene (PlayerPrefsManager.GetLastLevelPlayed());
		else {
			cutscene.SetActive (true);
			loop.SetActive (false);

			Destroy (mainMenu);
		}
	}
	public void MMUpdateSound ()
	{
		PlayerPrefs.Save ();
		GameObject.FindGameObjectWithTag ("GameController").GetComponent<AudioSource> ().volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetMusicVolume ();
		GameObject.Find ("WavesSounds").GetComponent<AudioSource> ().volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetAmbienceVolume ();
		GameObject.Find ("FireCrackleSounds").GetComponent<AudioSource> ().volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetSoundEffectVolume ();
		GameObject.Find ("Misc Sounds").GetComponent<AudioSource> ().volume = PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetSoundEffectVolume ();

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
		MMUpdateSound ();
	}
	public void OnEffectsSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetSoundEffectVolume (effectsSlider.value);
		//print (effectsSlider.value +" "+ PlayerPrefsManager.GetSoundEffectVolume());
		MMUpdateSound ();
	}
	public void OnAmbienceSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetAmbienceVolume (ambienceSlider.value);
		//print (ambienceSlider.value +" "+ PlayerPrefsManager.GetAmbienceVolume());
		MMUpdateSound ();
	}
	public void OnMusicSliderChange ()//TODO should be set via savefile
	{
		PlayerPrefsManager.SetMusicVolume (musicSlider.value);
		//print (musicSlider.value +" "+ PlayerPrefsManager.GetMusicVolume());
		MMUpdateSound ();
	}

	public void QuitGame ()
	{
		Application.Quit ();
	}
}
