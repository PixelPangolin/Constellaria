/*
 * Copyright (c) 2017 Razeware LLC
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * Notwithstanding the foregoing, you may not use, copy, modify, merge, publish, 
 * distribute, sublicense, create a derivative work, and/or sell copies of the 
 * Software in any work that is designed, intended, or marketed for pedagogical or 
 * instructional purposes related to programming, coding, application development, 
 * or information technology.  Permission for such use, copying, modification,
 * merger, publication, distribution, sublicensing, creation of derivative works, 
 * or sale is expressly withheld.
 *    
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 */

using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed = 1f;
    public float jumpSpeed = 3f;
    public bool groundCheck;
    public bool isSwinging;
    private SpriteRenderer playerSprite;
    private Rigidbody2D rBody;
    private bool isJumping;
    private Animator animator;
    private float jumpInput;
    private float horizontalInput;
	public Vector2 ropeHook;
	public float swingForce = 4f;

    void Awake()
    {
        playerSprite = GetComponent<SpriteRenderer>();
        rBody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        jumpInput = Input.GetAxis("Jump");
        horizontalInput = Input.GetAxis("Horizontal");
        var halfHeight = transform.GetComponent<SpriteRenderer>().bounds.extents.y;
        groundCheck = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y - halfHeight - 0.04f), Vector2.down, 0.025f);
    }

	void FixedUpdate()
	{
		if (horizontalInput < 0f || horizontalInput > 0f)
		{
			animator.SetFloat("Speed", Mathf.Abs(horizontalInput));
			playerSprite.flipX = horizontalInput < 0f;
			if (isSwinging)
			{
				animator.SetBool("IsSwinging", true);

				// 1 - Get a normalized direction vector from the player to the hook point
				var playerToHookDirection = (ropeHook - (Vector2)transform.position).normalized;

				// 2 - Inverse the direction to get a perpendicular direction
				Vector2 perpendicularDirection;
				if (horizontalInput < 0)
				{
					perpendicularDirection = new Vector2(-playerToHookDirection.y, playerToHookDirection.x);
					var leftPerpPos = (Vector2)transform.position - perpendicularDirection * -2f;
					Debug.DrawLine(transform.position, leftPerpPos, Color.green, 0f);
				}
				else
				{
					perpendicularDirection = new Vector2(playerToHookDirection.y, -playerToHookDirection.x);
					var rightPerpPos = (Vector2)transform.position + perpendicularDirection * 2f;
					Debug.DrawLine(transform.position, rightPerpPos, Color.green, 0f);
				}

				var force = perpendicularDirection * swingForce;
				rBody.AddForce(force, ForceMode2D.Force);
			}
			else
			{
				animator.SetBool("IsSwinging", false);
				if (groundCheck)
				{
					var groundForce = speed * 2f;
					rBody.AddForce(new Vector2((horizontalInput * groundForce - rBody.velocity.x) * groundForce, 0));
					rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y);
				}
			}
		}
		else
		{
			animator.SetBool("IsSwinging", false);
			animator.SetFloat("Speed", 0f);
		}

		if (!isSwinging)
		{
			if (!groundCheck) return;

			isJumping = jumpInput > 0f;
			if (isJumping)
			{
				rBody.velocity = new Vector2(rBody.velocity.x, jumpSpeed);
			}
		}
	}
}
