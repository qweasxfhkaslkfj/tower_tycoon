using UnityEngine;

/// <summary> Полёт и попадание снаряда / Projectile flight and hit </summary>
public class ProjectileView : MonoBehaviour
{
    private Transform target;
    private float speed;
    private System.Action onReachedTarget;

    /// <summary> Запустить снаряд / Launch projectile </summary>
    public void Launch(Enemy targetEnemy, float speed, int damage, int reward, Turret ownerTurret)
    {
        target = targetEnemy?.Transform;
        this.speed = speed;

        onReachedTarget = () =>
        {
            if (targetEnemy != null && targetEnemy.IsAlive)
                targetEnemy.TakeDamage(damage, ownerTurret);
            gameObject.SetActive(false);
            ObjectPool.Return(gameObject);
        };
    }

    private void Update()
    {
        if (target == null)
        {
            onReachedTarget?.Invoke();
            return;
        }

        Vector2 dir = ((Vector2)target.position - (Vector2)transform.position).normalized;
        transform.Translate(dir * speed * Time.deltaTime, Space.World);

        if (Vector2.Distance(transform.position, target.position) < 0.2f)
            onReachedTarget?.Invoke();
    }
}