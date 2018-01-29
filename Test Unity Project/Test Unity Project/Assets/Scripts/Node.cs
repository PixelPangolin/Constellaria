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

	void OnTriggerEnter2D (Collider2D other)
	{
		if (other.gameObject.CompareTag("Player")){
			NodeManager nodeManager = other.GetComponent(typeof(NodeManager)) as NodeManager;
			GameObject currentNode;
			currentNode = nodeManager.getCurrent();
			if(currentNode == adjNodeA){
				//Destroy(gameObject);
				connectA.SetActive(true);
			}

			if(currentNode == adjNodeB){
				//Destroy(gameObject);
				connectB.SetActive(true);
			}
			// // else if(nodeManager.getCurrent == adjNodeA){
				
			// // }
			nodeManager.setCurrent(gameObject);
			//NodeHandler handler = other.GetComponent(typeof(NodeHandler));
			//Destroy(gameObject);
		}

	}
}