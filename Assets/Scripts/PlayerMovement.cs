using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpSpeed = 1f;

    private Vector2 moveInput;

    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;

    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
    }

    private void Update()
    {
        Run();
        FlipSprite();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        if (value.isPressed)
        {
            myRigidbody2D.linearVelocity += new Vector2 (0f, jumpSpeed);
        }
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * moveSpeed, myRigidbody2D.linearVelocityY);
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody2D.linearVelocityX) > Mathf.Epsilon;

        myRigidbody2D.linearVelocity = playerVelocity;
        if (hasHorizontalSpeed)
        {
            myAnimator.SetBool("isRunning", true);
        }
        else
        {
            myAnimator.SetBool("isRunning", false);
        }
    }

    private void FlipSprite()
    {
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody2D.linearVelocityX) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody2D.linearVelocityX), 1f);
        }
    }
}
