using UnityEngine;

/// <summary>
/// Противник: здоровье, получение урона, взаимодействие с менеджером и движением.
/// </summary>
public class Enemy : MonoBehaviour
{
    private EnemyManager manager;
    private Transform pathRoot;

    [Header("Health Settings")]
    [SerializeField] private int maxHealth = 30;
    private int currentHealth;

    [Header("Reward")]
    [SerializeField] private int killReward = 10;

    [Header("Debug")]
    [SerializeField] private bool showDebugInfo = true;

    public bool IsAlive { get; private set; } = true;
    public Vector2 Position => transform.position;

    public void Init(EnemyManager manager, Transform pathRoot)
    {
        this.manager = manager;
        this.pathRoot = pathRoot;
        currentHealth = maxHealth;
        IsAlive = true;

        EnemyMovement movement = GetComponent<EnemyMovement>();
        if (movement != null)
        {
            movement.InitPath(pathRoot);
        }
    }

    public void TakeDamage(int amount, Turret source = null)
    {
        if (!IsAlive) return;

        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die(source);
        }
    }

    private void Die(Turret source)
    {
        IsAlive = false;

        if (source != null)
        {
            source.AddKillReward();
        }
        else
        {
            if (PlayerStats.Instance != null)
                PlayerStats.Instance.AddMoney(killReward);
        }

        manager?.OnEnemyDeath(this, pathRoot);
        Destroy(gameObject);
    }

    public void ReachEnd()
    {
        if (!IsAlive) return;
        IsAlive = false;
        manager?.OnEnemyDeath(this, pathRoot);
        Destroy(gameObject);
    }

    public int GetCurrentHealth() => currentHealth;
    public int GetMaxHealth() => maxHealth;

    void OnGUI()
    {
        if (!showDebugInfo || !IsAlive || !Camera.main) return;

        Vector3 screenPos = Camera.main.WorldToScreenPoint(transform.position);
        GUI.Label(new Rect(screenPos.x - 30, Screen.height - screenPos.y - 20, 60, 20),
                  $"HP: {currentHealth}/{maxHealth}");
    }
}