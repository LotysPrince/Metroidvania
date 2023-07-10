using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class inchWormBehavior : MonoBehaviour
{
    //Movement variables
    public int moveSpeedx;
    public int moveSpeedy;
    public LayerMask groundLayer;
    public string wormDirection;

    public Transform edgeCheck;
    public Transform wallCheck;
    private bool isGrounded;
    private bool wallSafe;
    public bool safeRotate;
    private int currentRotation;
    private bool startRotating;

    private Rigidbody2D rb2d;

    // Start is called before the first frame update
    void Start()
    {
        //start off facing left
        wormDirection = "Left";
        moveSpeedx = -3;
        moveSpeedy = 0;
        safeRotate = true;
        currentRotation = 0;
        startRotating = false;
        rb2d = GetComponent<Rigidbody2D>();
        isGrounded = true;

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        EdgeCheck();
        WallCheck();
        
        //rotates the image over time when it reaches the corner of a platform
        if (startRotating)
        {      
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.Euler(transform.rotation.x, transform.rotation.y, currentRotation), Time.deltaTime * 6);

            if (transform.rotation.z == currentRotation)
            {
                startRotating = false;
                safeRotate = true;
            }
        }
    }

    public void Move()
    {
        rb2d.velocity = new Vector2(moveSpeedx, moveSpeedy);
    }

    public void TurnLeft()
    {
        //called when it reaches the edge of a platform, changes direction counter clockwise
        if (wormDirection == "Left")
        {
            wormDirection = "Down";
            moveSpeedx = 0;
            moveSpeedy = -3;
        }

        else if (wormDirection == "Right")
        {
            wormDirection = "Up";
            moveSpeedx = 0;
            moveSpeedy = 3;
        }

        else if (wormDirection == "Up")
        {
            wormDirection = "Left";
            moveSpeedx = -3;
            moveSpeedy = 0;

        }

        else if (wormDirection == "Down")
        {
            wormDirection = "Right";
            moveSpeedx = 3;
            moveSpeedy = 0;

        }
        currentRotation += 90;
        startRotating = true;

    }


    public void TurnRight()
    {
        //changes direction clock wise
        if (wormDirection == "Left")
        {
            wormDirection = "Up";
            moveSpeedx = 0;
            moveSpeedy = 3;
        }

        else if (wormDirection == "Right")
        {
            wormDirection = "Down";
            moveSpeedx = 0;
            moveSpeedy = -3;
        }

        else if (wormDirection == "Up")
        {
            wormDirection = "Right";
            moveSpeedx = 3;
            moveSpeedy = 0;

        }

        else if (wormDirection == "Down")
        {
            wormDirection = "Left";
            moveSpeedx = -3;
            moveSpeedy = 0;

        }
        currentRotation -= 90;
        startRotating = true;
    }
    public void WallCheck()
    {

        //checks if worm runs into a wall

        wallSafe = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(wallCheck.position, .2f, groundLayer);


        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {

                wallSafe = true;
                safeRotate = true;
                break;
            }

        }

        if (wallSafe && safeRotate)
        {
            TurnRight();
            safeRotate = false;
        }
    }
    public void EdgeCheck()
    {

        //checks if worm is running off the edge

        isGrounded = false;
        Collider2D[] colliders = Physics2D.OverlapCircleAll(edgeCheck.position, .2f, groundLayer);


        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject != gameObject)
            {

                isGrounded = true;
                safeRotate = true;
                break;
            }

        }

        if (!isGrounded && safeRotate)
        {
            TurnLeft();
            safeRotate = false;
        }
    }
}
