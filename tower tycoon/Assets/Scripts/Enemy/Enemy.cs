using UnityEngine;

/// <summary> Чистая модель врага / Pure enemy model </summary>
public class Enemy
{
    public Transform Transform { get; set; }
    public Vector2 Position => Transform.position;
    public bool IsAlive { get; set; } = true;

    public void TakeDamage(int amount, Turret source = null)
    {
        if (!IsAlive) return;
        IsAlive = false;
<<<<<<< HEAD

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
=======
        source?.AddKillReward();
>>>>>>> 8b2f60b585e9a081942ca76fcb2124ee1885b1a2
    }
}