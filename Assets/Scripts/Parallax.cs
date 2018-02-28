using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour {

	public Camera camera;
	public float speed;

	private Vector3 oldCameraPosition;
	// Use this for initialization
	void Start () {
		oldCameraPosition = camera.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
		if (oldCameraPosition != camera.transform.position) {
			this.transform.Translate ((camera.transform.position.x-oldCameraPosition.x)*speed, (camera.transform.position.y-oldCameraPosition.y)*speed*Time.deltaTime, 0, Space.World);
			//this.transform.position. (camera.transform.position.x, camera.transform.position.y, depth);
			oldCameraPosition = camera.transform.position;
		}

	}
}
