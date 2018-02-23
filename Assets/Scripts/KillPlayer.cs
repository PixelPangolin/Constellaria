using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {

	}

	// Update is called once per frame
	void Update () {

	}

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag ("Player")) 
		{
			if (other.gameObject.GetComponent<GrapplingHook> ().currentNode) 
			{
				other.gameObject.transform.position = other.gameObject.GetComponent<GrapplingHook> ().currentNode.transform.position;
			} 
			else 
			{
				Application.LoadLevel (Application.loadedLevel);
			}
		}
	}
}