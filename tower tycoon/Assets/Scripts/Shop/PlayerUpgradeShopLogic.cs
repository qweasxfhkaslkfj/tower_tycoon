using UnityEngine;

public class PlayerUpgradeShopLogic : MonoBehaviour, IInteractableObject
{
    [Header("Upgrade Settings")]
    [SerializeField] private int maxUpgradeLevel = 2;

    [Header("Speed Upgrade")]
    [SerializeField] private int[] speedCosts = new int[] { 100, 200 };
    [SerializeField] private float[] speedBonuses = new float[] { 1.5f, 2f };

    [Header("Discount Upgrade")]
    [SerializeField] private int[] discountCosts = new int[] { 150, 250 };
    [SerializeField] private float[] discountValues = new float[] { 0.5f, 0.8f };

    [Header("UI Settings")]
    [SerializeField] private GameObject upgradeUIPanel;

    private int currentSpeedLevel = 0;
    private int currentDiscountLevel = 0;
    private float currentDiscount = 0f;

    private PlayerStats playerStats;
    private PlayerController playerController;

    public System.Action OnDataChanged;
    public System.Action<string, Color> OnMessageShow;

    public int CurrentSpeedLevel => currentSpeedLevel;
    public int CurrentDiscountLevel => currentDiscountLevel;
    public float CurrentDiscount => currentDiscount;
    public int MaxUpgradeLevel => maxUpgradeLevel;

    public UpgradeType upgradeType => UpgradeType.PlayerUpgrade;

    void Start()
    {
        if (playerController == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerController = player.GetComponent<PlayerController>();
                if (playerController == null)
                {
                    Debug.LogWarning("PlayerController not found on Player object!");
                }
            }
            else
            {
                Debug.LogWarning("Player object not found in scene!");
            }
        }

        if (upgradeUIPanel == null)
        {
            Debug.LogWarning("Upgrade UI Panel is not assigned in PlayerUpgradeShopLogic! Please assign it in the inspector.");
        }

        Debug.Log("PlayerUpgradeShopLogic initialized successfully");
    }

    public void Initialize(PlayerStats stats, PlayerController controller)
    {
        playerStats = stats;
        if (controller != null)
            playerController = controller;
        OnDataChanged?.Invoke();
    }

    public bool CanUpgradeSpeed()
    {
        return currentSpeedLevel < maxUpgradeLevel &&
               playerStats != null &&
               playerStats.GetMoney() >= speedCosts[currentSpeedLevel];
    }

    public bool CanUpgradeDiscount()
    {
        return currentDiscountLevel < maxUpgradeLevel &&
               playerStats != null &&
               playerStats.GetMoney() >= discountCosts[currentDiscountLevel];
    }

    public int GetSpeedCost()
    {
        return currentSpeedLevel < maxUpgradeLevel ? speedCosts[currentSpeedLevel] : 0;
    }

    public int GetDiscountCost()
    {
        return currentDiscountLevel < maxUpgradeLevel ? discountCosts[currentDiscountLevel] : 0;
    }

    public float GetCurrentSpeedBonus()
    {
        return currentSpeedLevel > 0 ? speedBonuses[currentSpeedLevel - 1] : 1f;
    }

    public float GetCurrentDiscountPercent()
    {
        return currentDiscountLevel > 0 ? discountValues[currentDiscountLevel - 1] * 100 : 0f;
    }

    public void UpgradeSpeed()
    {
        if (currentSpeedLevel >= maxUpgradeLevel)
        {
            OnMessageShow?.Invoke("Speed is already maxed out!", Color.yellow);
            return;
        }

        int cost = speedCosts[currentSpeedLevel];

        // Используем SpendMoney для проверки и списания
        if (playerStats != null && playerStats.SpendMoney(cost))
        {
            currentSpeedLevel++;

            if (playerController != null)
            {
                float newSpeed = 5f * speedBonuses[currentSpeedLevel - 1];
                playerController.speed = newSpeed;
                OnMessageShow?.Invoke($"Speed increased! New speed: {newSpeed}", Color.green);
            }
            else
            {
                OnMessageShow?.Invoke($"Speed increased! (Controller not found)", Color.green);
            }

            OnDataChanged?.Invoke();
        }
        else
        {
            OnMessageShow?.Invoke($"Not enough money! Need: {cost}", Color.red);
        }
    }

    public void UpgradeDiscount()
    {
        if (currentDiscountLevel >= maxUpgradeLevel)
        {
            OnMessageShow?.Invoke("Discount is already maxed out!", Color.yellow);
            return;
        }

        int cost = discountCosts[currentDiscountLevel];

        if (playerStats != null && playerStats.SpendMoney(cost))
        {
            currentDiscountLevel++;
            currentDiscount = discountValues[currentDiscountLevel - 1];

            if (WeaponDiscountManager.Instance != null)
                WeaponDiscountManager.Instance.SetDiscount(currentDiscount);

            OnMessageShow?.Invoke($"Weapon upgrade discount: {currentDiscount * 100}%!", Color.green);
            OnDataChanged?.Invoke();
        }
        else
        {
            OnMessageShow?.Invoke($"Not enough money! Need: {cost}", Color.red);
        }
    }

    public void Interact(PlayerStats playerStats)
    {
        OpenShop(playerStats);
    }

    public void OpenShop(PlayerStats playerStats)
    {
        Initialize(playerStats, playerController);

        if (upgradeUIPanel != null)
        {
            PlayerUpgradeShopUI shopUI = upgradeUIPanel.GetComponent<PlayerUpgradeShopUI>();
            if (shopUI != null)
            {
                upgradeUIPanel.SetActive(true);
                shopUI.SetPlayerStats(playerStats);
                Time.timeScale = 0f;
                return;
            }
        }


    }
}

