using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;




public class MainMenu : MonoBehaviour {
	
	public void StartGame ()//TODO should be set via savefile, and default to a level by string name
	{
		SceneManager.LoadScene(1);
	}

	public void QuitGame ()
	{
		Application.Quit ();
	}
}
