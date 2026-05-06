using UnityEngine;

public class WeaponModificationShopLogic : MonoBehaviour, IInteractableObject
{
    private PlayerStats playerStats;

    [Header("Modification Settings")]
    [SerializeField] private int explosiveCost = 300;
    [SerializeField] private int freezeCost = 350;

    [Header("UI Settings")]
    [SerializeField] private GameObject modificationUIPanel;

    private bool hasExplosiveMod = false;
    private bool hasFreezeMod = false;

    private const string EXPLOSIVE_MOD_KEY = "HasExplosiveMod";
    private const string FREEZE_MOD_KEY = "HasFreezeMod";

    public bool HasExplosiveMod => hasExplosiveMod;
    public bool HasFreezeMod => hasFreezeMod;
    public UpgradeType upgradeType => UpgradeType.TurretUpgrade;

    // Events for UI updates
    public System.Action OnDataChanged;
    public System.Action<string, Color> OnMessageShow;

    void Start()
    {
        LoadModifications();
    }

    void LoadModifications()
    {
        hasExplosiveMod = PlayerPrefs.GetInt(EXPLOSIVE_MOD_KEY, 0) == 1;
        hasFreezeMod = PlayerPrefs.GetInt(FREEZE_MOD_KEY, 0) == 1;
    }

    void SaveModifications()
    {
        PlayerPrefs.SetInt(EXPLOSIVE_MOD_KEY, hasExplosiveMod ? 1 : 0);
        PlayerPrefs.SetInt(FREEZE_MOD_KEY, hasFreezeMod ? 1 : 0);
        PlayerPrefs.Save();
    }

    public bool CanPurchaseExplosive()
    {
        return !hasExplosiveMod && playerStats != null && playerStats.GetMoney() >= explosiveCost;
    }

    public bool CanPurchaseFreeze()
    {
        return !hasFreezeMod && playerStats != null && playerStats.GetMoney() >= freezeCost;
    }

    public void PurchaseExplosiveMod()
    {
        if (hasExplosiveMod)
        {
            OnMessageShow?.Invoke("Взрывная модификация уже куплена!", Color.yellow);
            return;
        }

        if (playerStats != null && playerStats.GetMoney() >= explosiveCost)
        {
            playerStats.AddMoney(-explosiveCost);
            hasExplosiveMod = true;
            SaveModifications();

            Debug.Log("[WeaponShop] Explosive mod purchased!");
            OnMessageShow?.Invoke("Взрывные снаряды куплены! Теперь ваши снаряды взрываются!", Color.green);
            OnDataChanged?.Invoke();
        }
        else
        {
            OnMessageShow?.Invoke($"Недостаточно денег! Нужно: {explosiveCost}", Color.red);
        }
    }

    public void PurchaseFreezeMod()
    {
        if (hasFreezeMod)
        {
            OnMessageShow?.Invoke("Модификация заморозки уже куплена!", Color.yellow);
            return;
        }

        if (playerStats != null && playerStats.GetMoney() >= freezeCost)
        {
            playerStats.AddMoney(-freezeCost);
            hasFreezeMod = true;
            SaveModifications();

            Debug.Log("[WeaponShop] Freeze mod purchased!");
            OnMessageShow?.Invoke("Заморозка снарядов куплена! Враги будут замораживаться!", Color.green);
            OnDataChanged?.Invoke();
        }
        else
        {
            OnMessageShow?.Invoke($"Недостаточно денег! Нужно: {freezeCost}", Color.red);
        }
    }

    public int GetExplosiveCost() => explosiveCost;
    public int GetFreezeCost() => freezeCost;

    public void Interact(PlayerStats playerStats)
    {
        OpenShop(playerStats);
    }

    public void OpenShop(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
        OnDataChanged?.Invoke();

        if (modificationUIPanel != null)
        {
            WeaponModificationShopUI shopUI = modificationUIPanel.GetComponent<WeaponModificationShopUI>();
            if (shopUI != null)
            {
                modificationUIPanel.SetActive(true);
                shopUI.SetPlayerStats(playerStats);
                Time.timeScale = 0f;
                return;
            }
        }


    }
}


