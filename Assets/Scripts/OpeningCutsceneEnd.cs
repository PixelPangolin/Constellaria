using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class OpeningCutsceneEnd : MonoBehaviour {

	// Use this for initialization
	void Start () {
		SceneManager.LoadScene(PlayerPrefsManager.GetLastLevelPlayed());
    }
	
	// Update is called once per frame
	void Update () {
		
	}
}
