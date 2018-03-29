using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {

    private Controller2D controller;
    private Animator animator;
    private AudioController audioController;

	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        audioController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>();
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
            audioController.playDeathSound();
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