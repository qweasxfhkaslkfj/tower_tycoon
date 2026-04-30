using System.Runtime.CompilerServices;
using UnityEngine;

public class NewMonoBehaviourScript : MonoBehaviour
{
    // Объявление полей
    public float speed = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Animator animator;
    private SpriteRenderer sprite;

    // Стартовый метод
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();
    }

    // Апдейт фреймов
    void Update()
    {
        // ToDo: GameInput вместо GetAxisRaw
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

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

    // Фиксированный апдейт
    private void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);
    }
}
