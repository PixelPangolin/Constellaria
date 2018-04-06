using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelComplete : MonoBehaviour {

	public string nextSceneName;
	public AudioSource audio;
	public AudioClip endLevelSound;
	public GameObject cameraFocus;
	public GameObject constellation;
	public bool debugEndLevel =false; 
	public bool debugResetSaveGame =false; 
	private bool zoomOut=false;
	private bool moveCamera=false;
	private bool fadeIn=false;
	private bool endLevel=false;
	private bool endLevel2=false;
	private bool zoomIn=false;
	private bool moveCamera2=false;
	private bool fadeOut=false;
	private bool playAnim=false;
	public float endAnimationTime = 1.0f;
	public float fadeInSpeed = 1.25f;
	public Vector3 cameraEndPosition;
	public float cameraMoveSpeed = 15.0f;
	public float cameraZoom = 5.0f;
	public float cameraZoomSpeed = 6.0f;
    public List<GameObject> DeactivateWhenLevelCompleteZoomOut; // wanted to deactivate toturial stuff when you complete the level
	public List<GameObject> DeactivateWhenLevelCompleteZoomIn;
    public List<GameObject> ActivateWhenLevelComplete; // wanted to activate toturial stuff when you complete the level
	public Vector3 cameraEndPosition2;
	public float cameraMoveSpeed2 = 15.0f;
	public float cameraZoom2 = 5.0f;
	public float cameraZoomSpeed2 = 6.0f;
    // Use this for initialization
    void Start () { 
		//If current level is bigger farther than last level, save it as furthest level played
		int y = SceneManager.GetActiveScene().buildIndex;
		if (PlayerPrefsManager.GetLastLevelPlayed() < y){
			PlayerPrefsManager.SetLastLevelPlayed(y);
			PlayerPrefs.Save();
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (debugEndLevel) {
			debugEndLevel = false;
			EndLevel ();
		}
		if (debugResetSaveGame) {
			PlayerPrefsManager.SetLastLevelPlayed(3);
			PlayerPrefs.Save();
			debugResetSaveGame = false;
		}
	}

	void FixedUpdate(){
	//zoom camera out, fade in, then animate once
		if (zoomOut) {
			Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, cameraZoom, cameraZoomSpeed * Time.deltaTime);
			if (Camera.main.orthographicSize == cameraZoom) {
				zoomOut = false;
			}
		}
		if (moveCamera) {
			cameraFocus.transform.position = Vector3.MoveTowards (cameraFocus.transform.position,cameraEndPosition, cameraMoveSpeed*Time.deltaTime);
			if (cameraFocus.transform.position == cameraEndPosition) {
				moveCamera = false;

			}
		}
		if (!moveCamera && !zoomOut && endLevel) { fadeIn = true;
		} 
		if (fadeIn) 
		{
			constellation.GetComponent<SpriteRenderer>().color =  new Color (255f, 255f, 255f, constellation.GetComponent<SpriteRenderer>().color.a+fadeInSpeed*Time.deltaTime);
			//print (constellation.GetComponent<SpriteRenderer> ().color.a);
			if (constellation.GetComponent<SpriteRenderer> ().color.a >= 6) {
				fadeIn = false;
				zoomIn = true;
				moveCamera2 = true;

				//SceneManager.LoadScene(nextSceneName);
			}
		}

		if (zoomIn && !moveCamera && !zoomOut && endLevel) {
			Camera.main.orthographicSize = Mathf.MoveTowards (Camera.main.orthographicSize, cameraZoom2, cameraZoomSpeed2 * Time.deltaTime);
			if (Camera.main.orthographicSize == cameraZoom2) {
				zoomIn = false;
				endLevel2 = true;
			}
		}
		if (moveCamera2 && !moveCamera && !zoomOut && endLevel) {
			cameraFocus.transform.position = Vector3.MoveTowards (cameraFocus.transform.position,cameraEndPosition2, cameraMoveSpeed2*Time.deltaTime);
			if (cameraFocus.transform.position == cameraEndPosition2) {
				moveCamera2 = false;
				endLevel2 = true;
			}
		}
		if (!moveCamera && !zoomOut && endLevel && !moveCamera2 && !zoomIn && endLevel2) { 
			foreach (GameObject each in DeactivateWhenLevelCompleteZoomIn) //added to deactivate tutorial stuff
			{
				each.SetActive(false);
			}

			foreach (GameObject each in ActivateWhenLevelComplete) //added to active tutorial stuff
			{
				each.SetActive(true);
			}
			//SOUND TODO
			PlayerPrefsManager.SetLastLevelPlayed(PlayerPrefsManager.GetLastLevelPlayed()+1);
			PlayerPrefs.Save();
			StartCoroutine(WaitThenEnd(endAnimationTime));

		} 


	}


	public void EndLevel(){
		print ("Level complete");
		//SOUND: Completing the puzzle
		GameObject.Find("GameController").GetComponent<AudioController>().playEndLevelSound();
        // Write script here for what occurs when the level is complete
		foreach (GameObject each in DeactivateWhenLevelCompleteZoomOut) //added to deactivate tutorial stuff
        {
            each.SetActive(false);
        }


        cameraFocus.GetComponent<CameraFollow>().control = false; //stop the camera from following the player
		zoomOut = true;
		moveCamera = true;
		endLevel = true;
	}

	IEnumerator WaitThenEnd(float t){
		yield return new WaitForSeconds (t);
		SceneManager.LoadScene(nextSceneName);
	}
}
