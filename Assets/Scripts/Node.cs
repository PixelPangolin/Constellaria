using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Node : MonoBehaviour
{
	public bool on = false;
	private Behaviour c;
	public GrapplingHook pc;


	// Use this for initialization
	void Start()
	{
		c = (Behaviour)gameObject.GetComponent("Halo");
	}

	// Update is called once per frame
	void Update()
	{
		c.enabled = on;
	}

	void OnTriggerEnter(Collider other)
	{
		print ("Collision");
		pc = other.gameObject.GetComponent<GrapplingHook>();
		Node playerNode = pc.getCurrent ();
		if ((playerNode != null) && (!playerNode.Equals(this)))
		{
			GameObject.Find("GameController").GetComponent<ConnectionsManager>().SwitchConnectionStateBetweenNodes(playerNode, this);
			playerNode.on = false;
			this.on = true;
			pc.setCurrent(this);
		} else if (playerNode == null)
		{
			pc.setCurrent(this);
			this.on = true;
		}
	}

	public GameObject GetGameObject(){
		return gameObject;
	}
}

