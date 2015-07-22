using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
	Rigidbody2D rigidbody2D;

	// move left & right
	public float maxSpeed = 10f;
	bool facingRight = true;

	// animator controller
	Animator anim;

	// jump & not on the ground state
	bool grounded = false;
	public Transform groundCheck;
	public float groundRadius = 0.2f;
	public LayerMask whatIsGround;
	public float jumpForce = 700f;

	bool doubleJump = false;

	public Transform wallLeftCheck;
	public Transform wallRightCheck;
	public float wallRadius = 0.2f;
	public LayerMask whatIsWall;
	bool wallLeftPushing = false;
	bool wallRightPushing = false;

	public float wallJumpForce = 700f;

	// override methods
	void Start ()
	{
		rigidbody2D = GetComponent<Rigidbody2D> ();
		anim = GetComponent<Animator> ();
	}

	void Update ()
	{
		if (Input.GetKeyDown(KeyCode.C))
		{
			anim.SetBool("Ground", false);

			Vector2 velocity = Vector2.zero;
			Vector2 force = Vector2.zero;

			if (grounded)
			{
				velocity = new Vector2(this.rigidbody2D.velocity.x, 0);
				force = new Vector2(0, jumpForce);
			}
			else if (wallLeftPushing || wallRightPushing)
			{
				velocity = new Vector2(0, 0);
				force = new Vector2(-wallJumpForce, jumpForce);
			}

			this.rigidbody2D.velocity = velocity;
			this.rigidbody2D.AddForce(force);
		}
		return;

		/*
		if ((grounded || !doubleJump) && Input.GetKeyDown (KeyCode.Space)) 
		{
			anim.SetBool("Ground", false);

			this.rigidbody2D.velocity = new Vector2(this.rigidbody2D.velocity.x, 0);
			this.rigidbody2D.AddForce(new Vector2(0, jumpForce));

			if (!doubleJump && !grounded)
				doubleJump = true;
		}
		*/
	}

	void FixedUpdate ()
	{
		grounded = Physics2D.OverlapCircle (groundCheck.position, groundRadius, whatIsGround);
		anim.SetBool ("Ground", grounded);

		if (grounded)
			doubleJump = false;

		anim.SetFloat ("vSpeed", this.rigidbody2D.velocity.y);

		/*
		float move = Input.GetAxis ("Horizontal");
		anim.SetFloat ("Speed", Mathf.Abs (move));
		
		rigidbody2D.velocity = new Vector2 (move * maxSpeed, rigidbody2D.velocity.y);
		
		if (move > 0 && !facingRight)
			Flip ();
		else if (move < 0 && facingRight)
			Flip ();
			*/

		bool isLeftKeyDown = Input.GetKey (KeyCode.LeftArrow);
		bool isRightKeyDown = Input.GetKey (KeyCode.RightArrow);

		wallLeftPushing = (Physics2D.OverlapCircle (wallLeftCheck.position, wallRadius, whatIsWall) && isLeftKeyDown);
		wallRightPushing = (Physics2D.OverlapCircle (wallRightCheck.position, wallRadius, whatIsWall) && isRightKeyDown);
		anim.SetBool("WallLeftPushing", wallLeftPushing);
		anim.SetBool("WallRightPushing", wallRightPushing);

		if (isLeftKeyDown) {
			anim.SetFloat ("Speed", 0.011f);

			rigidbody2D.velocity = new Vector2 (-maxSpeed, rigidbody2D.velocity.y);

			if (facingRight == true)
				Flip ();
		} else if (isRightKeyDown) {
			anim.SetFloat ("Speed", 0.011f);

			rigidbody2D.velocity = new Vector2 (maxSpeed, rigidbody2D.velocity.y);

			if (facingRight == false)
				Flip ();
		} else {
			anim.SetFloat ("Speed", 0.0f);

			rigidbody2D.velocity = new Vector2 (0, rigidbody2D.velocity.y);
		}
	}
    
	// methods
	void Flip ()
	{
		facingRight = !facingRight;
		Vector3 theScale = transform.localScale;
		theScale.x *= -1;
		transform.localScale = theScale;
	}
}
