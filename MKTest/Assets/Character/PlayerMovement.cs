using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public CharacterController2D controller;

    public float moveSpeed = 40f;

    float moveX = 0f;

    bool jumping = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        // Calculate the speed of the player movement
        moveX = Input.GetAxisRaw("Horizontal") * moveSpeed;

        // Check if the player is trying to jump
        if (Input.GetButtonDown("Jump"))
        {
            jumping = true;
        }

    }

    private void FixedUpdate()
    {

        // Moving the character
        controller.Move(moveX * Time.fixedDeltaTime, false, jumping);
        jumping = false;
    }
}
