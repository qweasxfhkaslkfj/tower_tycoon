using System.Collections.Generic;
using UnityEngine;

/// <summary> Логика стрельбы турели, независимая от Unity / Turret shooting logic </summary>
public class TurretShooting
{
    private readonly Turret turret;
    private readonly IEnemyProvider enemyProvider;
    private readonly IProjectileFactory projectileFactory;
    private float cooldownTimer;

    public TurretShooting(Turret turret, IEnemyProvider enemyProvider, IProjectileFactory projectileFactory)
    {
        this.turret = turret;
        this.enemyProvider = enemyProvider;
        this.projectileFactory = projectileFactory;
    }

    public void Update(float deltaTime, Vector2 firePosition, bool canShoot)
    {
        if (!canShoot) return;

        cooldownTimer -= deltaTime;
        if (cooldownTimer > 0f) return;

        cooldownTimer = 1f / turret.Data.attackSpeed;
        Enemy target = FindClosestEnemy(firePosition);
        if (target != null)
            Fire(target, firePosition);
    }

    private Enemy FindClosestEnemy(Vector2 position)
    {
        List<Enemy> enemies = enemyProvider.GetEnemiesInRange(position, turret.Data.attackRange);
        Enemy closest = null;
        float minSqrDist = float.MaxValue;
        foreach (var e in enemies)
        {
            float sqrDist = (e.Position - position).sqrMagnitude;
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                closest = e;
            }
        }
        return closest;
    }

    private void Fire(Enemy target, Vector2 spawnPos)
    {
        ProjectileView proj = projectileFactory.Create();
        proj.transform.position = spawnPos;
        proj.Launch(target, turret.Data.projectileSpeed, turret.Damage, turret.KillReward, turret);
    }
}