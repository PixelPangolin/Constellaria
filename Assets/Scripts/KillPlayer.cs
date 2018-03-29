using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {

    private Controller2D controller;
    private Animator animator;
    private GameObject player;

	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
    }

	// Update is called once per frame
	void Update () {
        if (controller.playerDeath)
        {
            PlayerDeath();
        }
	}

	void PlayerDeath()
	{
        float delayTime = 2f;
        if (GetComponent<GrapplingHook>().currentNode)
        {
            animator.SetTrigger("Die");
            GameObject.Find("GameController").GetComponent<AudioController>().playDeathSound();
            //Invoke("TeleportPlayerToCheckpoint", delayTime);
            TeleportPlayerToCheckpoint();
        }
        else
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}

    void TeleportPlayerToCheckpoint()
    {
        transform.position = GetComponent<GrapplingHook>().currentNode.transform.position;
        controller.playerDeath = false;
    }
}