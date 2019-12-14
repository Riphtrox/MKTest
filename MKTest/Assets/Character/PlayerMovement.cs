using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;
    public Animator animator;

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

        // Calculate the speed of the player movement
        moveX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        // Set the animator to play the right animation
        animator.SetFloat("speed", Mathf.Abs(moveX));

        // Check if the player is trying to jump
        if (Input.GetButtonDown("Jump"))
        {
            jump = true;
            animator.SetBool("isJumping", true);
        }

    }

    public void OnLanding()
    {
        animator.SetBool("isJumping", false);
    }

    private void FixedUpdate()
    {

        // Moving the character
        controller.Move(moveX * Time.fixedDeltaTime, false, jump);
        jump = false;

    }
}
