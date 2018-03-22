using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour {

	public AudioSource audio;
	public AudioClip pullRope;
	public AudioClip tautRope;
    public Node currentNode;
    public LineRenderer line;
    public DistanceJoint2D hook;
    public bool connected = false;
	public Rope rope;
	public Rigidbody2D playerRigidBody;
	public float pullRopeDelay = 0.5f;
	private float pullRopeTimer = 0f;
    // Use this for initialization
    void Start () {
        hook = GetComponent<DistanceJoint2D>();
        hook.enabled = false;
    }

	void Update(){
		if (Input.GetButtonDown("Up")||Input.GetButtonDown("Down")){
			GameObject.Find("GameController").GetComponent<AudioController>().playPullRope();
			pullRopeTimer = Time.time+pullRopeDelay;
			print("pressed up or down!");


		}
		if (Input.GetButtonDown ("Shift")&&connected) {
			hook.distance = Vector3.Distance(transform.position,getCurrent().GetGameObject().transform.position);
			hook.enabled = true;
		}
		if (Input.GetButtonUp ("Shift")&&connected) {
			hook.distance = Vector3.Distance(transform.position,getCurrent().GetGameObject().transform.position);
			hook.enabled=false;
		}
	}

	// Update is called once per physics update
	void FixedUpdate () {

		if (Input.GetButton ("Shift")&&connected) {
			rope.nodes[0] = transform.position;
			Rope.ResetRope (rope, false);
		}

		if (Input.GetKey("up")||Input.GetKey("w"))
        {


				//rope.transform.GetChild (0).GetComponent<DistanceJoint2D> ().connectedBody = playerRigidBody;
				//Rope.UpdateEndsJoints(rope);
				//SOUND: When you pull back on the rope
				if (pullRopeTimer < Time.time) {
					print (pullRopeTimer);
					GameObject.Find("GameController").GetComponent<AudioController>().playPullRope();
					pullRopeTimer = (Time.time + pullRopeDelay);

				}


            hook.distance = hook.distance - 0.05f;
        }

		if (Input.GetKey("down")||Input.GetKey("s"))
		{			
			if (pullRopeTimer < Time.time) {
			//	audio.PlayOneShot (pullRope, PlayerPrefsManager.GetMasterVolume () * PlayerPrefsManager.GetSoundEffectVolume ());
				pullRopeTimer = (Time.time + pullRopeDelay);
			}
            hook.distance = hook.distance + 0.05f;
        }
			
        if (connected)
        {
            line.SetPosition(0, transform.position);
			GameObject node = getCurrent().GetGameObject();
			line.SetPosition(1, node.transform.position);
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

			/*
			if (Input.GetKey("left")||Input.GetKey("a")||Input.GetKey("right")||Input.GetKey("d")) //
			{
				hook.distance = Vector3.Distance(transform.position,node.transform.position) + 0.5f;
			}
			*/
			        }
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
		hook.connectedBody = nodeA.GetGameObject().GetComponent<Rigidbody2D>();

        //line.SetVertexCount(2); //may need this later for handling corners

        //no such thing as a 'get list/count of vertexes', so we will need to keep a list

        line.SetPosition(0, transform.position);
		line.SetPosition(1, nodeA.GetGameObject().transform.position);
		line.enabled = true;
        //line.SetWidth(0.1f, 0.1f);
        connected = true;
		rope.nodes[0] = transform.position;
		rope.nodes[1] = nodeA.GetGameObject().transform.position;
		//rope.firstSegmenthook = playerRigidBody;
		rope.secondSegmenthook = nodeA.GetGameObject().GetComponent<Rigidbody2D>();
		Rope.DestroyChildren (rope, false);
		Rope.ResetRope (rope, false);}
		//rope.LastSegmentConnectionAnchor.x = node.transform.position.x;
		//rope.LastSegmentConnectionAnchor.y = node.transform.position.y;
        // If we ever want to set the distance of the grappling hook
        //hook.distance = Vector2.Distance(transform.position,node.transform.position);
    
}
