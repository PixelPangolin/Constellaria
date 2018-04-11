using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartAnimation : MonoBehaviour {

	public GameObject player;
	public string trigger;
	public float delay;
	private bool inControl = false;
	public Player directionInput;
	public Vector2 direction;

	// Use this for initialization
	void Start () {
	//	player.GetComponent<Animator> ().applyRootMotion = false;
		player.GetComponent<Animator>().SetTrigger(trigger);
		StartCoroutine(WaitThenControl(delay));
	}

	IEnumerator WaitThenControl(float t){
		inControl = false;
		//player.GetComponent<GrapplingHook> ().enabled = false;
		transform.GetComponent<GrapplingHook> ().disabled = true;
		player.GetComponent<PlayerInput> ().enabled = false;
		yield return new WaitForSeconds (t);
	//	player.GetComponent<Animator> ().applyRootMotion = false;
		//player.GetComponent<GrapplingHook> ().enabled = true;
		transform.GetComponent<GrapplingHook> ().disabled = false;
		player.GetComponent<PlayerInput> ().enabled = true;
		inControl = true;

	}
	// Update is called once per frame
	void Update () {
		if (!inControl) {
			directionInput.SetDirectionalInput (direction);
		}
	}
}
