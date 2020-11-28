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
	private bool right = true;

	// Jumping
	public float jumpForce = 400f;							
	public Transform groundCheck;
	const float groundRadius = .2f;
	private bool grounded;
	private bool doubleJumpAvailable = true;

	// Dash
	public float m_DashForce = 25f;
	public float dashCooldown = 0.5f;
	private bool canDash = true;
	private bool isDashing = false;
	
	// Wall sliding
	private bool isWall = false; //If there is a wall in front of the player
	private bool isWallSliding = false; //If player is sliding in a wall
	private bool oldWallSlidding = false; //If player is sliding in a wall in the previous frame
	private bool canCheck = false; //For check if player is wallsliding

	private void FixedUpdate()
	{
		
		grounded = false;

		Collider2D[] collidersGround = Physics2D.OverlapCircleAll(groundCheck.position, groundRadius);
		if (collidersGround.Length > 0) {
			grounded = true;
			doubleJumpAvailable = true;
		}

		animator.SetBool("IsJumping", Math.Abs(body.velocity.y) > 0.5 && !grounded);
		// isWall = false;
		// if (!grounded)
		// {
		// 	Collider2D[] collidersWall = Physics2D.OverlapCircleAll(wallCheck.position, groundRadius);
		// 	for (int i = 0; i < collidersWall.Length; i++)
		// 	{
		// 		isDashing = false;
		// 		isWall = true;
		// 	}
		// }
	}


	public void Move(float move, bool jump, bool dash)
	{
		if (dash && canDash && !isWallSliding)
		{
			StartCoroutine(DashCooldown());
		}

		if (isDashing) {
			body.velocity = new Vector2(transform.localScale.x * m_DashForce, 0);
		} else {
			body.velocity = new Vector2(move*10f, body.velocity.y);
		}


		if (!isWallSliding) {
			if ((move > 0 && !right && !isWallSliding) || (move < 0 && right)) {
				Flip();
			}
		}
		
		if (grounded && jump) {
			grounded = false;
			body.AddForce(new Vector2(0f, jumpForce));
			animator.SetBool("IsJumping", true);
		} else if (!grounded && jump && doubleJumpAvailable && !isWallSliding) {
			doubleJumpAvailable = false;
			body.velocity = new Vector2(body.velocity.x, 0);
			body.AddForce(new Vector2(0f, jumpForce));
			animator.SetBool("IsJumping", true);
		}
		// else if (isWall && !grounded) {
		// 	if (!oldWallSlidding && body.velocity.y < 0 || isDashing)
		// 	{
		// 		isWallSliding = true;
		// 		wallCheck.localPosition = new Vector3(-wallCheck.localPosition.x, wallCheck.localPosition.y, 0);
		// 		Flip();
		// 		StartCoroutine(WaitToCheck(0.1f));
		// 		doubleJumpAvailable = true;
		// 		animator.SetBool("IsWallSliding", true);
		// 	}
		// 	isDashing = false;

		// 	if (isWallSliding)
		// 	{
		// 		if (move * transform.localScale.x > 0.1f)
		// 		{
		// 			StartCoroutine(WaitToEndSliding());
		// 		}
		// 		else 
		// 		{
		// 			oldWallSlidding = true;
		// 			body.velocity = new Vector2(-transform.localScale.x * 2, -5);
		// 		}
		// 	}

		// 	if (jump && isWallSliding)
		// 	{
		// 		animator.SetBool("IsJumping", true);
		// 		animator.SetBool("JumpUp", true); 
		// 		body.velocity = new Vector2(0f, 0f);
		// 		body.AddForce(new Vector2(transform.localScale.x * jumpForce *1.2f, jumpForce));
		// 		doubleJumpAvailable = true;
		// 		isWallSliding = false;
		// 		animator.SetBool("IsWallSliding", false);
		// 		oldWallSlidding = false;
		// 		wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
		// 	}
		// 	else if (dash && canDash)
		// 	{
		// 		isWallSliding = false;
		// 		animator.SetBool("IsWallSliding", false);
		// 		oldWallSlidding = false;
		// 		wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
		// 		doubleJumpAvailable = true;
		// 		StartCoroutine(DashCooldown());
		// 	}
		// } else if (isWallSliding && !isWall && canCheck) {
		// 	isWallSliding = false;
		// 	animator.SetBool("IsWallSliding", false);
		// 	oldWallSlidding = false;
		// 	wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
		// 	doubleJumpAvailable = true;
		// }
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

	// IEnumerator WaitToEndSliding()
	// {
	// 	yield return new WaitForSeconds(0.1f);
	// 	doubleJumpAvailable = true;
	// 	isWallSliding = false;
	// 	animator.SetBool("IsWallSliding", false);
	// 	oldWallSlidding = false;
	// 	wallCheck.localPosition = new Vector3(Mathf.Abs(wallCheck.localPosition.x), wallCheck.localPosition.y, 0);
	// }
}
