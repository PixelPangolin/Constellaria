using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

	public GameObject currentNode;
	public DistanceJoint2D hook;
	public bool connected = false;
	public LineRenderer line;

	// Use this for initialization
	void Start () {
		hook = GetComponent<DistanceJoint2D>();
		hook.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKey ("up")) {
			hook.distance = hook.distance - 0.5f;

		}
			

		if (Input.GetKey ("down")) {
			hook.distance = hook.distance + 0.5f;
			print("down arrow key is held down");
		}
			
		if (connected) 
		{
			line.SetPosition(0, transform.position);
			GameObject node = getCurrent();
			line.SetPosition(1, node.transform.position);
		}

	}

	public void setCurrent(GameObject node){
		this.currentNode = node;
		setGrapplingHook (node);
	}

	public GameObject getCurrent(){
		return this.currentNode;
	}

	public void setGrapplingHook(GameObject node){
		hook.enabled = true;
		//hook.anchor = node.transform.position;
		hook.connectedBody = node.GetComponent<Rigidbody2D>();

		//line.SetVertexCount(2); //may need this later for handling corners
		//no such thing as a 'get list/count of vertexes', so we will need to keep a list
		line.SetPosition(0, transform.position);
		line.SetPosition(1, node.transform.position);
		//line.SetWidth(0.1f, 0.1f);
		connected = true;

		// If we ever want to set the distance of the grappling hook
		hook.distance = Vector2.Distance(transform.position,node.transform.position);
	}
}
