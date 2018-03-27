using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class Controller2D : RaycastController {

    // Maximum slope the player can climb in degrees
    private float maxSlopeAngle = 80;

    public CollisionInfo collisions;

    public struct CollisionInfo
    {
        public bool above;
        public bool below;
        public bool left;
        public bool right;

        public bool climbingSlope;
        public bool descendingSlope;

        public float slopeAngle;
        public float slopeAngleOld;

        public Vector2 moveAmountOld;

        public void Reset()
        {
            above = false;
            below = false;
            left = false;
            right = false;
            climbingSlope = false;
            descendingSlope = false;
            slopeAngleOld = slopeAngle;
            slopeAngle = 0;
        }
    }

    public void Move(Vector2 moveAmount)
    {
        UpdateRaycastOrigins();
        collisions.Reset();
        collisions.moveAmountOld = moveAmount;

        if (moveAmount.y < 0)
        {
            DescendSlope(ref moveAmount);
        }

        // Check for collisions if player is moving
        if (moveAmount.x != 0)
        {
            HorizontalCollisions(ref moveAmount);
        }

        if (moveAmount.y != 0)
        {
            VerticalCollisions(ref moveAmount);
        }
        
        // Apply transform
        transform.Translate(moveAmount);
    }

    // Passes a reference of moveAmount rather than making a copy
    void HorizontalCollisions(ref Vector2 moveAmount)
    {
        // Where the ray should come from
        Vector2 rayOrigin;

        // Direction of movement and length of ray
        float directionX = Mathf.Sign(moveAmount.x);
        float rayLength = Mathf.Abs(moveAmount.x) + skinWidth;

        for (int i = 0; i < horizontalRayCount; i++)
        {
            // Jumping up or falling changes the direction of the rays
            if (directionX == -1)
            {
                rayOrigin = raycastOrigins.bottomLeft;
            }
            else
            {
                rayOrigin = raycastOrigins.bottomRight;
            }

            // Modifies the rayOrigin to cast from where we will be and check if it hits
            // Objects we collide with are determined by a layer mask
            rayOrigin += Vector2.up * (horizontalRaySpacing * i);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.right * directionX, Color.red);

            if (hit)
            {
                // Do not collide horizontally if it's a node
                if (hit.collider.tag == "Node")
                {
                    // TODO: Czeto's grappling hook modifications

                    continue;
                }

                // Get angle of surface we've hit - for slope moving purposes
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                
                if (i == 0 && slopeAngle <= maxSlopeAngle)
                {
                    // Preventing two crossed slopes from making player movement awkward
                    if (collisions.descendingSlope)
                    {
                        collisions.descendingSlope = false;
                        moveAmount = collisions.moveAmountOld;
                    }

                    float distanceToSlopeStart = 0;
                    
                    // Makes sure character climbs slopes smoothly
                    if (slopeAngle != collisions.slopeAngleOld)
                    {
                        distanceToSlopeStart = hit.distance - skinWidth;
                        moveAmount.x -= distanceToSlopeStart * directionX;
                    }

                    ClimbSlope(ref moveAmount, slopeAngle);
                    moveAmount.x += distanceToSlopeStart * directionX;
                }

                if (!collisions.climbingSlope || slopeAngle > maxSlopeAngle)
                {
                    // Sets our moveAmount to the amount needed to get from current position to the location hit
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    rayLength = hit.distance;

                    // Stops jittering when hitting an obstacle in the middle of a climb
                    if (collisions.climbingSlope)
                    {
                        moveAmount.y = Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x);
                    }

                    // Indicates if we will collide horizontally
                    if (directionX == -1)
                    {
                        collisions.left = true;
                    }
                    else
                    {
                        collisions.right = true;
                    }
                }
            }
        }
    }

    // Passes a reference of moveAmount rather than making a copy
    void VerticalCollisions(ref Vector2 moveAmount)
    {
        // Direction of movement and length of ray
        float directionY = Mathf.Sign(moveAmount.y);
        float rayLength = Mathf.Abs(moveAmount.y) + skinWidth;
        
        for (int i = 0; i < verticalRayCount; i++)
        {

            // Where the ray should come from
            Vector2 rayOrigin;

            // Jumping up or falling changes the direction of the rays
            if (directionY == -1)
            {
                rayOrigin = raycastOrigins.bottomLeft;
            }
            else
            {
                rayOrigin = raycastOrigins.topLeft;
            }

            // Modifies the rayOrigin to cast from where we will be and check if it hits
            // Objects we collide with are determined by a layer mask
            rayOrigin += Vector2.right * (verticalRaySpacing * i + moveAmount.x);
            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, collisionMask);

            Debug.DrawRay(rayOrigin, Vector2.up * directionY, Color.red);

            if (hit)
            {
                // Do not collide vertically if it is a node
                if (hit.collider.tag == "Node")
                {
                    // TODO: Czeto's grappling hook modifications
                    continue;
                }

                // Sets our moveAmount to the amount needed to get from current position to the location hit
                moveAmount.y = (hit.distance - skinWidth) * directionY;
                rayLength = hit.distance;

                // Stops jittering when hitting an obstacle above in the middle of a climb
                if (collisions.climbingSlope)
                {
                    moveAmount.x = moveAmount.y / Mathf.Tan(collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign(moveAmount.x);
                }

                // Indicates if we will collide vertically
                if (directionY == -1)
                {
                    collisions.below = true;
                }
                else
                {
                    collisions.above = true;
                }
            }
        }

        // Smooth transition from moving up different slopes
        if (collisions.climbingSlope)
        {
            float directionX = Mathf.Sign(moveAmount.x);
            rayLength = Mathf.Abs(moveAmount.x) + skinWidth;
            Vector2 rayOrigin;

            if (directionY == -1)
            {
                rayOrigin = raycastOrigins.bottomLeft + Vector2.up * moveAmount.y;
            }
            else
            {
                rayOrigin = raycastOrigins.topLeft + Vector2.up * moveAmount.y;
            }

            RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, collisionMask);

            if (hit)
            {
                float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
                
                if (slopeAngle != collisions.slopeAngle)
                {
                    moveAmount.x = (hit.distance - skinWidth) * directionX;
                    collisions.slopeAngle = slopeAngle;
                }
            }
        }
    }

    void ClimbSlope(ref Vector2 moveAmount, float slopeAngle)
    {
        // Treat the moveAmount on x as the same as flat ground
        // Distance we need to travel vertically and horizontally can be found using trigonometry
        float moveDistance = Mathf.Abs(moveAmount.x);

        // Calculating new moveAmount - converting degrees to radians
        float climbmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
        
        if (moveAmount.y <= climbmoveAmountY)
        {
            moveAmount.y = climbmoveAmountY;
            moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);

            // Setting below to true allows for jumping
            collisions.below = true;
            collisions.climbingSlope = true;
            collisions.slopeAngle = slopeAngle;
        }
    }

    void DescendSlope(ref Vector2 moveAmount)
    {
        // Casts a ray downwards as you descend the slope
        float directionX = Mathf.Sign(moveAmount.x);
        Vector2 rayOrigin;

        if (directionX == -1)
        {
            rayOrigin = raycastOrigins.bottomRight;
        }
        else
        {
            rayOrigin = raycastOrigins.bottomLeft;
        }

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin, -Vector2.up, Mathf.Infinity, collisionMask);

        if (hit)
        {
            float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);

            if (slopeAngle != 0 && slopeAngle <= maxSlopeAngle)
            {
                // Moving down the slope
                if (Mathf.Sign(hit.normal.x) == directionX)
                {
                    // Need to check we're close enough to the slope to move
                    if (hit.distance - skinWidth <= Mathf.Tan(slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(moveAmount.x))
                    {
                        float moveDistance = Mathf.Abs(moveAmount.x);
                        float descendmoveAmountY = Mathf.Sin(slopeAngle * Mathf.Deg2Rad) * moveDistance;
                        moveAmount.x = Mathf.Cos(slopeAngle * Mathf.Deg2Rad) * moveDistance * Mathf.Sign(moveAmount.x);
                        moveAmount.y -= descendmoveAmountY;

                        // Setting below to true allows for jumping
                        collisions.slopeAngle = slopeAngle;
                        collisions.descendingSlope = true;
                        collisions.below = true;
                    }
                }
            }
        }
    }
}
