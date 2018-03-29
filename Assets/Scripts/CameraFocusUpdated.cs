// This code dictates the camera movement
// Used the Unity3D manual for this code
// Updated to attempt to curb jitteriness
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFocusUpdated : MonoBehaviour
{

    // These define the object the camera follows, the speed it follows at and the max distance before the camera moves in x or y
    public GameObject player;
    public float max_x = 4;
    public float max_y = 1;
    public bool control = true;
    bool shouldMovex = false;
    bool shouldMovey = false;
    private Vector3 posLastFrame;
    public float minvelocity = 2;
    public float defaultvelocity = 4;


    // Use this for initialization
    void Start()
    {
        // initializes the camera to the player position
        //transform.position = player.transform;
        posLastFrame = player.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //control = false if something else wants the camera, i.e. Constellation animation
        if (control)
        {
            // find the velocity of the player from frame to frame
            var posThisFrame = player.transform.position;
            var vel = (posThisFrame - posLastFrame) / Time.deltaTime;
            float velmag = vel.magnitude;

            // if the player stops moving, the camera will still keep moving until it catches up
            if (velmag < minvelocity)
            {
                velmag = defaultvelocity;
            }

            //create position variables for the player character and the camera focus
            var playerPos = player.transform.position;
            var selfPos = transform.position;

            //finds the distance vector between the character and the camera
            var distancetoPlayer = playerPos - selfPos;
            float lineartoPlayer = Mathf.Sqrt(((playerPos.x - selfPos.x) * (playerPos.x - selfPos.x)) + ((playerPos.y - selfPos.y) * (playerPos.y - selfPos.y)));

            // booleans to decide whether the camera should move

            if (Mathf.Abs(distancetoPlayer.x) > max_x)
            {
                shouldMovex = true;
            }

            if (Mathf.Abs(distancetoPlayer.y) > max_y)
            {
                shouldMovey = true;
            }
           
            // starts the camera moving if x or y exceed their max distances
            if (shouldMovex || shouldMovey)
            {
                
                // distance camera moves towards the player
                float step = velmag * Time.deltaTime;
                Debug.Log("calculated step as " + step);

                // if the distance to the player is greater than what the step would be
                if (lineartoPlayer > step) {
                    Debug.Log("lineartoplayer > step, moving camera transform by " + step);
                    transform.position = Vector3.MoveTowards(selfPos, playerPos, step);
                }
                // if the distance to the player is less than what the step would be set the step to the distance to the player
                else
                {
                                        Debug.Log("moving camera transform by " + step);
                    transform.position = Vector3.MoveTowards(selfPos, playerPos, lineartoPlayer);
                    shouldMovex = false;
                    shouldMovey = false;
                }
                

            }
            // saves the position of this frame to calculate the velocity for the next frame
            posLastFrame = posThisFrame;
        }
    }
}
