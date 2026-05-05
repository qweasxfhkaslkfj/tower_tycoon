using UnityEngine;

public class PlayerController : MonoBehaviour
{
    // Declarating fields
    public float speed = 5f;
    private Rigidbody2D rb;
    private Animator animator;
    private SpriteRenderer sprite;

    // Starting Method
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        if (GameInput.Instance == null)
            Debug.LogError("GameInput.Instance == null");
    }

    // Updating frames
    void Update()
    {
        if (GameInput.Instance == null)
            return;

        Vector2 movement = GameInput.Instance.MovementInput;

        animator.SetFloat("Speed", movement.sqrMagnitude);

        // Sprite rotation
        if (movement.x < 0)
            sprite.flipX = true;
        else if (movement.x > 0)
            sprite.flipX = false;
    }

    // Fixed Update
    private void FixedUpdate()
    {
        if (GameInput.Instance == null)
            return;

        Vector2 movement = GameInput.Instance.MovementInput;
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
