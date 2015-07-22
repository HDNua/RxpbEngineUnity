using UnityEngine;
using System.Collections;

public class CharacterController : MonoBehaviour
{
    Rigidbody2D _rigidbody2D;

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

    // shot
    public Transform shotPosition;
    public float shotSpeed = 20f;
    public bool shotTriggered = false;

    // override methods
    void Start()
    {
        _rigidbody2D = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        shotSpeed = maxSpeed * 2;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            anim.SetBool("Ground", false);

            Vector2 velocity = Vector2.zero;
            Vector2 force = Vector2.zero;

            //            bool hasToUpdate = false;
            if (grounded)
            {
                //				velocity = new Vector2(this.rigidbody2D.velocity.x, 0);
                force = new Vector2(0, jumpForce);
                //                rigidbody2D.velocity = velocity;
                _rigidbody2D.AddForce(force);
            }
        }

        if (Input.GetKeyDown(KeyCode.X))
        {
            print("TEST");
        }
    }

    void FixedUpdate()
    {
        // get state
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundRadius, whatIsGround);
        anim.SetBool("Ground", grounded);

        anim.SetFloat("vSpeed", _rigidbody2D.velocity.y);

        bool isLeftKeyDown = Input.GetKey(KeyCode.LeftArrow);
        bool isRightKeyDown = Input.GetKey(KeyCode.RightArrow);

        bool shotTriggered = Input.GetKeyDown(KeyCode.X);
        anim.SetBool("ShotTriggered", shotTriggered);

        // handle by state
        if (isLeftKeyDown)
        {
            anim.SetFloat("Speed", 0.011f);
            _rigidbody2D.velocity = new Vector2(-maxSpeed, _rigidbody2D.velocity.y);
            if (facingRight == true)
                Flip();
        }
        else if (isRightKeyDown)
        {
            anim.SetFloat("Speed", 0.011f);
            _rigidbody2D.velocity = new Vector2(maxSpeed, _rigidbody2D.velocity.y);
            if (facingRight == false)
                Flip();
        }
        else
        {
            anim.SetFloat("Speed", 0.0f);
            _rigidbody2D.velocity = new Vector2(0, _rigidbody2D.velocity.y);
        }
    }

    // methods
    void Flip()
    {
        facingRight = !facingRight;
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }
}
