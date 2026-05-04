using UnityEngine;

/// <summary>
/// Данные типа турели (статические характеристики) / Turret type data (static stats)
/// </summary>
[CreateAssetMenu(fileName = "NewTurret", menuName = "TD/Turret Data")]
public class TurretData : ScriptableObject
{
    [Header("Атака / Attack")]
    public float attackSpeed = 1f;          // Выстрелов в секунду / Shots per second
    public float attackRange = 5f;          // Радиус атаки / Attack radius
    public int damage = 10;                 // Базовый урон / Base damage
    public int baseKillReward = 1;          // Базовая награда за убийство / Base kill reward

    [Header("Тип атаки / Attack Type")]
    public AttackType attackType;           // Single, Splash, Area
    public float splashRadius = 0f;         // Радиус поражения (для Splash/Area) / Splash radius
    public AreaMode areaMode;               // Способ нанесения area‑урона / How area damage is applied

    [Header("Снаряд / Projectile")]
    public GameObject projectilePrefab;     // Префаб снаряда (null для Area без снаряда) / Projectile prefab
    public float projectileSpeed = 10f;     // Скорость полёта / Flight speed

    [Header("Отображение / UI")]
    public Sprite icon;                     // Иконка для интерфейса / UI icon
    public string turretName;               // Название / Display name
}

/// <summary> Тип атаки турели / Turret attack type </summary>
public enum AttackType
{
    Single,     // Одна цель / Single target
    Splash,     // Основная цель + урон рядом / Splash damage
    Area        // По области (огнемёт/миномёт) / Area of effect
}

/// <summary> Режим area‑атаки / Area damage mode </summary>
public enum AreaMode
{
    DamageAllInRange,   // Каждую атаку – всем врагам в радиусе / Damage all enemies in range
    ProjectileExplosion // Снаряд взрывается при контакте / Projectile explosion on hit
}