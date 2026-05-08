using UnityEngine;

/// <summary> Unity-компонент турели / Turret MonoBehaviour adapter </summary>
public class TurretView : MonoBehaviour
{
    [SerializeField] private TurretData turretData;
    [SerializeField] private Transform firePoint;
    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private ObjectPool projectilePool;

    private Turret turret;
    private TurretShooting shooting;
    private bool isAutomatic;
    private bool isPlayerNearby;

    public Turret Turret => turret;

    private void Awake()
    {
        if (enemyManager == null)
            enemyManager = FindAnyObjectByType<EnemyManager>();
        if (projectilePool == null)
            projectilePool = FindAnyObjectByType<ObjectPool>();

        if (enemyManager == null)
            Debug.LogError("EnemyManager not found in scene!", this);
        if (projectilePool == null)
            Debug.LogError("ObjectPool not found in scene!", this);

        turret = new Turret(turretData);
        turret.OnKillReward += (amount) => Debug.Log($"[Turret] +{amount} gold");

        shooting = new TurretShooting(turret, enemyManager, projectilePool);
    }

    private void Update()
    {
        shooting.Update(Time.deltaTime, firePoint.position, isAutomatic || isPlayerNearby);
    }

    public void SetModes(bool automatic, bool playerNear)
    {
        isAutomatic = automatic;
        isPlayerNearby = playerNear;
    }

    public void SetPathRoot(Transform root) { }
}