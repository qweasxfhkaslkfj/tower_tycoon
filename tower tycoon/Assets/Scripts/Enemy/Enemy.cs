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
        source?.AddKillReward();
    }
}