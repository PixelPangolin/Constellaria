using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour {

	public GameObject player;
	public string trigger;
	public float delay;
	// Use this for initialization
	void Start () {
		player.GetComponent<Animator> ().applyRootMotion = false;
		player.GetComponent<Animator>().SetTrigger(trigger);
		StartCoroutine(WaitThenControl(delay));
	}

	IEnumerator WaitThenControl(float t){
		player.GetComponent<GrapplingHook> ().enabled = false;
		player.GetComponent<PlayerInput> ().enabled = false;
		yield return new WaitForSeconds (t);
		player.GetComponent<Animator> ().applyRootMotion = true;
		player.GetComponent<GrapplingHook> ().enabled = true;
		player.GetComponent<PlayerInput> ().enabled = true;

	}
	// Update is called once per frame
	void Update () {
		
	}
}
