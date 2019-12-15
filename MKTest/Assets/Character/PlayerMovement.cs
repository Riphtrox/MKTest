using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    private const float DEATH_ZONE_Y = 0f;
    public CharacterController2D controller;
    public Animator animator;
    public AudioSource coinSound;
    public AudioSource jumpSound;
    public AudioSource deathSound;

    public float moveSpeed = 40f;

    float moveX = 0f;

    bool jump = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (this.GetComponent<Transform>().position.y <= DEATH_ZONE_Y)
        {
            deathSound.Play();
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        // Calculate the speed of the player movement
        moveX = moveSpeed;

        // Set the animator to play the right animation
        animator.SetFloat("speed", Mathf.Abs(moveX));

        // Check for touches on mobile device
        int fingerCount = 0;
        foreach(Touch touch in Input.touches)
        {
            if(touch.phase != TouchPhase.Ended && touch.phase != TouchPhase.Canceled)
                fingerCount++;

        }

        // Check if the player is trying to jump
        if(Input.GetButtonDown("Jump") || fingerCount > 0)
        {
            jump = true;
            animator.SetBool("isJumping", true);
            jumpSound.Play();
        }

    }

    
    // Collision event with coin
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            coinSound.Play();
        }
        
    }
    

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    private void FixedUpdate()
    {

        // Moving the character
        controller.Move(moveX * Time.fixedDeltaTime, jump);
        jump = false;

    }
}
