using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovementController : MonoBehaviour
{
    float speed2 = 3f;
    public Transform movePoint;
    public LayerMask collideables;
    //Need animator here for animations
    public Animator animUp;
    public Animator animDown;
    public Animator animRight;
    public Animator animLeft;

    public GameObject upgradedStats;

    //Turning Animations
    public GameObject spriteUp;
    public GameObject spriteDown;
    public GameObject spriteRight;
    public GameObject spriteLeft;
    public bool attacking;

    public float upgradedSpeed;
    // Start is called before the first frame update
    void Start()
    {
        movePoint.parent = null;
    }

    /*
    // Changed to FixedUpdate
    void Update()
    {
		upgradedSpeed = upgradedStats.GetComponent<Stats>().upgradedAttackRate;
		transform.position = Vector3.MoveTowards(transform.position, movePoint.position, (speed + upgradedStats.GetComponent<Stats>().upgradedSpeed) * Time.deltaTime);
        GetInput();
    }*/
    float time = 0.0f;
    bool lockInput = false;

    void FixedUpdate()
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();
        // Vector2 temp = new Vector2(0, 0);
        upgradedSpeed = upgradedStats.GetComponent<Stats>().upgradedAttackRate;
        time += Time.deltaTime;
        rb.velocity = input() * speed2;
        if (!lockInput)
        {

           

        }
        else
        {
            //rb.velocity = new Vector2(0, 0);
        }
        //print(lockInput);
        if (time > 0.1f && lockInput)
        {
            lockInput = false;
            //rb.velocity = new Vector2(0, 0);
            time = 0;
        }
        //transform.position = Vector3.MoveTowards(transform.position, movePoint.position, (speed + upgradedStats.GetComponent<Stats>().upgradedSpeed) * Time.deltaTime);
        //GetInput();
    }
    private Vector2 input()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            SceneLoader.GoToDebug();
        }

        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            SceneLoader.GoToPause();
        }
        //if (!lockInput)
        {
            time = 0;

            if (Input.GetAxisRaw("Vertical") != 0)
            {
                lockInput = true;
                //print("this ranasdfasdfasdf");
                animDown.SetBool("Walking", true);
                animUp.SetBool("Walking", true);
                if (Input.GetKey(KeyCode.W) && !attacking)
                {
                    spriteUp.SetActive(true);
                    spriteDown.SetActive(false);
                    spriteRight.SetActive(false);
                    spriteLeft.SetActive(false);
                }
                else if (Input.GetKey(KeyCode.S) && !attacking)
                {
                    spriteUp.SetActive(false);
                    spriteDown.SetActive(true);
                    spriteRight.SetActive(false);
                    spriteLeft.SetActive(false);
                }
                return new Vector2(0f, Input.GetAxisRaw("Vertical"));
            }
            else if (Input.GetAxisRaw("Horizontal") != 0)
            {
                animRight.SetBool("Walking", true);
                animLeft.SetBool("Walking", true);
                lockInput = true;
                if (Input.GetKey(KeyCode.A) && !attacking)
                {
                    spriteUp.SetActive(false);
                    spriteDown.SetActive(false);
                    spriteRight.SetActive(false);
                    spriteLeft.SetActive(true);
                }
                else if (Input.GetKey(KeyCode.D) && !attacking)
                {
                    spriteUp.SetActive(false);
                    spriteDown.SetActive(false);
                    spriteRight.SetActive(true);
                    spriteLeft.SetActive(false);
                }
                return new Vector2(Input.GetAxisRaw("Horizontal"), 0f);
            }
            else
            {


                animUp.SetBool("Walking", false);
                animDown.SetBool("Walking", false);
                animRight.SetBool("Walking", false);
                animLeft.SetBool("Walking", false);
                
                return new Vector2(0f, 0f);

            }
        }

        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        return rb.velocity;
    }
    private void GetInput()
    {
        /*
		if(Input.GetKeyDown(KeyCode.Escape))
        {
			SceneLoader.GoToDebug();
        }

		if (Input.GetKeyDown(KeyCode.Backspace))
		{
           SceneLoader.GoToPause();
		}
        //Movement Input
        if (Vector3.Distance(transform.position, movePoint.position) <= 0.02f)
		{
			if(Mathf.Abs(Input.GetAxisRaw("Vertical")) == 1f)
			{
				//Movement animation here (this handles both up and down, you'll need to check which is happening)
				animDown.SetBool("Walking", true);
				animUp.SetBool("Walking", true);
				//if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(0f, Input.GetAxisRaw("Vertical"), 1f), .2f, collideables))
				{
					//movePoint.position += new Vector3(0f, , 0f);
                    rb.velocity = new Vector2(0f, Input.GetAxisRaw("Vertical"));
                    //Up and Down Animations
                    if (Input.GetKey(KeyCode.W) && !attacking) 
					{
						spriteUp.SetActive(true);
						spriteDown.SetActive(false);
						spriteRight.SetActive(false);
						spriteLeft.SetActive(false);
					}
					else if (Input.GetKey(KeyCode.S) && !attacking)
					{
						spriteUp.SetActive(false);
						spriteDown.SetActive(true);
						spriteRight.SetActive(false);
						spriteLeft.SetActive(false);
					}
				}
			}
			else if(Mathf.Abs(Input.GetAxisRaw("Horizontal")) == 1f)
			{
				animRight.SetBool("Walking", true);
				animLeft.SetBool("Walking", true);
				//if(!Physics2D.OverlapCircle(movePoint.position + new Vector3(Input.GetAxisRaw("Horizontal"), 0f, 1f), .2f, collideables))
				{
					//Movement animation here (this handles both left and right, you'll need to check which is happening)
					//movePoint.position += new Vector3(, 0f, 0f);
                    rb.velocity = new Vector2(Input.GetAxisRaw("Horizontal"), 0f);

                    //Left and Right Animations
                    if (Input.GetKey(KeyCode.A) && !attacking)
					{
						spriteUp.SetActive(false);
						spriteDown.SetActive(false);
						spriteRight.SetActive(false);
						spriteLeft.SetActive(true);
					}
					else if (Input.GetKey(KeyCode.D) && !attacking)
					{
						spriteUp.SetActive(false);
						spriteDown.SetActive(false);
						spriteRight.SetActive(true);
						spriteLeft.SetActive(false);
					}
				}
			}
			//added else statement here so when player is not moving, idle animation will play (walking animation will not play).
			else
			{
                rb.velocity = new Vector2(0f, 0f);

                animUp.SetBool("Walking", false);
				animDown.SetBool("Walking", false);
				animRight.SetBool("Walking", false);
				animLeft.SetBool("Walking", false);
			}

		}*/


    }
}
