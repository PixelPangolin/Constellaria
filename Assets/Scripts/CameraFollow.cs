using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow: MonoBehaviour {

    public Controller2D player;
    public Vector2 focusAreaSize;

    public float verticalOffset;

    FocusArea focusArea;

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
	
	// Update is called once per frame
	void LateUpdate () {
        focusArea.Update(player.collider.bounds);

        Vector2 focusPosition = focusArea.center + Vector2.up * verticalOffset;

        transform.position = (Vector3)focusPosition + Vector3.forward * -10;

	}

    // Draws the focusArea
    void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0, 0.5f);
        Gizmos.DrawCube(focusArea.center, focusAreaSize);
    }
}
