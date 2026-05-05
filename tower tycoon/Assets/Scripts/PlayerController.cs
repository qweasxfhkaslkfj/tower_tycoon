using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer sprite;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        if (animator != null)
            animator.SetFloat("Speed", movement.sqrMagnitude);

        if (movement.x < 0)
        {
            sprite.flipX = true;
        }
        else if (movement.x > 0)
        {
            sprite.flipX = false;
        }
    }

    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }

    //Added method for changing speed from the store
    public void SetSpeed(float newSpeed)
    {
        speed = newSpeed;
        Debug.Log($"Player speed changed to: {speed}");
    }

    //Added method for getting current speed
    public float GetSpeed() => speed;
}