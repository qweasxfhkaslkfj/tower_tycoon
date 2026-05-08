using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Transform target;
    private float speed;
    private int damage;
    private bool explosive;
    private float splashRadius;
    private int killReward;
    private Turret owner;

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

        if (((Vector2)(target.position - transform.position)).sqrMagnitude < 0.1f)
        {
            HitTarget();
        }
    }

    private void HitTarget()
    {
        if (explosive && splashRadius > 0f)
        {
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
            Enemy enemy = target?.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage, owner);
            }
        }

        ReturnToPool();
    }

    private void ReturnToPool()
    {
        gameObject.SetActive(false);
        ObjectPool.Return(gameObject);
    }
}