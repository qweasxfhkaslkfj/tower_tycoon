using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUpgradeShop : MonoBehaviour
{
    [Header("Upgrade Settings")]
    [SerializeField] private int maxUpgradeLevel = 2;

    [Header("Speed Upgrade")]
    [SerializeField] private int[] speedCosts = new int[] { 100, 200 };
    [SerializeField] private float[] speedBonuses = new float[] { 1.5f, 2f };

    [Header("Discount Upgrade")]
    [SerializeField] private int[] discountCosts = new int[] { 150, 250 };
    [SerializeField] private float[] discountValues = new float[] { 0.5f, 0.8f };

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private Button speedUpgradeButton;
    [SerializeField] private Button discountUpgradeButton;
    [SerializeField] private TextMeshProUGUI speedLevelText;
    [SerializeField] private TextMeshProUGUI discountLevelText;
    [SerializeField] private TextMeshProUGUI speedCostText;
    [SerializeField] private TextMeshProUGUI discountCostText;
    [SerializeField] private TextMeshProUGUI speedBonusText;
    [SerializeField] private TextMeshProUGUI discountBonusText;

    [Header("Messages")]
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float messageDuration = 2f;

    private int currentSpeedLevel = 0;
    private int currentDiscountLevel = 0;
    private float currentDiscount = 0f;

    // Temporary links that are installed when the store is opened
    private PlayerController currentPlayerController;
    private PlayerStats currentPlayerStats;

    void Start()
    {
        if (speedUpgradeButton != null)
            speedUpgradeButton.onClick.AddListener(UpgradeSpeed);

        if (discountUpgradeButton != null)
            discountUpgradeButton.onClick.AddListener(UpgradeDiscount);
    }

    // Called when opening a store - passing the current player
    public void Initialize(PlayerController playerController, PlayerStats playerStats)
    {
        currentPlayerController = playerController;
        currentPlayerStats = playerStats;
        UpdateUI();
    }

    public void UpdateUI()
    {
        if (currentPlayerStats != null && moneyText != null)
            moneyText.text = $"{currentPlayerStats.GetMoney()}";
        else if (moneyText != null)
            moneyText.text = "0";

        // Speed upgrade section
        if (speedLevelText != null)
            speedLevelText.text = $"{currentSpeedLevel}/{maxUpgradeLevel}";

        if (speedCostText != null)
        {
            if (currentSpeedLevel < maxUpgradeLevel)
                speedCostText.text = $"{speedCosts[currentSpeedLevel]}";
            else
                speedCostText.text = "MAX";
        }

        if (speedBonusText != null)
        {
            if (currentSpeedLevel > 0)
                speedBonusText.text = $"x{speedBonuses[currentSpeedLevel - 1]}";
            else
                speedBonusText.text = "x1";
        }

        // Discount upgrade section
        if (discountLevelText != null)
            discountLevelText.text = $"{currentDiscountLevel}/{maxUpgradeLevel}";

        if (discountCostText != null)
        {
            if (currentDiscountLevel < maxUpgradeLevel)
                discountCostText.text = $"{discountCosts[currentDiscountLevel]}";
            else
                discountCostText.text = "MAX";
        }

        if (discountBonusText != null)
        {
            if (currentDiscountLevel > 0)
                discountBonusText.text = $"-{discountValues[currentDiscountLevel - 1] * 100}%";
            else
                discountBonusText.text = "0%";
        }

        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        bool hasValidPlayer = currentPlayerStats != null;

        if (speedUpgradeButton != null)
        {
            bool canUpgrade = hasValidPlayer &&
                             currentSpeedLevel < maxUpgradeLevel &&
                             currentPlayerStats.GetMoney() >= speedCosts[currentSpeedLevel];
            speedUpgradeButton.interactable = canUpgrade;
        }

        if (discountUpgradeButton != null)
        {
            bool canUpgrade = hasValidPlayer &&
                             currentDiscountLevel < maxUpgradeLevel &&
                             currentPlayerStats.GetMoney() >= discountCosts[currentDiscountLevel];
            discountUpgradeButton.interactable = canUpgrade;
        }
    }

    void UpgradeSpeed()
    {
        if (currentSpeedLevel >= maxUpgradeLevel)
        {
            ShowMessage("Speed is already maxed out!", Color.yellow);
            return;
        }

        if (currentPlayerStats == null)
        {
            ShowMessage("Error: No player data!", Color.red);
            return;
        }

        int cost = speedCosts[currentSpeedLevel];

        if (currentPlayerStats.GetMoney() >= cost)
        {
            currentPlayerStats.AddMoney(-cost);
            currentSpeedLevel++;

            if (currentPlayerController != null)
            {
                float newSpeed = 5f * speedBonuses[currentSpeedLevel - 1];
                currentPlayerController.SetSpeed(newSpeed);
                ShowMessage($"Speed increased! New speed: {newSpeed}", Color.green);
            }

            UpdateUI();
        }
        else
        {
            ShowMessage($"Not enough money! Need: {cost}", Color.red);
        }
    }

    void UpgradeDiscount()
    {
        if (currentDiscountLevel >= maxUpgradeLevel)
        {
            ShowMessage("Discount is already maxed out!", Color.yellow);
            return;
        }

        if (currentPlayerStats == null)
        {
            ShowMessage("Error: No player data!", Color.red);
            return;
        }

        int cost = discountCosts[currentDiscountLevel];

        if (currentPlayerStats.GetMoney() >= cost)
        {
            currentPlayerStats.AddMoney(-cost);
            currentDiscountLevel++;
            currentDiscount = discountValues[currentDiscountLevel - 1];

            // Update the discount in WeaponUpgradeManager
            var weaponManager = WeaponUpgradeManager.Instance;
            if (weaponManager != null)
                weaponManager.SetDiscount(currentDiscount);

            ShowMessage($"Weapon upgrade discount: {currentDiscount * 100}%!", Color.green);
            UpdateUI();
        }
        else
        {
            ShowMessage($"Not enough money! Need: {cost}", Color.red);
        }
    }

    void ShowMessage(string message, Color color)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.color = color;
            Invoke(nameof(ClearMessage), messageDuration);
        }
        else
        {
            Debug.Log(message);
        }
    }

    void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }

    public void CloseUpgradeMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
        currentPlayerController = null;
        currentPlayerStats = null;
    }

    public int GetSpeedLevel() => currentSpeedLevel;
    public int GetDiscountLevel() => currentDiscountLevel;
    public float GetCurrentDiscount() => currentDiscount;
}