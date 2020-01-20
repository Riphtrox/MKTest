using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private const float DEATH_ZONE_Y = 0f;      //The y value that is the killzone
    public CharacterController2D controller;    //The character controller
    public Animator animator;                   //The animator in charge of player animations
    public AudioSource coinSound;               //Audio that plays when collecting coins
    public float moveSpeed = 40f;               //The speed at which the player runs

    Transform prevPos;                          //The player's previous position
    float moveX = 0f;                           //The horizontal movement value
    bool hasPrevPos = false;                    //A test for if the players previous position has been stored
    bool jump = false;                          //A test for the player trying to jump
    bool jumpBoost = false;                     //Is the player holding the button
    bool usingFingers = false;                  //As soon as a finger is detected, conclude that touch controls are used
    bool jumpFinished = false;                  //Has the player finished their jump

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {


        //Check if the player is stuck (can't move horizontally)
        bool isStuck = false;
        if(hasPrevPos)
        {
            isStuck = controller.MovementCheck();
        }

        if (isStuck)
        {
            controller.UnStuck();
        }

        //Check if the player is in a death situation
        if(this.GetComponent<Transform>().position.y <= DEATH_ZONE_Y)
        {
            //Open the game over menu
            FindObjectOfType<GameManager>().GameOver();
        }

        //Calculate the speed of the player movement
        moveX = moveSpeed;

        //Set the animator to play the right animation
        animator.SetFloat("speed", Mathf.Abs(moveX));

        //Check for touches on mobile device
        int fingerCount = 0;
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                fingerCount++;
        }

        //Check if the player is using touch control
        if(fingerCount > 0)
        {
            usingFingers = true;
        }

        //Check if the player is trying to jump
        if(Input.GetButtonDown("Jump") || (usingFingers && fingerCount > 0))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }
        else if (Input.GetButtonUp("Jump") || (usingFingers && fingerCount == 0))
        {
            jump = false;                                       //Should be false but just to be sure
            jumpBoost = false;
            jumpFinished = true;
        }

        prevPos = this.GetComponent<Transform>();
        hasPrevPos = true;
    }

    

    private void OnTriggerEnter2D(Collider2D other)
    {
        //Collision event with coin
        if (other.gameObject.CompareTag("Coin"))
        {
            //Disable the collected coin
            other.gameObject.SetActive(false);
            coinSound.Play();
        }
        //Collision event with spike
        else if (other.gameObject.CompareTag("Spike"))
        {
            //Open the game over menu
            FindObjectOfType<GameManager>().GameOver();
        }
        
    }
    

    public void OnLanding()
    {
        //Change to the required animation
        animator.SetBool("isJumping", false);
        jumpFinished = false;
    }

    private void FixedUpdate()
    {

        //Moving the character
        controller.Move(moveX * Time.fixedDeltaTime, jump, jumpBoost, jumpFinished);

        //Checking if the player continues holding the jump button
        if (jump && !jumpFinished)
        {
            jumpBoost = true;
        }
        jump = false;

    }
}
