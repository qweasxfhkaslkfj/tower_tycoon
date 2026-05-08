using UnityEngine;

[CreateAssetMenu(fileName = "NewTurret", menuName = "TD/Turret Data")]
public class TurretData : ScriptableObject
{
    [Header("Атака / Attack")]
    public float attackSpeed = 2f;          // ← 2 выстрела в секунду
    public float attackRange = 10f;         // ← радиус 10
    public int damage = 30;                 // ← урон 30
    public int baseKillReward = 10;         // ← награда 10 монет

    [Header("Тип атаки / Attack Type")]
    public AttackType attackType;
    public float splashRadius = 0f;
    public AreaMode areaMode;

    [Header("Снаряд / Projectile")]
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;

    [Header("Отображение / UI")]
    public Sprite icon;
    public string turretName;
}

public enum AttackType
{
    Single,
    Splash,
    Area
}

public enum AreaMode
{
    DamageAllInRange,
    ProjectileExplosion
}