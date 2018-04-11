using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class GrapplingHook : MonoBehaviour {

    private Controller2D controller;
    private AudioController audioController;

	//public AudioSource audio;
	//public AudioClip pullRope;
	//public AudioClip tautRope;
    public Node currentNode;
    public LineRenderer line;
    public DistanceJoint2D hook;
    public bool connected = false;
	public bool isSwinging = false;
    public bool hanging = false;
	public Rope rope;
	public Rigidbody2D playerRigidBody;
	public Rigidbody2D ropeRigidBody;
	public float ropeDistance;

	public float pullRopeDelay = 1.3f;
	private float pullRopeTimer = 0f;
	public List<Vector2> ropePositions = new List<Vector2>();
	private bool distanceSet;
	public Dictionary<Vector2, int> wrapPointsLookup = new Dictionary<Vector2, int>();
	public LayerMask ropeLayerMask;
	private Vector2 playerPosition;



    // Use this for initialization
    void Start () {
        controller = GetComponent<Controller2D>();
        audioController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>();

        hook = GetComponent<DistanceJoint2D>();
        hook.enabled = false;
		playerPosition = transform.position;
    }

	void Update(){
		
		if (Input.GetButtonDown ("Shift")&&connected) {
			isSwinging = true;
            hanging = true;
            hook.distance = Vector3.Distance(transform.position,ropePositions.Last());
			ropeDistance = hook.distance;
			hook.enabled = true;
		}
		if (Input.GetButtonUp ("Shift")&&connected) {
			isSwinging = false;
            hanging = false;
			pullRopeTimer = 0.0f;
            //hook.distance = Vector3.Distance(transform.position,ropePositions.Last());
            hook.enabled=false;
		}

		playerPosition = transform.position;

		if (connected)
		{
			// If the ropePositions list has any positions stored, then...
			if (ropePositions.Count > 0)
			{
				// Fire a raycast out from the player's position, in the direction of the player looking at the last rope position in the list
				// the pivot point where the grappling hook is hooked into the rock — with a raycast distance set to the distance between the player and rope pivot position.
				var lastRopePoint = ropePositions.Last();
				var playerToCurrentNextHit = Physics2D.Raycast(playerPosition, (lastRopePoint - playerPosition).normalized, Vector2.Distance(playerPosition, lastRopePoint) - 0.1f, ropeLayerMask);

				// If the raycast hits something, then that hit object's collider is safe cast to a PolygonCollider2D. 
				// As long as it's a real PolygonCollider2D, then the closest vertex position on that collider is returned as a Vector2
				if (playerToCurrentNextHit)
				{
					
					var colliderWithVertices = playerToCurrentNextHit.collider as PolygonCollider2D;
					//print (colliderWithVertices);
					if (colliderWithVertices != null)
					{
						var closestPointToHit = GetClosestColliderPointFromRaycastHit(playerToCurrentNextHit, colliderWithVertices);

						// The wrapPointsLookup is checked to make sure the same position is not being wrapped again. If it is, then it'll reset the rope and cut it, dropping the player.
						//if (wrapPointsLookup.ContainsKey(closestPointToHit))
						//{
						//	ResetLine();
						//	return;
						//}

						// The ropePositions list is now updated, adding the position the rope should wrap around, 
						// and the wrapPointsLookup dictionary is also updated. Lastly the distanceSet flag is disabled, 
						// so that UpdateRopePositions() method can re-configure the rope's distances to take into account the new rope length and segments.
						ropePositions.Add(closestPointToHit);
						wrapPointsLookup.Add(closestPointToHit, 0);
						distanceSet = false;
					}
				}
			}
		}
		UpdateRopePositions();
		//HandleRopeLength();
		HandleRopeUnwrap();
	}

	// Update is called once per physics update
	void FixedUpdate () {

		if (Input.GetButton ("Shift")&&connected) {
			//rope.nodes[0] = transform.position;
			//Rope.ResetRope (rope, false);
		}

        if (hanging)
        {
            if (Input.GetKey("up") && !controller.collisions.below || Input.GetKey("w") && !controller.collisions.below)
            {
                if (pullRopeTimer < Time.time)
                {
                    //print(pullRopeTimer);

                    // Sound for going up the rope
                    audioController.playPullRope();
                    pullRopeTimer = (Time.time + pullRopeDelay);

                }

                hook.distance = hook.distance - 0.05f;
				ropeDistance = hook.distance;
            }
        }

		if (Input.GetKey("down")||Input.GetKey("s"))
		{			
			if (pullRopeTimer < Time.time) {
				pullRopeTimer = (Time.time + pullRopeDelay);
			}
            hook.distance = hook.distance + 0.05f;
			ropeDistance = hook.distance;
        }
		/*
        if (connected)
        {
          //  line.SetPosition(0, transform.position);
			GameObject node = getCurrent().GetGameObject();
			//line.SetPosition(1, node.transform.position);
			//rope.nodes[0] = transform.position;
			//Rope.ResetRope (rope, false);
			if (rope.transform.childCount*rope.linkSpriteLength < rope.lengthenBound*Vector3.Distance(transform.position,node.transform.position))
			{
				Rope.Lengthen(rope);
				//Rope.ResetRope(rope, false);
				//print ("lengthen");
			}
			if (rope.transform.childCount*rope.linkSpriteLength > rope.shortenBound*Vector3.Distance(transform.position,node.transform.position))
				
			{
				Rope.Shorten(rope, node);
				//Rope.ResetRope(rope, false);
				//print ("shorten");
			}
			if (Mathf.Abs(Vector3.Distance(transform.position,node.transform.position) - hook.distance)< 0.5f) {
				//SOUND: When the Rope becomes taut
			//	audio.PlayOneShot(tautRope ,PlayerPrefsManager.GetMasterVolume()*PlayerPrefsManager.GetSoundEffectVolume());//TODO get volume from something

			}
			//print ((rope.transform.childCount*rope.transform.GetChild(0).GetComponent<SpriteRenderer>().bounds.size.y).ToString() + " " + (Vector3.Distance(transform.position,node.transform.position)).ToString());
			        }
		*/
    }


    public void setCurrent(Node nodeA)
    {
        currentNode = nodeA;
        setGrapplingHook(nodeA);
    }
	public Node getCurrent(){
		return currentNode;
	}

    public void setGrapplingHook(Node nodeA)
    {
        //hook.enabled = true;
		//hook.connectedAnchor = nodeA.GetGameObject().transform.position;
		Vector2 nodePosition = nodeA.transform.position;
		ropeRigidBody.transform.position = nodePosition;

		//hook.connectedBody = ropeRigidBody;//nodeA.GetGameObject().GetComponent<Rigidbody2D>();

		//may need this later for handling corners 
		connected = true;
		line.enabled = true;
		ResetLine ();
		//ropePositions.Add(nodePosition);
		//line.positionCount = 2;

        //no such thing as a 'get list/count of vertexes', so we will need to keep a list
	
        //line.SetPosition(0, transform.position);
		//line.SetPosition(1, nodeA.GetGameObject().transform.position);
	
        //line.SetWidth(0.1f, 0.1f);
        /*
	 	rope.nodes[0] = transform.position;
		rope.nodes[1] = nodeA.GetGameObject().transform.position;
		//rope.firstSegmenthook = playerRigidBody;
		rope.secondSegmenthook = nodeA.GetGameObject().GetComponent<Rigidbody2D>();
		Rope.DestroyChildren (rope, false);
		Rope.ResetRope (rope, false);}
		//rope.LastSegmentConnectionAnchor.x = node.transform.position.x;
		//rope.LastSegmentConnectionAnchor.y = node.transform.position.y;
		*/
	}

private void UpdateRopePositions ()
	{
		// 2
		line.positionCount = ropePositions.Count + 1;
		//playerMovement.ropeHook = ropePositions.Last();
		// 3
		for (var i = line.positionCount - 1; i >= 0; i--) {
			if (i != line.positionCount - 1) { // if not the Last point of line renderer
				line.SetPosition (i, new Vector3(ropePositions[i].x, ropePositions[i].y, 1f));

				// 4
				if (i == ropePositions.Count - 1 || ropePositions.Count == 1) {
					var ropePosition = ropePositions [ropePositions.Count - 1];
					if (ropePositions.Count == 1) {
						ropeRigidBody.transform.position = ropePosition;
						if (!distanceSet) {
							hook.distance = Vector2.Distance (transform.position, ropePosition);
							ropeDistance = hook.distance;
							distanceSet = true;
						}
					} else {
						ropeRigidBody.transform.position = ropePosition;
						if (!distanceSet) {
							hook.distance = Vector2.Distance (transform.position, ropePosition);
							ropeDistance = hook.distance;
							distanceSet = true;
						}
					}
				}
			// 5
			else if (i - 1 == ropePositions.IndexOf (ropePositions.Last ())) {
					var ropePosition = ropePositions.Last ();
					ropeRigidBody.transform.position = ropePosition;
					if (!distanceSet) {
						hook.distance = Vector2.Distance (transform.position, ropePosition);
						ropeDistance = hook.distance;
						distanceSet = true;
					}
				}
			} else {
				// 6
				line.SetPosition (i, new Vector3(transform.position.x, transform.position.y, 1f));
			}
		}
	}
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
	public void ResetLine()
	{
		//hook.enabled = false;
		//connected = false;
		//playerMovement.isSwinging = false;
		line.positionCount = 2;
		line.SetPosition(0, new Vector3(currentNode.transform.position.x, currentNode.transform.position.y, 1f));
		line.SetPosition(1, new Vector3(transform.position.x, transform.position.y, 1f));
		ropePositions.Clear();
		//ropePositions.Add(line.GetPosition(0));
		ropePositions.Add(new Vector3(currentNode.transform.position.x, currentNode.transform.position.y, 1f));
		//ropeHingeAnchorSprite.enabled = false;
		wrapPointsLookup.Clear();
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
		ropeRigidBody.transform.position = newAnchorPosition;
		distanceSet = false;

		// Set new rope distance joint distance for anchor position if not yet set.
		if (distanceSet)
		{
			return;
		}
		hook.distance = Vector2.Distance(transform.position, newAnchorPosition);
		ropeDistance = hook.distance;
		distanceSet = true;
	}
}
