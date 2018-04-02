using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CapsuleCollider2D))]
public class RaycastController : MonoBehaviour {

    // The layers we want to collide with
    public LayerMask collisionMask;

    // The amount we inset our raycasts by - we do not want to change this
    public const float skinWidth = 0.010f;
    const float distanceBetweenRays = 0.25f;

    // The number of raycasts we have and the spacing
    [HideInInspector] public int horizontalRayCount;
    [HideInInspector] public int verticalRayCount;
    [HideInInspector] public float horizontalRaySpacing;
    [HideInInspector] public float verticalRaySpacing;

    [HideInInspector] public CapsuleCollider2D collider;
    [HideInInspector] public RaycastOrigins raycastOrigins;

    [HideInInspector] public AudioController audioController;
    [HideInInspector] public GrapplingHook grapple;

    // A struct that stores the places the raycasts come from
    public struct RaycastOrigins
    {
        public Vector2 topLeft;
        public Vector2 topRight;
        public Vector2 bottomLeft;
        public Vector2 bottomRight;
    }

    // Use this for initialization
    public virtual void Awake()
    {
        collider = GetComponent<CapsuleCollider2D>();
        audioController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>();
        grapple = GetComponent<GrapplingHook>();
    }

    public virtual void Start()
    {
        CalculateRaySpacing();
    }

    public void UpdateRaycastOrigins()
    {
        // Finds the bounds of the box collider and shrinks it slightly by the skinWidth
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
        raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
        raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
        raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
    }

    public void CalculateRaySpacing()
    {
        // Finds the bounds of the box collider and shrinks it slightly by the skinWidth
        Bounds bounds = collider.bounds;
        bounds.Expand(skinWidth * -2);

        float boundsWidth = bounds.size.x;
        float boundsHeight = bounds.size.y;

        // Calculate number of rays based on distance between rays
        horizontalRayCount = Mathf.RoundToInt(boundsHeight / distanceBetweenRays);
        verticalRayCount = Mathf.RoundToInt(boundsWidth / distanceBetweenRays);

        // Calculate ray spacing
        horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
        verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
    }

}
