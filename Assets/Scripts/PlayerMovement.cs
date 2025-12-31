using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float climbSpeed = 1f;

    private Vector2 moveInput;

    private Rigidbody2D myRigidbody;
    private Animator myAnimator;
    private CapsuleCollider2D myBodyCollider;
    private BoxCollider2D myFeetCollider;
    private float gravityStart;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityStart = myRigidbody.gravityScale;
    }

    private void Update()
    {
        Run();
        Climb();
        FlipSprite();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        int groundLayer = LayerMask.GetMask("Ground");

        if (value.isPressed && myFeetCollider.IsTouchingLayers(groundLayer))
        {
            myRigidbody.linearVelocity += new Vector2 (0f, jumpSpeed);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * moveSpeed, myRigidbody.linearVelocityY);
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocityX) > Mathf.Epsilon;

        myRigidbody.linearVelocity = playerVelocity;
        if (hasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    private void Climb()
    {   
        Vector2 playerVelocity = new Vector2 (myRigidbody.linearVelocityX , moveInput.y * climbSpeed);
        bool hasVerticalSpeed = Mathf.Abs(myRigidbody.linearVelocityY) > Mathf.Epsilon;
        int climbingLayer = LayerMask.GetMask("Climbing");

        if (!myBodyCollider.IsTouchingLayers(climbingLayer))
        {
            myRigidbody.gravityScale = gravityStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        
        myRigidbody.linearVelocity = playerVelocity;
        myRigidbody.gravityScale = 0;
        if (hasVerticalSpeed)
        {
            myAnimator.SetBool("isClimbing", true);
        }
        else
        {
            myAnimator.SetBool("isClimbing", false);
        }
    }

    private void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody.linearVelocityX) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody.linearVelocityX), 1f);
        }
    }
}
