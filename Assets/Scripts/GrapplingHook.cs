﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour {

    public GameObject currentNode;
    public LineRenderer line;
    public DistanceJoint2D hook;
    public bool connected = false;
	public Rope rope;

    // Use this for initialization
    void Start () {
        hook = GetComponent<DistanceJoint2D>();
        hook.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey("up")||Input.GetKey("w"))
        {
            hook.distance = hook.distance - 0.5f;
        }

		if (Input.GetKey("down")||Input.GetKey("s"))
        {
            hook.distance = hook.distance + 0.5f;
        }
			
        if (connected)
        {
            line.SetPosition(0, transform.position);
			GameObject node = getCurrent();
			line.SetPosition(1, node.transform.position);
			if (Input.GetKey("left")||Input.GetKey("a")||Input.GetKey("right")||Input.GetKey("d"))
			{
				hook.distance = Vector3.Distance(transform.position,node.transform.position) + 0.5f;
			}
        }
    }

    public GameObject getCurrent()
    {
        return currentNode;
    }

    public void setCurrent(GameObject node)
    {
        currentNode = node;
        setGrapplingHook(node);
    }

    public void setGrapplingHook(GameObject node)
	{
        hook.enabled = true;
		hook.connectedAnchor = node.transform.position;
        hook.connectedBody = node.GetComponent<Rigidbody2D>();

        //line.SetVertexCount(2); //may need this later for handling corners

        //no such thing as a 'get list/count of vertexes', so we will need to keep a list
        line.SetPosition(0, transform.position);
		line.SetPosition(1, node.transform.position);
        //line.SetWidth(0.1f, 0.1f);
        connected = true;

		rope.nodes[1] = node.transform.position;
	
		//Rope.DestroyChildren (rope, false);
		Rope.UpdateRope (rope, false);
		//rope.LastSegmentConnectionAnchor.x = node.transform.position.x;
		//rope.LastSegmentConnectionAnchor.y = node.transform.position.y;
        // If we ever want to set the distance of the grappling hook
        //hook.distance = Vector2.Distance(transform.position,node.transform.position);
    }
}