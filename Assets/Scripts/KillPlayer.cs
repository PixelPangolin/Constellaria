using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KillPlayer : MonoBehaviour {

    private Controller2D controller;
    private Animator animator;
    private AudioController audioController;
	private Player playerMovement;
	private bool animating=false;

	// Use this for initialization
	void Start () {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        audioController = GameObject.FindGameObjectWithTag("GameController").GetComponent<AudioController>();
		playerMovement = GetComponent<Player> ();
    }

	// Update is called once per frame
	void Update () {
        if (controller.playerDeath)
        {
			if (!animating) {
				animating = true;
				playerMovement.velocity.x = 0;
				playerMovement.velocity.y = 0;
				PlayerDeath ();
			}
			if (animating) {
				XenoTeleportPlayerToCheckpoint ();
			}
        }
	}

	void PlayerDeath()
	{
        float delayTime = 2f;
        if (GetComponent<GrapplingHook>().currentNode)
        {
            animator.SetTrigger("Die");
			audioController.playDeathSound();
			GetComponent<GrapplingHook>().ResetLine();
			//XenoTeleportPlayerToCheckpoint();
			transform.GetComponent<GrapplingHook> ().enabled = false;
			transform.GetComponent<PlayerInput> ().enabled = false;
        }
        else
        {
            Application.LoadLevel(Application.loadedLevel);
        }
	}

    void TeleportPlayerToCheckpoint()
    {
        transform.position = GetComponent<GrapplingHook>().currentNode.transform.position;

    }
	void XenoTeleportPlayerToCheckpoint()
	{
		transform.position = Vector3.Lerp(transform.position, GetComponent<GrapplingHook>().currentNode.transform.position, Time.deltaTime*2);
		float distance = Vector3.Distance (GetComponent<GrapplingHook>().currentNode.transform.position, transform.position);

		if (distance < 3) {

			transform.GetComponent<GrapplingHook> ().enabled = true;
			transform.GetComponent<PlayerInput> ().enabled = true;
			controller.playerDeath = false;
			animating = false;

		}

	}
	//IEnumerator WaitThenControl(){
		

		//transform.position = transform.position + (GetComponent<GrapplingHook>().currentNode.transform.position - transform.position)*0.1f;
		//transform.position = Vector3.Lerp(transform.position, (transform.position + (GetComponent<GrapplingHook>().currentNode.transform.position - transform.position)), Time.deltaTime);
		//	player.GetComponent<Animator> ().applyRootMotion = false;

		//XenoTeleportPlayerToCheckpoint ();
	//}
}