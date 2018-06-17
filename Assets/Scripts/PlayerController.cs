using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public float maxSpeed = 4;
    public float jumpForce = 400;
    public float minHeight, maxHeight;

    private float currentSpeed;
    private Rigidbody rb;
    private Animator animator;
    private Transform groundCheck;
    private bool onGround;
    private bool isDead;
    private bool facingRight = true;
    private bool jump = false;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();
        groundCheck = gameObject.transform.Find("GroundCheck");
        currentSpeed = maxSpeed;
	}
	
	// Update is called once per frame
	void Update () {
        onGround = Physics.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        animator.SetBool("OnGround", onGround);
        animator.SetBool("Dead", isDead);

        if (Input.GetButtonDown("Jump") && onGround)
        {
            jump = true;
        }

        if (Input.GetButtonDown("Fire1"))
        {
            animator.SetTrigger("Attack");
        }
	}

    void FixedUpdate()
    {
        if (!isDead)
        {
            float h = Input.GetAxis("Horizontal");
            float z = Input.GetAxis("Vertical");

            if (!onGround)
            {
                z = 0;
            }

            rb.velocity = new Vector3(h * currentSpeed, rb.velocity.y, z * currentSpeed);

            if (onGround)
            {
                animator.SetFloat("Speed", Mathf.Abs(h));
            }

            if ((h > 0 && !facingRight) || (h < 0 && facingRight))
            {
                Flip();
            }

            if (jump)
            {
                jump = false;

                rb.AddForce(Vector3.up * jumpForce);
            }

            float minWidth = Camera.main.ScreenToWorldPoint(new Vector3(0, 0, 10)).x;
            float maxWidth = Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0, 10)).x;
            rb.position = new Vector3(Mathf.Clamp(rb.position.x, minWidth + 0.01f, maxWidth - 0.01f), 
                rb.position.y, 
                Mathf.Clamp(rb.position.z, minHeight + 0.01f, maxHeight - 0.01f));
        }
    }

    void Flip()
    {
        facingRight = !facingRight;

        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    public void ZeroSpeed()
    {
        currentSpeed = 0;
    }

    public void ResetSpeed()
    {
        currentSpeed = maxSpeed;
    }
}
