using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;
    [SerializeField] private float jumpSpeed = 1f;
    [SerializeField] private float climbSpeed = 1f;
    [SerializeField] private Vector2 deathFling = new Vector2 (10f, 20f);
    [SerializeField] private GameObject arrow;
    [SerializeField] private Transform bow;
    
    private Vector2 moveInput;
    private bool isAlive = true;

    private Rigidbody2D myRigidbody2D;
    private Animator myAnimator;
    private CapsuleCollider2D myBodyCollider;
    private BoxCollider2D myFeetCollider;
    private float gravityStart;

    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myAnimator = GetComponent<Animator>();
        myBodyCollider = GetComponent<CapsuleCollider2D>();
        myFeetCollider = GetComponent<BoxCollider2D>();
        gravityStart = myRigidbody2D.gravityScale;
    }

    private void Update()
    {
        if (!isAlive) {return;}

        Run();
        Climb();
        FlipSprite();
        Die();
    }

    private void OnMove(InputValue value)
    {
        if (!isAlive) {return;}

        moveInput = value.Get<Vector2>();
    }

    private void OnJump(InputValue value)
    {
        int groundLayer = LayerMask.GetMask("Ground");

        if (value.isPressed && myFeetCollider.IsTouchingLayers(groundLayer))
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

    private void Climb()
    {   
        Vector2 playerVelocity = new Vector2 (myRigidbody2D.linearVelocityX , moveInput.y * climbSpeed);
        bool hasVerticalSpeed = Mathf.Abs(myRigidbody2D.linearVelocityY) > Mathf.Epsilon;
        int climbingLayer = LayerMask.GetMask("Climbing");

        if (!myBodyCollider.IsTouchingLayers(climbingLayer))
        {
            myRigidbody2D.gravityScale = gravityStart;
            myAnimator.SetBool("isClimbing", false);
            return;
        }
        
        myRigidbody2D.linearVelocity = playerVelocity;
        myRigidbody2D.gravityScale = 0;
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
        bool hasHorizontalSpeed = Mathf.Abs(myRigidbody2D.linearVelocityX) > Mathf.Epsilon;

        if (hasHorizontalSpeed)
        {
            transform.localScale = new Vector2 (Mathf.Sign(myRigidbody2D.linearVelocityX), 1f);
        }
    }

    private void Die()
    {
        if (myBodyCollider.IsTouchingLayers(LayerMask.GetMask("Enemies", "Hazards")))
        {
            isAlive = false;
            myAnimator.SetTrigger("Dying");
            myRigidbody2D.linearVelocity = deathFling;
        }
    }

    private void OnAttack(InputValue value)
    {
        if (!isAlive) {return;}
        Instantiate(arrow, bow.position, transform.rotation);
    }

}
