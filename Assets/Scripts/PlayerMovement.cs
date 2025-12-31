using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float climbSpeed = 1f;

    private Vector2 moveInput;

    private Rigidbody2D rigidbody;
    private Animator animator;
    private CapsuleCollider2D bodyCollider;
    private BoxCollider2D feetCollider;
    private float gravityStart;

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        bodyCollider = GetComponent<CapsuleCollider2D>();
        feetCollider = GetComponent<BoxCollider2D>();
        gravityStart = rigidbody.gravityScale;
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

        if (value.isPressed && feetCollider.IsTouchingLayers(groundLayer))
        {
            rigidbody.linearVelocity += new Vector2 (0f, jumpSpeed);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * moveSpeed, rigidbody.linearVelocityY);
        bool hasHorizontalSpeed = Mathf.Abs(rigidbody.linearVelocityX) > Mathf.Epsilon;

        rigidbody.linearVelocity = playerVelocity;
        if (hasHorizontalSpeed)
        {
            animator.SetBool("isRunning", true);
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }

    private void Climb()
    {   
        Vector2 playerVelocity = new Vector2 (rigidbody.linearVelocityX , moveInput.y * climbSpeed);
        bool hasVerticalSpeed = Mathf.Abs(rigidbody.linearVelocityY) > Mathf.Epsilon;
        int climbingLayer = LayerMask.GetMask("Climbing");

        if (!bodyCollider.IsTouchingLayers(climbingLayer))
        {
            rigidbody.gravityScale = gravityStart;
            animator.SetBool("isClimbing", false);
            return;
        }
        
        rigidbody.linearVelocity = playerVelocity;
        rigidbody.gravityScale = 0;
        if (hasVerticalSpeed)
        {
            animator.SetBool("isClimbing", true);
        }
        else
        {
            animator.SetBool("isClimbing", false);
        }
    }

    private void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(rigidbody.linearVelocityX) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(rigidbody.linearVelocityX), 1f);
        }
    }
}
