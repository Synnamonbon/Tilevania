using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float moveSpeed = 1f;

    private Vector2 moveInput;
    Rigidbody2D myRigidbody2D;

    private void Start()
    {
        myRigidbody2D = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        Run();
    }

    private void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }

    private void Run()
    {
        Vector2 playerVelocity = new Vector2 (moveInput.x * moveSpeed, myRigidbody2D.linearVelocityY);
        myRigidbody2D.linearVelocity = playerVelocity;
    }
}
