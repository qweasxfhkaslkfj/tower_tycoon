using UnityEngine;

[CreateAssetMenu(fileName = "NewTurret", menuName = "TD/Turret Data")]
public class TurretData : ScriptableObject
{
    public float attackSpeed = 1f;
    public float attackRange = 5f;
    public int damage = 10;
    public int baseKillReward = 1;
    public AttackType attackType;
    public float splashRadius = 0f;
    public AreaMode areaMode;
    public GameObject projectilePrefab;
    public float projectileSpeed = 10f;
    public Sprite icon;
    public string turretName;
}

public enum AttackType { Single, Splash, Area }
public enum AreaMode { DamageAllInRange, ProjectileExplosion }