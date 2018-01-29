using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeManager : MonoBehaviour {

	public GameObject currentNode;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setCurrent(GameObject node){
		this.currentNode = node;
	}

	public GameObject getCurrent(){
		return this.currentNode;
	}
}
