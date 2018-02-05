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
			NodeManager nodeManager = other.GetComponent(typeof(NodeManager)) as NodeManager;
			GameObject currentNode;
			currentNode = nodeManager.getCurrent();
			if(currentNode == adjNodeA){
				connectA.SetActive(true);
			}

			if(currentNode == adjNodeB){
				connectB.SetActive(true);
			}
				
			nodeManager.setCurrent(gameObject);
			//NodeHandler handler = other.GetComponent(typeof(NodeHandler));
			//Destroy(gameObject);
		}

	}
}