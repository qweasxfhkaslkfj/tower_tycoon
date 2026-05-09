using UnityEditor;
using UnityEngine;

public class BuildTowerZone : MonoBehaviour, IInteractableObject
{
    // Serialize Fields
    [SerializeField] private GameObject towerPrefab;
    [SerializeField] private int towerCost = 50;
    [SerializeField] private Transform spawnPoint;

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
            Vector3 spawnPos;
            if (spawnPoint != null)
                spawnPos = spawnPoint.position;
            else
                spawnPos = transform.position;

            Instantiate(towerPrefab, spawnPos, Quaternion.identity);
            Destroy(gameObject);
        }
        else
        {
            Debug.Log("It's not enought money");
        }
    }
}
