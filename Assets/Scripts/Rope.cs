using UnityEngine;
using System.Collections.Generic;
public enum SegmentSelectionMode
{
    RoundRobin,
    Random
}
public enum LineOverflowMode
{
    Round,
    Shrink,
    Extend
}
public class Rope : MonoBehaviour {
    public SpriteRenderer[] SegmentsPrefabs;
    public SegmentSelectionMode SegmentsMode;
    public LineOverflowMode OverflowMode; 
	[HideInInspector]
	public Rigidbody2D firstSegmenthook;
	[HideInInspector]
	public Rigidbody2D secondSegmenthook;
    [HideInInspector]
    public bool useBendLimit = true;
    [HideInInspector]
    public int bendLimit = 45;
    [HideInInspector]
    public bool HangFirstSegment = false;
    [HideInInspector]
    public Vector2 FirstSegmentConnectionAnchor;
    [HideInInspector]
    public Vector2 LastSegmentConnectionAnchor;
    [HideInInspector]
    public bool HangLastSegment = false;

#if UNITY_5
    [HideInInspector]
    public bool BreakableJoints=false;
    [HideInInspector]
    public float BreakForce = 100;
#endif
    [Range(-0.5f,0.5f)]
    public float overlapFactor;
    public List<Vector3> nodes = new List<Vector3>(new Vector3[] {new Vector3(-3,0,0),new Vector3(3,0,0) });
    public bool WithPhysics=true;
    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public static void DestroyChildren(Rope rope, bool edit)
	{
		if (edit) {
			while (rope.transform.childCount > 0) {
				DestroyImmediate (rope.transform.GetChild (0).gameObject);
			}
		} 
		else {
			for (int i = 0; i < rope.transform.childCount; i++) {
				Destroy (rope.transform.GetChild (i).gameObject);
				print (rope.transform.childCount);
			}
		}

	}
	public static void AddJoint(Rope rope, float dtheta, float segmentHeight, Rigidbody2D previousSegment, GameObject segment)
	{
		HingeJoint2D joint = segment.AddComponent<HingeJoint2D>();
		joint.connectedBody = previousSegment;
		joint.anchor = new Vector2(0, -segmentHeight / 2);
		joint.connectedAnchor = new Vector2(0, segmentHeight / 2);
		if (rope.useBendLimit)
		{
			joint.useLimits = true;
			joint.limits = new JointAngleLimits2D()
			{
				min = dtheta - rope.bendLimit,
				max = dtheta + rope.bendLimit
			};
		}

		#if UNITY_5
		if (rope.BreakableJoints)
		joint.breakForce = rope.BreakForce;
		#endif
	}
	public static Rigidbody2D GetConnectedObject(Vector2 position, Rigidbody2D originalObj) //TODO load this from script menu, and make it setable
	{
		Rigidbody2D[] sceneRigidbodies = GameObject.FindObjectsOfType<Rigidbody2D>();
		for (int i = 0; i < sceneRigidbodies.Length; i++) {
			if (originalObj != sceneRigidbodies [i] && sceneRigidbodies [i].GetComponent<SpriteRenderer> ()!= null) {
				if (sceneRigidbodies [i].GetComponent<SpriteRenderer> ().bounds.Contains (position)){
					return sceneRigidbodies [i];
				}
			}
		}
		return null;
	}
	public static void UpdateEndsJoints(Rope rope)
	{
		Transform firstSegment = rope.transform.GetChild(0);
		if (rope.WithPhysics&& 
			rope.HangFirstSegment&&
			rope.transform.childCount>0)
		{

			HingeJoint2D joint = firstSegment.gameObject.GetComponent<HingeJoint2D>();
			if(!joint)
				joint = firstSegment.gameObject.AddComponent<HingeJoint2D>();
			Vector2 hingePositionInWorldSpace = rope.transform.TransformPoint(rope.FirstSegmentConnectionAnchor);
			joint.connectedAnchor = hingePositionInWorldSpace;
			joint.anchor = firstSegment.transform.InverseTransformPoint(hingePositionInWorldSpace);
			//joint.connectedBody = GetConnectedObject(hingePositionInWorldSpace, firstSegment.GetComponent<Rigidbody2D>());
			joint.connectedBody = GetConnectedObject(hingePositionInWorldSpace, firstSegment.GetComponent<Rigidbody2D>());
			if(joint.connectedBody)
			{
				joint.connectedAnchor = joint.connectedBody.transform.InverseTransformPoint(hingePositionInWorldSpace);
			}
		}
		else
		{
			HingeJoint2D joint = firstSegment.gameObject.GetComponent<HingeJoint2D>();
			if (joint) DestroyImmediate(joint);
		}
		Transform lastSegment = rope.transform.GetChild(rope.transform.childCount - 1);
		if (rope.WithPhysics && rope.HangLastSegment)
		{
			HingeJoint2D[] joints = lastSegment.gameObject.GetComponents<HingeJoint2D>();
			HingeJoint2D joint = null;
			if (joints.Length > 1)
				joint = joints[1];
			else
				joint = lastSegment.gameObject.AddComponent<HingeJoint2D>();
			Vector2 hingePositionInWorldSpace = rope.transform.TransformPoint(rope.LastSegmentConnectionAnchor);
			joint.connectedAnchor = hingePositionInWorldSpace;
			joint.anchor = lastSegment.transform.InverseTransformPoint(hingePositionInWorldSpace) ;
			joint.connectedBody = GetConnectedObject(hingePositionInWorldSpace, lastSegment.GetComponent<Rigidbody2D>());
			if (joint.connectedBody)
			{
				joint.connectedAnchor = joint.connectedBody.transform.InverseTransformPoint(hingePositionInWorldSpace);
			}
		}
		else
		{
			HingeJoint2D[] joints = lastSegment.gameObject.GetComponents<HingeJoint2D>();
			if (joints.Length > 1)
				for (int i = 1; i < joints.Length; i++)
					DestroyImmediate(joints[i]);
		}
	}
	public static void UpdateRope(Rope rope, bool edit)
	{
		DestroyChildren(rope, edit);
		if (rope.SegmentsPrefabs==null||rope.SegmentsPrefabs.Length == 0)
		{
			Debug.LogWarning("Rope Segments Prefabs is Empty");
			return;
		}
		float segmentHeight = rope.SegmentsPrefabs[0].bounds.size.y * (1 + rope.overlapFactor);
		List<Vector3> nodes = rope.nodes;
		int currentSegPrefIndex = 0;
		Rigidbody2D previousSegment = null;
		float previousTheta = 0;
		int currentSegment = 0;
		for (int i = 0; i < nodes.Count - 1; i++)
		{
			//construct line between nodes[i] and nodes[i+1]
			float theta = Mathf.Atan2(nodes[i + 1].y - nodes[i].y, nodes[i + 1].x - nodes[i].x);
			float dx = segmentHeight * Mathf.Cos(theta);
			float dy = segmentHeight * Mathf.Sin(theta);
			float startX = nodes[i].x + dx / 2;
			float startY = nodes[i].y + dy / 2;
			float lineLength = Vector2.Distance(nodes[i + 1], nodes[i]);
			int segmentCount = 0;
			switch(rope.OverflowMode)
			{
			case LineOverflowMode.Round:
				segmentCount = Mathf.RoundToInt(lineLength / segmentHeight);
				break;
			case LineOverflowMode.Shrink:
				segmentCount = (int)(lineLength / segmentHeight);
				break;
			case LineOverflowMode.Extend:
				segmentCount = Mathf.CeilToInt(lineLength / segmentHeight);
				break;
			}
			for (int j = 0; j < segmentCount; j++)
			{
				if (rope.SegmentsMode == SegmentSelectionMode.RoundRobin)
				{
					currentSegPrefIndex++;
					currentSegPrefIndex %= rope.SegmentsPrefabs.Length;
				}
				else if (rope.SegmentsMode == SegmentSelectionMode.Random)
				{
					currentSegPrefIndex = Random.Range(0, rope.SegmentsPrefabs.Length);
				}
				GameObject segment = (Instantiate(rope.SegmentsPrefabs[currentSegPrefIndex]) as SpriteRenderer).gameObject;
				segment.name = "Segment_" + currentSegment;
				segment.transform.parent = rope.transform;
				segment.transform.localPosition = new Vector3(startX + dx * j, startY + dy * j);
				segment.transform.localRotation = Quaternion.Euler(0, 0, theta * Mathf.Rad2Deg - 90);
				if (rope.WithPhysics)
				{
					Rigidbody2D segRigidbody = segment.GetComponent<Rigidbody2D>();
					if (segRigidbody == null)
						segRigidbody = segment.AddComponent<Rigidbody2D>();
					//if not the first segment, make a joint
					if (currentSegment != 0)
					{
						float dtheta = 0;
						if (j == 0)
						{
							//first segment in the line
							dtheta = (theta - previousTheta) * Mathf.Rad2Deg;
							if (dtheta > 180) dtheta -= 360;
							else if (dtheta < -180) dtheta += 360;
						}
						//add Hinge
						AddJoint(rope, dtheta, segmentHeight, previousSegment, segment);
					}
					previousSegment = segRigidbody;
				}
				currentSegment++;
			}
			previousTheta = theta;
		}
		UpdateEndsJoints(rope);
	}
}
                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                        