using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float runSpeed = 8f;
    [SerializeField] float jumpSpeed = 20f;
    [SerializeField] float climbSpeed = 5f;

    Vector2 moveInput;

    Rigidbody2D playerRigidbody2D;
    Animator animator;

    CapsuleCollider2D capsuleCollider2D;

    void Start()
    {
        playerRigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
    }

    void Update()
    {
        Run();
        ClimbLadder();
        FlipSprite();
    }

    void FlipSprite()
    {
        // mathf epsilon means 0
        bool playerIsMoving = Mathf.Abs(playerRigidbody2D.velocity.x) > Mathf.Epsilon;

        if (playerIsMoving)
        {
            transform.localScale = new Vector2(Mathf.Sign(playerRigidbody2D.velocity.x), 1f);
        }

    }

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * runSpeed, playerRigidbody2D.velocity.y);
        playerRigidbody2D.velocity = playerVelocity;

        bool playerIsMoving = Mathf.Abs(playerRigidbody2D.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", playerIsMoving);
    }

    void ClimbLadder()
    {
        // check if player is touching climbing layer
        bool isTouchingLadder = capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Climbing"));

        if (isTouchingLadder)
        {
            Vector2 climbVelocity = new Vector2(playerRigidbody2D.velocity.x, moveInput.y * climbSpeed);
            playerRigidbody2D.velocity = climbVelocity;
        }
    }

    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        // check that player is touching ground layer before jump is possible
        bool isGrounded = capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground"));

        if (isGrounded && value.isPressed)
        {
            // increase y velocity of player
            playerRigidbody2D.velocity += new Vector2(0, jumpSpeed);
        }
    }
}
