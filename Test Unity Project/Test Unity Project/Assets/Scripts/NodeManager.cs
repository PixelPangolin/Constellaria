using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

	public GameObject currentNode;
	public DistanceJoint2D hook;

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

		// If we ever want to set the distance of the grappling hook
		hook.distance = Vector2.Distance(transform.position,node.transform.position);
	}
}
