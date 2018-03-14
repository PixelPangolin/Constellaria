// https://www.raywenderlich.com/171250/make-2d-grappling-hook-game-unity-part-1
// https://www.raywenderlich.com/173760/make-2d-grappling-hook-game-unity-part-2
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
public class RopeSystem : MonoBehaviour {

	// Use this for initialization
	// 1
public GameObject ropeHingeAnchor;
public DistanceJoint2D ropeJoint;
public Transform crosshair;
public SpriteRenderer crosshairSprite;
public PlayerMovement playerMovement;
private bool ropeAttached;
private Vector2 playerPosition;
private Rigidbody2D ropeHingeAnchorRb;
private SpriteRenderer ropeHingeAnchorSprite;
public LineRenderer ropeRenderer;
public LayerMask ropeLayerMask;
private float ropeMaxCastDistance = 20f;
private List<Vector2> ropePositions = new List<Vector2>();
	private bool distanceSet;
	private Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();
	public float climbSpeed = 3f;
	private bool isColliding;
void Awake()
{
    // 2
    ropeJoint.enabled = false;
    playerPosition = transform.position;
    ropeHingeAnchorRb = ropeHingeAnchor.GetComponent<Rigidbody2D>();
    ropeHingeAnchorSprite = ropeHingeAnchor.GetComponent<SpriteRenderer>();
}

void Update()
{
    // 3
    var worldMousePosition =
        Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0f));
    var facingDirection = worldMousePosition - transform.position;
    var aimAngle = Mathf.Atan2(facingDirection.y, facingDirection.x);
    if (aimAngle < 0f)
    {
        aimAngle = Mathf.PI * 2 + aimAngle;
    }

    // 4
    var aimDirection = Quaternion.Euler(0, 0, aimAngle * Mathf.Rad2Deg) * Vector2.right;
    // 5
    playerPosition = transform.position;

    // 6
    if (!ropeAttached)
    {
			SetCrosshairPosition (aimAngle);
    }
    else
    {
			crosshairSprite.enabled = false;
			// If the ropePositions list has any positions stored, then...
			if (ropePositions.Count > 0)
			{
				// Fire a raycast out from the player's position, in the direction of the player looking at the last rope position in the list — the pivot point where the grappling hook is hooked into the rock — with a raycast distance set to the distance between the player and rope pivot position.
				var lastRopePoint = ropePositions.Last();
				var playerToCurrentNextHit = Physics2D.Raycast(playerPosition, (lastRopePoint - playerPosition).normalized, Vector2.Distance(playerPosition, lastRopePoint) - 0.1f, ropeLayerMask);

				// If the raycast hits something, then that hit object's collider is safe cast to a PolygonCollider2D. As long as it's a real PolygonCollider2D, then the closest vertex position on that collider is returned as a Vector2, using that handy-dandy method you wrote earlier.
				if (playerToCurrentNextHit)
				{
					var colliderWithVertices = playerToCurrentNextHit.collider as PolygonCollider2D;
					if (colliderWithVertices != null)
					{
						var closestPointToHit = GetClosestColliderPointFromRaycastHit(playerToCurrentNextHit, colliderWithVertices);

						// The wrapPointsLookup is checked to make sure the same position is not being wrapped again. If it is, then it'll reset the rope and cut it, dropping the player.
						if (wrapPointsLookup.ContainsKey(closestPointToHit))
						{
							ResetRope();
							return;
						}

						// The ropePositions list is now updated, adding the position the rope should wrap around, and the wrapPointsLookup dictionary is also updated. Lastly the distanceSet flag is disabled, so that UpdateRopePositions() method can re-configure the rope's distances to take into account the new rope length and segments.
						ropePositions.Add(closestPointToHit);
						wrapPointsLookup.Add(closestPointToHit, 0);
						distanceSet = false;
					}
				}
			}
    }
		HandleInput (aimDirection);
		UpdateRopePositions();
		HandleRopeLength();
		HandleRopeUnwrap();
}
	private void SetCrosshairPosition(float aimAngle)
	{
		if (!crosshairSprite.enabled)
		{
			crosshairSprite.enabled = true;
		}

		var x = transform.position.x + 1f * Mathf.Cos(aimAngle);
		var y = transform.position.y + 1f * Mathf.Sin(aimAngle);

		var crossHairPosition = new Vector3(x, y, 0);
		crosshair.transform.position = crossHairPosition;
	}
	// 1
	private void HandleInput(Vector2 aimDirection)
	{
		if (Input.GetMouseButton(0))
		{
			// 2
			if (ropeAttached) return;
			ropeRenderer.enabled = true;

			var hit = Physics2D.Raycast(playerPosition, aimDirection, ropeMaxCastDistance, ropeLayerMask);

			// 3
			if (hit.collider != null)
			{
				ropeAttached = true;
				if (!ropePositions.Contains(hit.point))
				{
					// 4
					// Jump slightly to distance the player a little from the ground after grappling to something.
					transform.GetComponent<Rigidbody2D>().AddForce(new Vector2(0f, 2f), ForceMode2D.Impulse);
					ropePositions.Add(hit.point);
					ropeJoint.distance = Vector2.Distance(playerPosition, hit.point);
					ropeJoint.enabled = true;
					ropeHingeAnchorSprite.enabled = true;
				}
			}
			// 5
			else
			{
				ropeRenderer.enabled = false;
				ropeAttached = false;
				ropeJoint.enabled = false;
			}
		}

		if (Input.GetMouseButton(1))
		{
			ResetRope();
		}
	}

	// 6
	private void ResetRope()
	{
		ropeJoint.enabled = false;
		ropeAttached = false;
		playerMovement.isSwinging = false;
		ropeRenderer.positionCount = 2;
		ropeRenderer.SetPosition(0, transform.position);
		ropeRenderer.SetPosition(1, transform.position);
		ropePositions.Clear();
		ropeHingeAnchorSprite.enabled = false;
		wrapPointsLookup.Clear();
	}
	private void UpdateRopePositions()
	{
		// 1
		if (!ropeAttached) {
			playerMovement.isSwinging = false;
			return;
		} 
		else {
			playerMovement.isSwinging = true;
			playerMovement.ropeHook = ropePositions.Last();
		}

		// 2
		ropeRenderer.positionCount = ropePositions.Count + 1;

		// 3
		for (var i = ropeRenderer.positionCount - 1; i >= 0; i--)
		{
			if (i != ropeRenderer.positionCount - 1) // if not the Last point of line renderer
			{
				ropeRenderer.SetPosition(i, ropePositions[i]);

				// 4
				if (i == ropePositions.Count - 1 || ropePositions.Count == 1)
				{
					var ropePosition = ropePositions[ropePositions.Count - 1];
					if (ropePositions.Count == 1)
					{
						ropeHingeAnchorRb.transform.position = ropePosition;
						if (!distanceSet)
						{
							ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
							distanceSet = true;
						}
					}
					else
					{
						ropeHingeAnchorRb.transform.position = ropePosition;
						if (!distanceSet)
						{
							ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
							distanceSet = true;
						}
					}
				}
				// 5
				else if (i - 1 == ropePositions.IndexOf(ropePositions.Last()))
				{
					var ropePosition = ropePositions.Last();
					ropeHingeAnchorRb.transform.position = ropePosition;
					if (!distanceSet)
					{
						ropeJoint.distance = Vector2.Distance(transform.position, ropePosition);
						distanceSet = true;
					}
				}
			}
			else
			{
				// 6
				ropeRenderer.SetPosition(i, transform.position);
			}
		}
	}
	// 1
	private Vector2 GetClosestColliderPointFromRaycastHit(RaycastHit2D hit, PolygonCollider2D polyCollider)
	{
		// 2
		var distanceDictionary = polyCollider.points.ToDictionary<Vector2, float, Vector2>(
			position => Vector2.Distance(hit.point, polyCollider.transform.TransformPoint(position)), 
			position => polyCollider.transform.TransformPoint(position));

		// 3
		var orderedDictionary = distanceDictionary.OrderBy(e => e.Key);
		return orderedDictionary.Any() ? orderedDictionary.First().Value : Vector2.zero;
	}
	private void HandleRopeLength()
	{
		// 1
		if (Input.GetAxis("Vertical") >= 1f && ropeAttached && !isColliding)
		{
			ropeJoint.distance -= Time.deltaTime * climbSpeed;
		}
		else if (Input.GetAxis("Vertical") < 0f && ropeAttached)
		{
			ropeJoint.distance += Time.deltaTime * climbSpeed;
		}
	}
	void OnTriggerStay2D(Collider2D colliderStay)
	{
		isColliding = true;
	}

	private void OnTriggerExit2D(Collider2D colliderOnExit)
	{
		isColliding = false;
	}
	private void HandleRopeUnwrap()
	{
		if (ropePositions.Count <= 1)
		{
			return;
		}
		// Hinge = next point up from the player position
		// Anchor = next point up from the Hinge
		// Hinge Angle = Angle between anchor and hinge
		// Player Angle = Angle between anchor and player

		// 1
		var anchorIndex = ropePositions.Count - 2;
		// 2
		var hingeIndex = ropePositions.Count - 1;
		// 3
		var anchorPosition = ropePositions[anchorIndex];
		// 4
		var hingePosition = ropePositions[hingeIndex];
		// 5
		var hingeDir = hingePosition - anchorPosition;
		// 6
		var hingeAngle = Vector2.SignedAngle(anchorPosition, hingeDir);
		if (hingeAngle < 0)
			hingeAngle = hingeAngle + 360f;
		// 7
		var playerDir = playerPosition - anchorPosition;
		// 8
		var playerAngle = Vector2.SignedAngle(anchorPosition, playerDir);
		if (playerAngle < 0)
			playerAngle = playerAngle + 360f;
		if (!wrapPointsLookup.ContainsKey(hingePosition))
		{
			Debug.LogError("We were not tracking hingePosition (" + hingePosition + ") in the look up dictionary.");
			return;
		}

		if (playerAngle < hingeAngle)
		{
			// 1
			if (wrapPointsLookup[hingePosition] == 1)
			{
				UnwrapRopePosition(anchorIndex, hingeIndex);
				return;
			}

			// 2
			wrapPointsLookup[hingePosition] = -1;
		}
		else
		{
			// 3
			if (wrapPointsLookup[hingePosition] == -1)
			{
				UnwrapRopePosition(anchorIndex, hingeIndex);
				return;
			}

			// 4
			wrapPointsLookup[hingePosition] = 1;
		}
	}
	private void UnwrapRopePosition(int anchorIndex, int hingeIndex)
	{
		// 1
		var newAnchorPosition = ropePositions[anchorIndex];
		wrapPointsLookup.Remove(ropePositions[hingeIndex]);
		ropePositions.RemoveAt(hingeIndex);

		// 2
		ropeHingeAnchorRb.transform.position = newAnchorPosition;
		distanceSet = false;

		// Set new rope distance joint distance for anchor position if not yet set.
		if (distanceSet)
		{
			return;
		}
		ropeJoint.distance = Vector2.Distance(transform.position, newAnchorPosition);
		distanceSet = true;
	}
}