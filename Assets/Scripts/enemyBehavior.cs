using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyBehavior : MonoBehaviour
{
    public string patrolMode; //planning to add more 'modes' depending on enemy behaviour I want

    public int moveSpeed;

    //collision detections
    public Transform wallCheck;
    public Transform wallCheck2;
    public Transform edgeCheck;
    public Transform edgeCheck2;
    public LayerMask GroundLayer;
    private bool flipped = false;
    private bool walkingOnGround = true;
    private bool walkingOnGround2 = true;

    //field of view
    [SerializeField] private fieldOfView fieldOfView;
    public float aimDir = 225f; //sets angle for field of view, for some reason doesnt effect it on start, only when changing during gameplay, need to look into it more




    public Animator animator;
    Rigidbody2D rb2d;
    float scaleX;


    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        scaleX = transform.localScale.x;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        //sets field of view point
        fieldOfView.SetOrigin(transform.position);
        fieldOfView.SetAimDirection(aimDir);
        Move();
        WallCheck();
        EdgeCheck();
    }

    public void Move()
    {
        //Flip();
        rb2d.velocity = new Vector2(moveSpeed, rb2d.velocity.y);
    }

    /// <summary>
    /// Checks for when enemy runs into a wall
    /// </summary>
    public void WallCheck()
    {
        //Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheck.position, .1f, GroundLayer);
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(wallCheck2.position, .1f, GroundLayer);
        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(wallCheck.position, 1);

        /*for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {

                //animator.SetBool("Jumping", false);
                flipped = true;
                Flip();
                break;
                //moveSpeed = -moveSpeed;
                
                //Debug.Log(flipped);

            }
        }*/

        //enemy turns around when about to collide
        if (!flipped)
        {
            for (int i = 0; i < colliders2.Length; i++)
            {
                if (colliders2[i].gameObject != gameObject)
                {
                    //animator.SetBool("Jumping", false);
                    flipped = true;
                    Flip();
                    break;
                    //moveSpeed = -1 * moveSpeed;

                }
            }
        }
        flipped = false;


    }

    /// <summary>
    /// Checks if the enemy is about to run off the edge, turns them around if so
    /// Uses two checks because stairs sometimes trick it into thinking the enemy is about to run off an edge so it needs two
    /// </summary>
    public void EdgeCheck()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(edgeCheck.position, .2f, GroundLayer);
        Collider2D[] colliders2 = Physics2D.OverlapCircleAll(edgeCheck2.position, .2f, GroundLayer);

        //Gizmos.color = Color.yellow;
        //Gizmos.DrawSphere(wallCheck.position, 1);
        walkingOnGround = false;
        walkingOnGround2 = false;

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {

                //animator.SetBool("Jumping", false);
                walkingOnGround = true;
                //Flip();
                break;
                //moveSpeed = -moveSpeed;

                //Debug.Log(flipped);

            }
        }
        for (int i = 0; i < colliders2.Length; i++)
        {
            if (colliders2[i].gameObject != gameObject)
            {

                //animator.SetBool("Jumping", false);
                walkingOnGround2 = true;
                //Flip();
                break;
                //moveSpeed = -moveSpeed;

                //Debug.Log(flipped);

            }
        }

        //enemy turns around istead of walking off the edge
        if (!walkingOnGround && !walkingOnGround2)
        {
            Flip();
            //walkingOnGround = true;
        }
    }

    public void Flip()
    {
        if(moveSpeed < 0)
        {
            //fieldOfView.angle += 180f;
            aimDir = 45f;

            moveSpeed = Mathf.Abs(moveSpeed);
            transform.localScale = new Vector3(Mathf.Abs(transform.localScale.x), transform.localScale.y, transform.localScale.z);

        }
        else
        {
            aimDir = 225f;
            //fieldOfView.angle -= 180f;

            moveSpeed = -moveSpeed;
            transform.localScale = new Vector3((-1) * transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
        //fieldOfView.angle += 180f;

    }
}
