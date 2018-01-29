// Created with help from  https://forum.unity.com/threads/forcing-a-character-to-fall-through-a-platform.222020/#post-1480670
using UnityEngine;
using System.Collections;

public class BadLine : MonoBehaviour {

	public Transform groundCheck;

	private bool onLine = false;
	private BoxCollider2D boxCollider;


	void Awake () 
	{
		boxCollider = GetComponent<BoxCollider2D>();
	}

	// Update is called once per frame
	void Update () 
	{
		onLine = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Lines"));

		if (Input.GetButtonDown("Down") && onLine) 
		{
			StartCoroutine("Fall");
		}
	}

	// Makes the character fall by disabling then enabling the collider
	private IEnumerator Fall() {
		boxCollider.isTrigger = true;
		yield return new WaitForSeconds(0.2f);
		if (Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Lines")))
		{
			boxCollider.isTrigger = false;
		}

	}

}