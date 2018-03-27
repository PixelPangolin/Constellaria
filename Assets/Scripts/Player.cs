
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    // Use this for initialization
    void Start()
    {
        controller = GetComponent<Controller2D>();
        animator = GetComponent<Animator>();

        // Note that gravity has to be negative, hence the -1
        gravity = (-1)*(2 * maxJumpHeight) / Mathf.Pow(timeToJumpApex, 2);
        maxJumpVelocity = Mathf.Abs(gravity) * timeToJumpApex;
        minJumpVelocity = Mathf.Sqrt(2 * Mathf.Abs(gravity) * minJumpHeight);
    }

    // Update is called once per frame
    void Update()
    {
        CalculateVelocity();

        controller.Move(velocity * Time.deltaTime);

        // This makes our falling make more sense
        // Due to how our gravity is implemented, this makes sure we don't just fall really quickly when we step off a platform
        if (controller.collisions.above || controller.collisions.below)
        {
            velocity.y = 0;
        }

        Debug.Log(velocity.x);
        Debug.Log(velocity.y);

        // Animations for player character
        if (velocity.x < 0)
        {
            TurnLeft();
            animator.SetFloat("Speed", (velocity.x));
        }
        else if (velocity.x > 0)
        {
            TurnRight();
            animator.SetFloat("Speed", (velocity.x));
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
