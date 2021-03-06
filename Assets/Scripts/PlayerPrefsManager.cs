// from https://answers.unity.com/questions/1117712/master-audio-source-volume-control-with-slider.html
using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class PlayerPrefsManager : MonoBehaviour {

	const string MASTER_VOLUME_KEY = "master_volume";
	const string MUSIC_VOLUME_KEY = "music_volume";
	const string SE_VOLUME_KEY = "sound_effect_volume";
    const string AMBIENCE_KEY = "ambience_volume";
	const string PROGRESS_KEY = "Last_Level_Played";

	public static void DefaultPlayerPrefs(){
		if (!PlayerPrefs.HasKey ("master_volume"))	PlayerPrefsManager.SetMasterVolume (1.0f);
		if (!PlayerPrefs.HasKey ("music_volume"))	PlayerPrefsManager.SetMusicVolume (1.0f);
		if (!PlayerPrefs.HasKey ("sound_effect_volume"))	PlayerPrefsManager.SetSoundEffectVolume (1.0f);
		if (!PlayerPrefs.HasKey ("ambience_volume"))	PlayerPrefsManager.SetAmbienceVolume (1.0f);
		if (!PlayerPrefs.HasKey ("Last_Level_Played"))	PlayerPrefsManager.SetLastLevelPlayed (3);
	}

	public static void SetLastLevelPlayed (int levelName) {
		PlayerPrefs.SetInt ("Last_Level_Played", levelName);
	}


	public static int GetLastLevelPlayed () {
		return PlayerPrefs.GetInt (PROGRESS_KEY, 3);
	}


	public static void SetMasterVolume (float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat ("master_volume", volume);
		} else {
			Debug.LogError ("Master Volume out of range");
		}
	}


	public static float GetMasterVolume () {
		return PlayerPrefs.GetFloat (MASTER_VOLUME_KEY, 1f);
	}
		
	public static void SetMusicVolume (float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat ("music_volume", volume);
		} else {
			Debug.LogError ("Music Volume out of range");
		}
	}


	public static float GetMusicVolume () {
		return PlayerPrefs.GetFloat (MUSIC_VOLUME_KEY, 1f);
	}

	public static void SetSoundEffectVolume (float volume) {
		if (volume >= 0f && volume <= 1f) {
			PlayerPrefs.SetFloat ("sound_effect_volume", volume);
		} else {
			Debug.LogError ("Sound Effect Volume out of range");
		}
	}


	public static float GetSoundEffectVolume () {
		return PlayerPrefs.GetFloat (SE_VOLUME_KEY, 1f);
	}

    public static void SetAmbienceVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
			PlayerPrefs.SetFloat("ambience_volume", volume);
        }
        else
        {
            Debug.LogError("Ambience Volume out of range");
        }
    }


    public static float GetAmbienceVolume()
    {
		return PlayerPrefs.GetFloat(AMBIENCE_KEY, 1f);
    }



}