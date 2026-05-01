using UnityEngine;

public class GameInput : MonoBehaviour
{
    // Getter & Setter
    public static GameInput Instance { get; private set; }
    public Vector2 MovementInput { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }


    private void Update()
    {
        float moveX = 0f;
        float moveY = 0f;

        // Left/Right Movement
        if (Input.GetKey(KeyCode.A))
            moveX = -1f;
        if (Input.GetKey(KeyCode.D))
            moveX = 1f;

        // Up/Down Movement
        if (Input.GetKey(KeyCode.W))
            moveY = 1f;
        if (Input.GetKey(KeyCode.S))
            moveY = -1f;

        MovementInput = new Vector2(moveX, moveY).normalized;
    }
}
