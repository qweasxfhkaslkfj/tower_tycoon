using UnityEditor;
using UnityEngine;

public class BuildTowerZone : MonoBehaviour, IInteractableObject
{
    // Serialize Fields
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private int towerCost = 50;
    [SerializeField] private Transform spawnPoint;

    [SerializeField] private EnemyManager enemyManager;
    [SerializeField] private Transform pathRoot;
    [SerializeField] private Transform enemySpawnPoint;
    [SerializeField] private int maxEnemiesOnPath = 3;

    public UpgradeType upgradeType
    {
        get { return UpgradeType.PlayerUpgrade; }
    }

    public void Interact(PlayerStats playerStats)
    {
        if (playerStats == null)
        {
            Debug.Log("playerStats == null");
            return;
        }

        if (playerStats.SpendMoney(towerCost))
        {
            // Creating tower
            Vector3 spawnPos;
            if (spawnPoint != null)
                spawnPos = spawnPoint.position;
            else
                spawnPos = transform.position;
            Instantiate(towerPrefab, spawnPos, Quaternion.identity);

            // New enemy path
            if (enemyManager != null && pathRoot != null && enemySpawnPoint != null)
                enemyManager.RegisterPath(pathRoot, enemySpawnPoint, maxEnemiesOnPath);
            else
                Debug.LogWarning("Не все поля для спавна назначены");

            Destroy(gameObject);
        }
        else
        {
            Debug.Log("It's not enought money");
        }
    }
}
