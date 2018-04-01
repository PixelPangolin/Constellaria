using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow: MonoBehaviour {

    public Controller2D player;

    // For our character, a 3x3 or a 3x4 focusAreaSize is recommended
    public Vector2 focusAreaSize;

    // Determines if this script is in control of the camera
    [HideInInspector] public bool control = true;

    // Sets how our camera moves and the smoothing
    private float verticalOffset = 0;
    private float lookAheadDistanceX = 2;
    private  float lookSmoothTimeX = 0.5f;
    private float verticalSmoothTime = 0.1f;

    // The area around the player we focus on
    private FocusArea focusArea;

    // Used to calculate camera movement
    private float currentLookAheadX;
    private float playerLookAheadX;
    private float lookAheadDirectionX;
    private float smoothLookVelocityX;
    private float smoothVelocityY;

    private bool lookAheadStopped;


    // The area we will focus on and track around the player
    struct FocusArea {
        public Vector2 center;
        public Vector2 velocity;

        float left;
        float right;
        float top;
        float bottom;

        public FocusArea (Bounds targetBounds, Vector2 size)
        {
            left = targetBounds.center.x - size.x / 2;
            right = targetBounds.center.x + size.x / 2;

            bottom = targetBounds.min.y;
            top = targetBounds.min.y + size.y;

            velocity = Vector2.zero;

            center = new Vector2((left + right) / 2, (top + bottom) / 2);
        }

        // Updating the area's position when the target moves against the edges
        public void Update (Bounds targetBounds)
        {
            // Shifting for the x axis
            float shiftX = 0;

            if (targetBounds.min.x < left)
            {
                shiftX = targetBounds.min.x - left;
            }
            else if (targetBounds.max.x > right)
            {
                shiftX = targetBounds.max.x - right;
            }

            left += shiftX;
            right += shiftX;

            // Shifting for the y axis
            float shiftY = 0;

            if (targetBounds.min.y < bottom)
            {
                shiftY = targetBounds.min.y - bottom;
            }
            else if (targetBounds.max.y > top)
            {
                shiftY = targetBounds.max.y - top;
            }

            top += shiftY;
            bottom += shiftY;

            // Update center position, keep track of how the area has moved
            center = new Vector2((left + right) / 2, (top + bottom) / 2);
            velocity = new Vector2(shiftX, shiftY);

        }

    }

	// Use this for initialization
	void Start () {
        focusArea = new FocusArea(player.collider.bounds, focusAreaSize);
	}
	
	// LateUpdate is called once per frame only after Update has been called because the camera tracks objects that may have moved in Update
	void LateUpdate () {

        if (control)
        {
            focusArea.Update(player.collider.bounds);

            Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

            if (focusArea.velocity.x != 0)
            {
                lookAheadDirectionX = Mathf.Sign(focusArea.velocity.x);

                // Only want to set lookAheadX if the input is the same direction as the focusArea is moving
                if ((Mathf.Sign(player.playerInput.x) == Mathf.Sign(focusArea.velocity.x)) && player.playerInput.x != 0)
                {
                    lookAheadStopped = false;
                    playerLookAheadX = lookAheadDirectionX * lookAheadDistanceX;
                }
                else
                {
                    // Allows us to stop lookAhead prematurely when the character stops
                    if (!lookAheadStopped)
                    {
                        playerLookAheadX = currentLookAheadX + (lookAheadDirectionX * lookAheadDistanceX - currentLookAheadX) / 4f;
                        lookAheadStopped = true;
                    }
                }
            }

            // Smoothing
            currentLookAheadX = Mathf.SmoothDamp(currentLookAheadX, playerLookAheadX, ref smoothLookVelocityX, lookSmoothTimeX);
            focusPosition.y = Mathf.SmoothDamp(transform.position.y, focusPosition.y, ref smoothVelocityY, verticalSmoothTime);

            focusPosition += Vector2.right * currentLookAheadX;

            transform.position = (Vector3)focusPosition + Vector3.forward * -10;
        }
	}

    // Draws the focusArea
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }
}
