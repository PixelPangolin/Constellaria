
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[RequireComponent(typeof(Controller2D))]

public class Player : MonoBehaviour {

    // Helps set gravity and jumpVelocity - note that time is in seconds
    private float moveSpeed = 5;
    private float minJumpHeight = 1;
    private float maxJumpHeight = 5.5f;
    private float timeToJumpApex = 0.65f;
    private float accelerationTimeAirborne = 0.2f;
    private float accelerationTimeGrounded = 0.1f;


    // Equation assuming we're at the top of our jump should help explain gravity - jumpHeight = (gravity * timeToJumpApex**2)/2 so isolate gravity
    // Equation for velocity is jumpVelocity = gravity * timeToJumpApex
    private float gravity;
    private float minJumpVelocity;
    private float maxJumpVelocity;

    private Vector2 directionalInput;
    private Vector3 velocity;
    private float velocityXSmoothing;

    private Controller2D controller;
    private Animator animator;
    private GrapplingHook grapple;
	private Rigidbody2D rb2d;
	private CapsuleCollider2D capColl2d;

    private bool facingRight;

    public void SetDirectionalInput(Vector2 input)
    {
        directionalInput = input;
    }

    public void OnJumpInputDown()
    {
        if (controller.collisions.below)
        {
            velocity.y = maxJumpVelocity;

            // Animation for jumping
            animator.SetTrigger("Jump");
        }
    }
    public void OnJumpInputUp()
    {
        if (velocity.y > minJumpVelocity)
        {
            velocity.y = minJumpVelocity;
        }
    }
	public void OnShiftInputDown(){
		if (grapple.connected) {
			rb2d.gravityScale = 1f;
			rb2d.velocity = velocity;
			capColl2d.isTrigger = false;
		}
	}
	public void OnShiftInputUp(){
		if (grapple.connected) {
			rb2d.gravityScale = 0f;
			velocity = rb2d.velocity;
			rb2d.velocity = new Vector2 (0f, 0f);
			capColl2d.isTrigger = true;
		}
	}

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();
        grapple = GetComponent<GrapplingHook>();
		rb2d = GetComponent<Rigidbody2D> ();
		capColl2d = GetComponent<CapsuleCollider2D> ();

        // Note that gravity has to be negative, hence the -1
        gravity = (-1)*(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    // Update is called once per frame
    void Update()
    {




	
        // This makes our falling make more sense
        // Due to how our gravity is implemented, this makes sure we don't just fall really quickly when we step off a platform


		if (grapple.isSwinging) {
			animator.SetBool("isSwinging", true);
			animator.SetTrigger("Swing");
			CalculateVelocitySwing2();

		}
		if (!grapple.isSwinging) {
			animator.SetBool("isSwinging", false);
			CalculateVelocity();
			controller.Move(velocity * Time.deltaTime, directionalInput);
		}

		if (controller.collisions.above || controller.collisions.below  || controller.playerDeath)
		{
			velocity.y = 0;
		}



        // Animations for player character
		if (velocity.x < 1 )
        {
            TurnLeft();
            animator.SetFloat("Speed", (velocity.x));
        }
		else if (velocity.x > 1 )
        {
            TurnRight();
            animator.SetFloat("Speed", (velocity.x));
        }
    }

	void CalculateVelocitySwing2()
	{
		float targetVelocityX = directionalInput.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeAirborne);
		this.GetComponent<Rigidbody2D> ().AddForce (Vector2.right * velocity.x*10f);
	}

	void CalculateVelocitySwing()
	{

		// Smoothing movement in x
		float targetVelocityX = directionalInput.x * moveSpeed;
		//float angleA = Mathf.PI - Mathf.Atan2 ((grapple.ropePositions.Last ().y - transform.position.y), (grapple.ropePositions.Last ().x - transform.position.x));
		float angleA = Mathf.PI - Mathf.Atan2 ((grapple.ropePositions.Last ().y - transform.position.y), (grapple.ropePositions.Last ().x - transform.position.x));

		float convertedVelocityX = Mathf.Sin(angleA)*targetVelocityX;
		float convertedVelocityY = Mathf.Cos(angleA)*targetVelocityX;

		if (controller.collisions.below)
		{

			velocity.x = Mathf.SmoothDamp(velocity.x, convertedVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
		}
		else
		{
			velocity.x = Mathf.SmoothDamp(velocity.x, convertedVelocityX, ref velocityXSmoothing, accelerationTimeAirborne);
		}

		// Applying gravity to the player's velocity and moves accordingly
		velocity.x = (convertedVelocityX);
		velocity.y = (convertedVelocityY);

		//calculate future position
		Vector3 testPosition = transform.position;
		testPosition.x = testPosition.x + velocity.x;
		testPosition.y = testPosition.y + velocity.y;
		float newDistance = Vector3.Distance(testPosition, grapple.ropePositions.Last());

		//if future position farther away from hook than it should be, change movement to pendulum
		if (grapple.ropeDistance < newDistance){
			//find point between the grapple and testposition, one rope length away from grapple
			testPosition = Vector3.Lerp(testPosition, grapple.ropePositions.Last (),  grapple.ropeDistance/newDistance);
			velocity.x = (testPosition.x - transform.position.x);
			velocity.y = (testPosition.y - transform.position.y);
		}
	}

    void CalculateVelocity()
    {
        // Smoothing movement in x
        float targetVelocityX = directionalInput.x * moveSpeed;

        if (controller.collisions.below)
        {

            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeGrounded);
        }
        else
        {
            velocity.x = Mathf.SmoothDamp(velocity.x, targetVelocityX, ref velocityXSmoothing, accelerationTimeAirborne);
        }

        // Applying gravity to the player's velocity and moves accordingly
        velocity.y += gravity * Time.deltaTime;
    }

    void TurnRight()
    {
        facingRight = true;
        animator.SetBool("FacingRight", facingRight);
        animator.SetTrigger("Turn");
    }
    void TurnLeft()
    {
        facingRight = false;
        animator.SetBool("FacingRight", facingRight);
        animator.SetTrigger("Turn");
    }
}
