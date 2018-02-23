using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public Node visitedNode;
    public float speed = 5.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        float translationX = Input.GetAxis("Vertical") * speed;
        float translationY = Input.GetAxis("Horizontal") * speed;
        translationX *= Time.deltaTime;
        translationY *= Time.deltaTime;
        transform.Translate(translationY, translationX, 0);
    }

    public Node getVisitedNode()
    {
        return this.visitedNode;
    }

    public void setVisitedNode(Node n)
    {
        this.visitedNode = n;
    }

}
