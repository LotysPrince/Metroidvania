using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pitcherPlantBehavior : MonoBehaviour
{

    private Vector3 ceilingAttachPoint;

    //player detection objects///////////
    public Transform objectDetection1;
    private Vector3 objectDetection1position;
    public Transform objectDetection2;
    private Vector3 objectDetection2position;
    /////////////////////////////////////
    public LayerMask HumanoidsLayer;
    public Animator animator;

    //player script and sprite references to remove control and delete sprite temporarily when eatten
    public playerMovement scientistControl;
    public SpriteRenderer scientistSprite;
    private bool entityCaptured = false;
    private bool alive = true;


    //RectTransform plantTransform;
    // Start is called before the first frame update
    void Start()
    {
        ceilingAttachPoint = transform.position;
        objectDetection1position = objectDetection1.position;
        objectDetection2position = objectDetection2.position;

    }

    // Update is called once per frame
    void Update()
    {
        CheckDetection();
    }

    /// <summary>
    /// Checks if the player is running beneath the plant
    /// </summary>
    public void CheckDetection()
    {
        if (!entityCaptured && alive)
        {
          
            Collider2D[] colliders = Physics2D.OverlapCircleAll(objectDetection1.position, .2f, HumanoidsLayer);
            Collider2D[] colliders2 = Physics2D.OverlapCircleAll(objectDetection2.position, .2f, HumanoidsLayer);

            for (int i = 0; i < colliders.Length; i++)
            {
                if (colliders[i].gameObject != gameObject)
                {

                    animator.SetInteger("plantTrigger", 1);
                    transform.position = new Vector3(transform.position.x, transform.position.y, -5.1f);
                    scientistControl.canMove = false;
                    scientistControl.stoppedPoint = objectDetection1;
                    //scientistControl.rb2d.velocity = new Vector2(0, -10);

                    break;


                }
            }
            for (int i = 0; i < colliders2.Length; i++)
            {
                if (colliders2[i].gameObject != gameObject)
                {
                    animator.SetInteger("plantTrigger", 1);
                    transform.position = new Vector3(transform.position.x, transform.position.y, -5.1f);
                    scientistControl.canMove = false;
                    scientistControl.stoppedPoint = objectDetection1;
                    //scientistControl.rb2d.velocity = new Vector2(0, -10);



                    break;
                }
            }

        }
    }


    /// <summary>
    /// Because of the way animations work in Unity, has to adjust the position of the sprite on certain frames
    /// </summary>
    public void LowerSpriteAnimation()
    {
        transform.position = new Vector3(transform.position.x, ceilingAttachPoint.y - 1.2f, transform.position.z);
        objectDetection1.position = objectDetection1position;
        objectDetection2.position = objectDetection2position;


    }

    public void LowerSpriteForDeathAnim()
    {
        transform.position = new Vector3(transform.position.x, ceilingAttachPoint.y - 1.2f, transform.position.z);
        objectDetection1.position = objectDetection1position;
        objectDetection2.position = objectDetection2position;
    }


    /// <summary>
    /// Deletes player sprite when triggered
    /// </summary>
    public void EatPlayer()
    {
        scientistSprite.enabled = false;
        entityCaptured = true;
    }

    /// <summary>
    /// If the scientist has a scalpel on hand, will decide whether to kill the plant or kill the player
    /// </summary>
    public void checkForDeath()
    {
        if (scientistControl.scalpelEquipped == true)
        {
            RaiseSpriteAnimation();
            animator.SetInteger("plantTrigger", 2);
        }
        else
        {
            animator.SetInteger("plantTrigger", 1);
        }
    }

    /// <summary>
    /// Trigger death animation and return control to player
    /// </summary>
    public void plantDeath()
    {
        alive = false;
        scientistSprite.enabled = true;
        scientistControl.canMove = true;
        animator.SetInteger("plantTrigger", 3);
    }

    /// <summary>
    /// Adjust sprite position for janky animations
    /// </summary>
    public void RaiseSpriteAnimation()
    {
        transform.position = ceilingAttachPoint;
        objectDetection1.position = objectDetection1position;
        objectDetection2.position = objectDetection2position;
    }

    /// <summary>
    /// Ends plant animation
    /// </summary>
    public void EndAnimation()
    {
        animator.SetInteger("plantTrigger", 0);

    }
}
