using UnityEngine;

/// <summary>
/// Слот для установки турели / Turret placement slot.
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
        {
            Debug.Log("Можно строить турель");
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Ушёл из зоны постройки");
        }
    }

    /// <summary>
    /// Установить турель из префаба / Place turret from prefab.
    /// </summary>
    public void PlaceTurret()
    {
        if (currentTurret != null) return;

        if (turretPrefab == null)
        {
            Debug.LogError("[TurretSlot] Префаб турели не назначен");
            return;
        }

        // Проверяем хватает ли денег
        int buildCost = GetBuildCost();
        if (PlayerStats.Instance != null && !PlayerStats.Instance.SpendMoney(buildCost))
        {
            Debug.Log($"[TurretSlot] Не хватает денег для постройки! Нужно: {buildCost}");
            return;
        }

        GameObject instance = Instantiate(turretPrefab, transform.position, Quaternion.identity);
        Turret turret = instance.GetComponent<Turret>();

        if (turret == null)
        {
            Debug.LogError("[TurretSlot] В префабе нет компонента Turret");
            Destroy(instance);
            return;
        }

        turret.SetPathRoot(pathRoot);

        currentTurret = turret;

        if (TurretManager.Instance != null)
            TurretManager.Instance.RegisterTurret(turret, pathRoot);

        Debug.Log($"[TurretSlot] Турель построена за {buildCost} монет!");
    }

    /// <summary>
    /// Получить стоимость постройки турели
    /// </summary>
    public int GetBuildCost()
    {
        int originalCost = 100;
        float discount = WeaponDiscountManager.Instance?.GetCurrentDiscount() ?? 0f;
        return Mathf.RoundToInt(originalCost * (1f - discount));
    }

    /// <summary>
    /// Получить текущую установленную турель
    /// </summary>
    public Turret GetCurrentTurret()
    {
        return currentTurret;
    }

    /// <summary>
    /// Проверить, установлена ли турель
    /// </summary>
    public bool HasTurret()
    {
        return currentTurret != null;
    }

    /// <summary>
    /// Получить стоимость улучшения со скидкой
    /// </summary>
    public int GetUpgradeCostWithDiscount()
    {
        if (currentTurret == null) return 0;

        int originalCost = currentTurret.GetUpgradeCost();
        float discount = WeaponDiscountManager.Instance?.GetCurrentDiscount() ?? 0f;
        return Mathf.RoundToInt(originalCost * (1f - discount));
    }

    /// <summary>
    /// Улучшить турель в слоте
    /// </summary>
    public bool UpgradeTurret()
    {
        if (currentTurret == null)
        {
            Debug.LogWarning("[TurretSlot] Нет турели для улучшения");
            return false;
        }

        int originalCost = currentTurret.GetUpgradeCost();
        float discount = WeaponDiscountManager.Instance?.GetCurrentDiscount() ?? 0f;
        int cost = Mathf.RoundToInt(originalCost * (1f - discount));

        if (PlayerStats.Instance != null && PlayerStats.Instance.SpendMoney(cost))
        {
            currentTurret.Upgrade();
            Debug.Log($"[TurretSlot] Турель улучшена! Стоимость: {cost} (было {originalCost})");
            return true;
        }
        else
        {
            Debug.Log($"[TurretSlot] Не хватает денег для улучшения! Нужно: {cost}");
            return false;
        }
    }
}