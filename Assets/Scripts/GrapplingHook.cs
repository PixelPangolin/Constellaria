using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrapplingHook : MonoBehaviour {

    public Node currentNode;
    public LineRenderer line;
    public DistanceJoint2D hook;
    public bool connected = false;
    
    // Use this for initialization
    void Start () {
        hook = GetComponent<DistanceJoint2D>();
        hook.enabled = false;
    }
	
	// Update is called once per frame
	void Update () {

        if (Input.GetKey("up"))
        {
            hook.distance = hook.distance - 0.5f;
        }

        if (Input.GetKey("down"))
        {
            hook.distance = hook.distance + 0.5f;
        }

        if (connected)
        {
            line.SetPosition(0, transform.position);
			line.SetPosition(1, currentNode.GetGameObject().transform.position);
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
        hook.enabled = true;
		hook.connectedAnchor = nodeA.GetGameObject().transform.position;
		hook.connectedBody = nodeA.GetGameObject().GetComponent<Rigidbody2D>();

        //line.SetVertexCount(2); //may need this later for handling corners

        //no such thing as a 'get list/count of vertexes', so we will need to keep a list
        line.SetPosition(0, transform.position);
		line.SetPosition(1, nodeA.GetGameObject().transform.position);
        //line.SetWidth(0.1f, 0.1f);
        connected = true;

        // If we ever want to set the distance of the grappling hook
        //hook.distance = Vector2.Distance(transform.position,node.transform.position);
    }
}
