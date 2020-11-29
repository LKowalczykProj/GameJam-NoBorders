using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using UnityEngine.SceneManagement;
using System;

public class PlayerController : MonoBehaviour
{
	[SerializeField] private Transform wallCheck;	

	private Rigidbody2D body => GetComponent<Rigidbody2D>();
	private Animator animator => GetComponent<Animator>();

	// Movement
	public bool canMove = true;
	private bool right = true;

	// Jumping
	public float jumpForce = 400f;							
	public Transform groundCheck;
	const float groundRadius = .01f;
	private bool grounded;
	private bool doubleJumpAvailable = true;

	// Dash
	public float dashForce = 25f;
	public float dashCooldown = 0.5f;
	private bool canDash = true;
	private bool isDashing = false;
	
	// Wall sliding
	public float wallSlidingSpeed = 1f;
	private bool isWall = false;
	private bool isWallSliding = false;

	private void FixedUpdate()
	{
		
		grounded = false;

		Collider2D[] collidersGround = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius);
		if (collidersGround.Length > 0) {
			grounded = true;
			doubleJumpAvailable = true;
		}
		animator.SetBool("IsJumping", Math.Abs(body.velocity.y) > 0.5 && !grounded);
		
		isWall = false;
		if (!grounded) {
			Collider2D[] collidersWall = Physics2D.OverlapCircleAll(wallCheck.position, groundRadius);
			if (collidersWall.Length > 0) {
				isDashing = false;
				isWall = true;
			}
		}		
		Debug.Log(animator.GetBool("IsJumping"));

	}


	public void Move(float move, bool jump, bool dash)
	{
		if (!canMove) return;
		if (dash && canDash && !isWallSliding) {
			// Dashing
			StartCoroutine(DashCooldown());
		}

		if (isDashing) {
			body.velocity = new Vector2(transform.localScale.x * dashForce, 0);
		} else {
			body.velocity = new Vector2(move*10f, body.velocity.y);
		}


		if (!isWallSliding) {
			// Player direction
			if ((move > 0 && !right && !isWallSliding) || (move < 0 && right)) {
				Flip();
			}
		}
		
		if (grounded && jump) {
			// Jump
			grounded = false;
			body.velocity = new Vector2(body.velocity.x, 0);
			body.AddForce(new Vector2(0f, jumpForce));
			animator.SetBool("IsJumping", true);
		} else if (!grounded && jump && doubleJumpAvailable && !isWallSliding) {
			// Double
			doubleJumpAvailable = false;
			body.velocity = new Vector2(body.velocity.x, 0);
			body.AddForce(new Vector2(0f, jumpForce));
			animator.SetBool("IsJumping", true);
		} else if (isWall && !grounded) {
			if (!isWallSliding && body.velocity.y < 0) {
				isWallSliding = true;
				wallCheck.localPosition = new Vector3(-wallCheck.localPosition.x, wallCheck.localPosition.y, 0);
				Flip();
				doubleJumpAvailable = true;
				animator.SetBool("IsWallSliding", true);
			}
			
			if (isWallSliding) {
				if (move * transform.localScale.x > 0.1f) {
					animator.SetBool("IsWallSliding", false);
					isWallSliding = false;
				} else {
					body.velocity = new Vector2(-transform.localScale.x * 2, -wallSlidingSpeed);
				}
			}

			if (jump && isWallSliding) {
				animator.SetBool("IsJumping", true);
				body.velocity = new Vector2(0f, 0f);
				body.AddForce(new Vector2(transform.localScale.x * jumpForce * 1.2f, jumpForce));
				doubleJumpAvailable = true;
				isWallSliding = false;
				animator.SetBool("IsWallSliding", false);
				wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
			}
		} else {
			isWallSliding = false;
			animator.SetBool("IsWallSliding", false);
			wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
		}
	}

	private void Flip()
	{
		right = !right;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}

	IEnumerator DashCooldown()
	{
		animator.SetBool("IsDashing", true);
		isDashing = true;
		canDash = false;
		yield return new WaitForSeconds(0.1f);
		animator.SetBool("IsDashing", false);
		isDashing = false;
		yield return new WaitForSeconds(dashCooldown);
		canDash = true;
	}
}
