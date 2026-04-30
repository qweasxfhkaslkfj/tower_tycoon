using UnityEngine;

/// <summary>
/// Слот для установки турели / Turret placement slot.
/// Использует готовый префаб турели.
/// </summary>
public class TurretSlot : MonoBehaviour
{
    [Header("Привязка к пути / Path binding")]
    [SerializeField] private Transform pathRoot;

    [Header("Префаб турели / Turret prefab")]
    [SerializeField] private GameObject turretPrefab;

    [Header("Опциональные данные (для UI) / Optional data")]
    [SerializeField] private TurretData turretData;

    private Turret currentTurret;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && currentTurret == null)
            UIManager.Instance.ShowBuildButton(this);
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
            UIManager.Instance.HideBuildButton();
    }

    /// <summary>
    /// Установить турель из префаба / Place turret from prefab.
    /// Вызывается из UI.
    /// </summary>
    public void PlaceTurret()
    {
        if (currentTurret != null) return;

        if (turretPrefab == null)
        {
            Debug.LogError("[TurretSlot] Префаб турели не назначен / Turret prefab missing");
            return;
        }

        GameObject instance = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        Turret turret = instance.GetComponent<Turret>();

        if (turret == null)
        {
            Debug.LogError("[TurretSlot] В префабе нет компонента Turret / Turret component missing");
            Destroy(instance);
            return;
        }

        turret.SetPathRoot(pathRoot);

        currentTurret = turret;
        TurretManager.Instance.RegisterTurret(turret, pathRoot);
    }
}