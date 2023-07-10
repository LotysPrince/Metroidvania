using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMovement : MonoBehaviour
{
    /// <summary>
    /// Player movement variables
    /// </summary>
    private bool jump = false;
    private bool crouch = false;
    private bool sprint = false;
    public int moveSpeed;
    public int jumpForce;
    private float maxJumpVel = 20;
    private float moveInput;
    public Transform GroundCheck;
    public Transform CeilingCheck;
    public LayerMask GroundLayer;
    public bool isGrounded = true;
    private bool isCrouched = false;

    /// <summary>
    /// Crouching variables
    /// </summary>
    public BoxCollider2D crouchingHitbox1;
    public CircleCollider2D crouchingHitbox2;
    public Transform changeCrouchSprite;
    private Vector3 originalSpritePos;   //To adjust sprite position when crouching to account for weird yPos interactions with Unity
    private bool changeSpriteOnce = true;

    public Animator animator;

    public Rigidbody2D rb2d;
    float scaleX;

    //Removes movement if captured/trapped
    public bool canMove = true;
    public Transform stoppedPoint;

    // TESTING///////////////////////////////
    public bool scalpelEquipped;






    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;
    }

    // Update is called once per frame
    /// <summary>
    /// Movement inputs
    /// </summary>
    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");


        if (Input.GetButtonDown("Jump"))
        {
            jump = true;

        }
        if (Input.GetButtonDown("Crouch"))
        {
            crouch = true;
        } else if (Input.GetButtonUp("Crouch"))
        {
            crouch = false;
            isCrouched = false;
            //re-enables head and torso hitboxes when standing up
            crouchingHitbox1.enabled = true;
            crouchingHitbox2.enabled = true;
        }
        if (Input.GetButtonDown("Sprint"))
        {
            sprint = true;
        } else if (Input.GetButtonUp("Sprint"))
        {
            sprint = false;
        }
        

    }

    private void FixedUpdate()
    {
        originalSpritePos = changeCrouchSprite.position;
        groundedCheck();
        moveSpeed = 0; //adjusts movement speed based on sprinting/crouching/walking
        if (canMove)
        {
            Jump();
            Crouch();
            Move();
        }
        if (!canMove)
        {
            //if the player is trapped, they will lose control and move towards the spot where they are trapped so they dont get trapped in the wrong position
            rb2d.velocity = new Vector2(0, -10);
            Vector2 tempVector = Vector2.MoveTowards(transform.position, stoppedPoint.position, 3 * Time.deltaTime);
            transform.position = new Vector3(tempVector.x, tempVector.y, -5);
        }

        animator.SetFloat("Speed", moveSpeed); //changes animation based on movement speed

    }

    /// <summary>
    /// Moves player
    /// </summary>
    public void Move()
    {
        Flip();
        rb2d.velocity = new Vector2(moveInput * moveSpeed, rb2d.velocity.y) ;
    }

    /// <summary>
    /// Checks if the player is grounded in order to allow them to jump from the ground
    /// </summary>
    public void groundedCheck()
    {
        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(GroundCheck.position, .2f, GroundLayer);
        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {
                isGrounded = true;
                animator.SetBool("Jumping", false);


            }
        }
    }

    /// <summary>
    /// Jumping mechanics
    /// </summary>
    public void Jump()
    {
        if (isGrounded && jump)
        {
            // Add a vertical force to the player.
            animator.SetBool("Jumping", true);
            isGrounded = false;
            rb2d.AddForce(new Vector2(0f, jumpForce));
        }
        rb2d.velocity = Vector3.ClampMagnitude(rb2d.velocity, maxJumpVel); // clamps the vertical magnitude so that the player doesnt fly upwards when not intended, could probably be done better if different method found
        jump = false;
    }


    /// <summary>
    /// Crouching systems
    ///         - still have to implement blocking the ability to stand while under a platform
    /// </summary>
    public void Crouch()
    {
        if (isGrounded && crouch)
        {
            isCrouched = true;
            crouchingHitbox1.enabled = false;
            crouchingHitbox2.enabled = false;
            moveSpeed = 2;
            if (changeSpriteOnce)
            {
                changeCrouchSprite.position = new Vector3(originalSpritePos.x, originalSpritePos.y - .4f, originalSpritePos.z);
                changeSpriteOnce = false;
            }


        }
        if (!crouch || !isGrounded)
        {
            if (!changeSpriteOnce)
            {
                changeCrouchSprite.position = new Vector3(originalSpritePos.x, originalSpritePos.y + .4f, originalSpritePos.z);
                changeSpriteOnce = true;
            }
        }
    }


    /// <summary>
    /// Flips the player sprite, flips according to whether movement speed is negative or positive to determine direction.
    /// </summary>
    public void Flip()
    {
        if (moveInput > 0)
        {
            if (!isCrouched)
            {
                moveSpeed = 4;
                if (sprint)
                {
                    moveSpeed = 8;
                }
            }

            transform.localScale = new Vector3(scaleX, transform.localScale.y, transform.localScale.z);
        }
        if (moveInput < 0)
        {
            if (!isCrouched)
            {
                moveSpeed = 4;
                if (sprint)
                {
                    moveSpeed = 8;
                }
            }
            transform.localScale = new Vector3((-1) * scaleX, transform.localScale.y, transform.localScale.z);
        }
        if (moveInput == 0)
        {
            if (!isCrouched)
            {
                moveSpeed = 0;
            }
        }
    }


}

