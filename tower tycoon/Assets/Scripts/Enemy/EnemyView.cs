using UnityEngine;

/// <summary> Unity-адаптер врага / Enemy MonoBehaviour adapter </summary>
public class EnemyView : MonoBehaviour
{
    private Enemy enemy;
    private EnemyMovement movement;
    private Transform pathRoot;          // <-- СОХРАНЯЕМ путь для уведомления менеджера

    public Enemy Enemy => enemy;

    /// <summary> Инициализация после спавна / Initialize after spawn </summary>
    public void Init(EnemyManager manager, Transform pathRoot)
    {
        this.pathRoot = pathRoot;        // <-- СОХРАНЯЕМ
        enemy = new Enemy();
        enemy.Transform = transform;
        movement = GetComponent<EnemyMovement>();
        if (movement != null)
            movement.InitPath(pathRoot, enemy);
    }

    /// <summary> Вызывается из EnemyMovement при достижении конца пути / Called when reaching end </summary>
    public void ReachEnd()
    {
        if (!enemy.IsAlive) return;
        enemy.IsAlive = false;
        // Передаём СОХРАНЁННЫЙ путь
        EnemyManager.Instance?.OnEnemyDeath(this, pathRoot);
        Destroy(gameObject);
    }

    /// <summary> Получение урона (может вызываться снарядом) / Take damage from projectile </summary>
    public void TakeDamage(int amount, Turret source = null)
    {
        if (!enemy.IsAlive) return;
        enemy.TakeDamage(amount, source);
        if (!enemy.IsAlive)
        {
            // Передаём СОХРАНЁННЫЙ путь при смерти
            EnemyManager.Instance?.OnEnemyDeath(this, pathRoot);
            Destroy(gameObject);
        }
    }
}