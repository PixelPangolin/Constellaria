// Created with help from  https://forum.unity.com/threads/forcing-a-character-to-fall-through-a-platform.222020/#post-1480670
using UnityEngine;
using System.Collections;

public class TwoWayLine : MonoBehaviour {

	//TODO: Make this only disable the line you are standing on, for as long as you are touching it.


	GameObject groundCheck;
	private bool grounded = false;
	//private BoxCollider2D boxCollider;

	//public Transform groundCheck;
	void Awake () 
	{
		groundCheck = GameObject.Find("groundCheck");
		//boxCollider = GameObject.Find("hero").GetComponent<BoxCollider2D>();
	}

	// FixedUpdate is called once per physics update
	void FixedUpdate () 
	{
		grounded = (Physics2D.Linecast(transform.position, groundCheck.transform.position, 1 << LayerMask.NameToLayer("Lines")) || Physics2D.Linecast(transform.position, groundCheck.transform.position, 1 << LayerMask.NameToLayer("Ground"))) ;

		if (grounded) 
		{
			this.GetComponent<BoxCollider2D> ().enabled = true;
		}

		if (Input.GetKey("down")||Input.GetKey("s")) 
		{
			StartCoroutine("Fall");
		}
			
	}

	// Makes the character fall by disabling then enabling the collider
	private IEnumerator Fall() {
		this.GetComponent<BoxCollider2D>().enabled = false;
		yield return new WaitForSeconds(0.2f);
		if (grounded)
		{
		this.GetComponent<BoxCollider2D>().enabled = true;
		}

	}

}