// This code dictates the camera movement
// Used the Unity3D manual for this code
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocus : MonoBehaviour {

    // These define the object the camera follows, the speed it follows at and the max distance before the camera moves in x or y
    public GameObject player;
    public float max_x = 4;
    public float max_y = 1;
    public float speed = 8;
	public bool control = true;

	// Use this for initialization
	void Start () {
        // initializes the camera to the player position
        //transform.position = player.transform;
	}
	
	// Update is called once per frame
	void Update () {
        //control = false if something else wants the camera, i.e. Constellation animation
		if (control) {
			//create position variables for the player character and the camera focus
			var playerPos = player.transform.position;
			var selfPos = transform.position;

			//finds the distance vector between the character and the camera
			var distancetoPlayer = playerPos - selfPos;

			// booleans to decide whether the camera should move
			bool shouldMovex = (Mathf.Abs (distancetoPlayer.x) > max_x);
			bool shouldMovey = (Mathf.Abs (distancetoPlayer.y) > max_y);

			// starts the camera moving if x or y exceed their max distances
			if (shouldMovex || shouldMovey) {

				// if only x needs to move then the y will not change
				if (shouldMovey == false) {
					playerPos = new Vector3 (playerPos.x, selfPos.y, -10); //Moving in Z is bad, and should be static
				}
       
				float step = speed * Time.deltaTime;
				transform.position = Vector3.MoveTowards (selfPos, playerPos, step);
      
			}

		}
    }
}
