using UnityEngine;
using System.Collections;

public class Node : MonoBehaviour {


	public GameObject adjNodeA;
	public GameObject adjNodeB;
	public GameObject connectA;
	public GameObject connectB;
	// Use this for initialization
	void Start () {

		

	}

	// Update is called once per frame
	void Update () {

	}

	// If the player collids with a node
	// 1. Check what was the last node they collided with
	// 2. Activate the apporiate connecting line between the two nodes 
	// 3. Set current Node of the nodeManager to be this node

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag("Player")){
			GrapplingHook grapplingHook = other.GetComponent(typeof(GrapplingHook)) as GrapplingHook;
			GameObject currentNode;
			currentNode = grapplingHook.getCurrent();
			if(currentNode == adjNodeA){
				connectA.SetActive(true);
			}

			if(currentNode == adjNodeB){
				connectB.SetActive(true);
			}
				
			grapplingHook.setCurrent(gameObject);
			//NodeHandler handler = other.GetComponent(typeof(NodeHandler));
			//Destroy(gameObject);
		}

	}
}