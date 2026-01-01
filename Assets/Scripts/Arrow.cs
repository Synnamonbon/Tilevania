using UnityEngine;

public class Arrow : MonoBehaviour
{
    [SerializeField] private float arrowSpeed = 10f;

    private Rigidbody2D myRigidBody2D;
    private PlayerMovement player;

    private float xSpeed;

    private void Start()
    {
        myRigidBody2D = GetComponent<Rigidbody2D>();
        player = FindFirstObjectByType<PlayerMovement>();
        xSpeed = player.transform.localScale.x * arrowSpeed;
    }

    private void Update()
    {
        myRigidBody2D.linearVelocity = new Vector2 (xSpeed, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
        }
        Destroy(gameObject);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject, 0.25f);
    }
}
