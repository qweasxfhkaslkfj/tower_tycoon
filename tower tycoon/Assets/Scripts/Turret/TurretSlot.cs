using UnityEngine;

/// <summary> Слот для установки турели / Turret placement slot </summary>
public class TurretSlot : MonoBehaviour
{
    [SerializeField] private Transform pathRoot;
    [SerializeField] private GameObject turretPrefab;

    private TurretView currentTurret;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentTurret == null)
            Debug.Log($"Player entered slot {name}");
    }

    public void PlaceTurret()
    {
        if (currentTurret != null) return;
        if (turretPrefab == null) return;

        GameObject instance = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        TurretView turretView = instance.GetComponent<TurretView>();
        if (turretView != null)
        {
            turretView.SetPathRoot(pathRoot);
            currentTurret = turretView;
            TurretManager.Instance.RegisterTurret(turretView);
        }
    }
}