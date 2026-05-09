using UnityEngine;

/// <summary>
/// Снаряд турели / Turret projectile
/// </summary>
public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private int damage;
    private bool explosive;
    private float splashRadius;
    private int killReward;
    private Turret owner;

    /// <summary> Инициализация параметров / Initialize parameters </summary>
    public void Init(Transform target, float speed, int damage,
                     bool explosive, float splashRadius, int reward, Turret owner)
    {
        this.target = target;
        this.speed = speed;
        this.damage = damage;
        this.explosive = explosive;
        this.splashRadius = splashRadius;
        this.killReward = reward;
        this.owner = owner;
    }

    private void Update()
    {
        if (target == null)
        {
            ReturnToPool();
            return;
        }

        Vector2 direction = (target.position - transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);

        // Достигли цели (малое расстояние) / Reached target (small distance)
        if (((Vector2)(target.position - transform.position)).sqrMagnitude < 0.04f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (explosive && splashRadius > 0f)
        {
            // Взрыв: сбор коллайдеров в радиусе / Explosion: collect colliders in radius
            Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, splashRadius);
            foreach (var hit in hits)
            {
                Enemy enemy = hit.GetComponent<Enemy>();
                if (enemy == null || !enemy.IsAlive) continue;

                bool isPrimary = hit.transform == target;
                int dealt = isPrimary ? damage : Mathf.RoundToInt(damage * 0.5f);
                enemy.TakeDamage(dealt, owner);
            }
        }
        else
        {
            // Одиночное попадание / Single hit
            Enemy enemy = target?.GetComponent<Enemy>();
            if (enemy != null)
                enemy.TakeDamage(damage, owner);
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        ObjectPool.Return(gameObject);
    }
}