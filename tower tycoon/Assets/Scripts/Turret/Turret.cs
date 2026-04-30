using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Основной компонент турели: стрельба, улучшения / Main turret component: shooting, upgrades
/// </summary>
public class Turret : MonoBehaviour
{
    [SerializeField] private TurretData data;           // Данные турели / Turret data asset
    [SerializeField] private Transform firePoint;       // Точка вылета снарядов / Bullet spawn point
    [SerializeField] private Transform pathRoot;        // Привязка к пути / Path this turret belongs to

    // Улучшаемые параметры / Upgradeable stats
    private int currentDamage;
    private int currentKillReward;
    private int upgradeLevel = 0;

    private const float UPGRADE_MULTIPLIER = 1.1f;       // Множитель улучшения / Upgrade multiplier

    private float attackCooldown;
    private ObjectPool projectilePool;
    private EnemyManager enemyManager;

    // Состояния управления / Control states (устанавливаются TurretManager)
    public bool IsAutomatic { get; set; } = false;
    public bool IsPlayerNearby { get; set; } = false;

    /// <summary> Установить привязку к пути (вызывается слотом) / Set path binding (called by slot) </summary>
    public void SetPathRoot(Transform root)
    {
        pathRoot = root;
    }

    private void Awake()
    {
        if (data == null)
        {
            Debug.LogError($"[Turret] TurretData отсутствует на {gameObject.name}");
            return;
        }

        currentDamage = data.damage;
        currentKillReward = data.baseKillReward;

        if (data.projectilePrefab != null)
            projectilePool = ObjectPool.CreatePool(data.projectilePrefab, 10);

        enemyManager = FindObjectOfType<EnemyManager>();
    }

    private void Update()
    {
        if (!IsAutomatic && !IsPlayerNearby)
            return;

        attackCooldown -= Time.deltaTime;
        if (attackCooldown <= 0f)
        {
            FindAndAttack();
            attackCooldown = 1f / data.attackSpeed;
        }
    }

    /// <summary> Ищет ближайшего врага и атакует / Find closest enemy and attack </summary>
    private void FindAndAttack()
    {
        if (enemyManager == null) return;

        List<Enemy> enemies = enemyManager.GetEnemiesOnPath(pathRoot);
        if (enemies.Count == 0) return;

        Enemy closest = null;
        float minSqrDist = data.attackRange * data.attackRange;
        Vector2 myPos = transform.position;

        foreach (var enemy in enemies)
        {
            if (!enemy.IsAlive) continue;

            float sqrDist = (enemy.Position - myPos).sqrMagnitude;
            if (sqrDist < minSqrDist)
            {
                minSqrDist = sqrDist;
                closest = enemy;
            }
        }

        if (closest != null)
            Attack(closest);
    }

    /// <summary> Выполнение атаки согласно типу / Perform attack based on type </summary>
    private void Attack(Enemy primaryTarget)
    {
        switch (data.attackType)
        {
            case AttackType.Single:
                FireProjectile(primaryTarget.transform);
                break;
            case AttackType.Splash:
                FireProjectile(primaryTarget.transform, explosive: true);
                break;
            case AttackType.Area:
                ApplyAreaDamage();
                break;
        }
    }

    /// <summary> Запуск снаряда / Launch projectile </summary>
    private void FireProjectile(Transform target, bool explosive = false)
    {
        if (projectilePool == null) return;

        GameObject projGO = projectilePool.Get(data.projectilePrefab);
        projGO.transform.position = firePoint.position;
        projGO.transform.rotation = Quaternion.identity;

        var proj = projGO.GetComponent<Projectile>();
        if (proj != null)
        {
            proj.Init(target, data.projectileSpeed, currentDamage,
                     explosive, data.splashRadius, currentKillReward, this);
        }
    }

    /// <summary> Прямой урон всем врагам в радиусе (для огнемётов) / Damage all enemies in range (flame etc.) </summary>
    private void ApplyAreaDamage()
    {
        List<Enemy> enemies = enemyManager.GetEnemiesOnPath(pathRoot);
        float sqrRange = data.attackRange * data.attackRange;
        Vector2 myPos = transform.position;

        foreach (var enemy in enemies)
        {
            if (enemy.IsAlive && (enemy.Position - myPos).sqrMagnitude <= sqrRange)
            {
                enemy.TakeDamage(currentDamage, this);
            }
        }
    }

    /// <summary> Вызывается снарядом при убийстве врага / Called by projectile when enemy is killed </summary>
    public void AddKillReward()
    {
        // TODO: Интеграция с валютой / Currency integration
        Debug.Log($"[Turret] +{currentKillReward} gold");
    }

    // ====== Улучшения / Upgrades ======

    public int UpgradeLevel => upgradeLevel;
    public int CurrentDamage => currentDamage;
    public int CurrentKillReward => currentKillReward;

    /// <summary> Стоимость следующего улучшения / Cost of next upgrade </summary>
    public int GetUpgradeCost()
    {
        return Mathf.RoundToInt(50 * Mathf.Pow(1.1f, upgradeLevel));
    }

    /// <summary> Повысить уровень турели / Upgrade turret level </summary>
    public void Upgrade()
    {
        upgradeLevel++;
        currentDamage = Mathf.RoundToInt(currentDamage * UPGRADE_MULTIPLIER);
        currentKillReward = Mathf.Max(1, Mathf.RoundToInt(currentKillReward * UPGRADE_MULTIPLIER));
    }
}