using UnityEngine;
using UnityEngine.UI;
using TMPro; // TextMeshPro for high-quality UI text

public class PlayerUpgradeShop : MonoBehaviour
{
    [Header("Upgrade Settings")]
    [SerializeField] private int maxUpgradeLevel = 2; // Maximum upgrade level for any stat (2 upgrades max)

    [Header("Speed Upgrade")]
    [SerializeField] private int[] speedCosts = new int[] { 100, 200 }; // Cost for level 1 and level 2 speed upgrades
    [SerializeField] private float[] speedBonuses = new float[] { 1.5f, 2f }; // Speed multipliers: x1.5 then x2.0

    [Header("Discount Upgrade")]
    [SerializeField] private int[] discountCosts = new int[] { 150, 250 }; // Cost for level 1 and level 2 discount upgrades
    [SerializeField] private float[] discountValues = new float[] { 0.5f, 0.8f }; // Discount values: 50% then 80% off

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText; // UI text displaying current money
    [SerializeField] private Button speedUpgradeButton; // Button to upgrade speed
    [SerializeField] private Button discountUpgradeButton; // Button to upgrade discount
    [SerializeField] private TextMeshProUGUI speedLevelText; // Text showing current speed level (0/2, 1/2, 2/2)
    [SerializeField] private TextMeshProUGUI discountLevelText; // Text showing current discount level
    [SerializeField] private TextMeshProUGUI speedCostText; // Text showing cost of next speed upgrade
    [SerializeField] private TextMeshProUGUI discountCostText; // Text showing cost of next discount upgrade
    [SerializeField] private TextMeshProUGUI speedBonusText; // Text showing current speed multiplier
    [SerializeField] private TextMeshProUGUI discountBonusText; // Text showing current discount percentage

    [Header("Messages")]
    [SerializeField] private TextMeshProUGUI messageText; // UI text for feedback messages
    [SerializeField] private float messageDuration = 2f; // How long messages stay on screen

    // Current upgrade levels (0 = not upgraded, 1 = first upgrade, 2 = max)
    private int currentSpeedLevel = 0;
    private int currentDiscountLevel = 0;

    // References to player components
    private PlayerController playerController; // Reference to player movement script
    private PlayerStats playerStats; // Reference to player stats (money)
    private float currentDiscount = 0f; // Local storage of current discount value

    void Start()
    {
        if (playerStats == null)
            Debug.LogError("playerStats == null");
        if (playerController == null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null) playerController = player.GetComponent<PlayerController>();
        }

        speedUpgradeButton?.onClick.AddListener(UpgradeSpeed);
        discountUpgradeButton?.onClick.AddListener(UpgradeDiscount);
        UpdateUI();
    }

    // Updates all UI elements with current stats and money
    public void UpdateUI()
    {
        // Update money display
        if (moneyText != null && playerStats != null)
            moneyText.text = $"{playerStats.GetMoney()}";

        // Update speed upgrade section
        if (speedLevelText != null)
            speedLevelText.text = $"{currentSpeedLevel}/{maxUpgradeLevel}";

        if (speedCostText != null)
        {
            if (currentSpeedLevel < maxUpgradeLevel)
                speedCostText.text = $"{speedCosts[currentSpeedLevel]}"; // Show next upgrade cost
            else
                speedCostText.text = "MAX"; // Show MAX when fully upgraded
        }

        if (speedBonusText != null)
        {
            if (currentSpeedLevel > 0)
                speedBonusText.text = $"x{speedBonuses[currentSpeedLevel - 1]}"; // Show current multiplier
            else
                speedBonusText.text = "x1"; // No upgrade yet
        }

        // Update discount upgrade section
        if (discountLevelText != null)
            discountLevelText.text = $"{currentDiscountLevel}/{maxUpgradeLevel}";

        if (discountCostText != null)
        {
            if (currentDiscountLevel < maxUpgradeLevel)
                discountCostText.text = $"{discountCosts[currentDiscountLevel]}"; // Show next upgrade cost
            else
                discountCostText.text = "MAX"; // Show MAX when fully upgraded
        }

        if (discountBonusText != null)
        {
            if (currentDiscountLevel > 0)
                discountBonusText.text = $"-{discountValues[currentDiscountLevel - 1] * 100}%"; // Show discount percentage
            else
                discountBonusText.text = "0%"; // No discount yet
        }

        UpdateButtonStates(); // Enable/disable buttons based on affordability
    }

    // Enables or disables upgrade buttons based on player's money
    void UpdateButtonStates()
    {
        // Speed button: enabled if not max level AND player can afford it
        if (speedUpgradeButton != null)
        {
            bool canUpgrade = currentSpeedLevel < maxUpgradeLevel &&
                             playerStats != null &&
                             playerStats.GetMoney() >= speedCosts[currentSpeedLevel];
            speedUpgradeButton.interactable = canUpgrade;
        }

        // Discount button: enabled if not max level AND player can afford it
        if (discountUpgradeButton != null)
        {
            bool canUpgrade = currentDiscountLevel < maxUpgradeLevel &&
                             playerStats != null &&
                             playerStats.GetMoney() >= discountCosts[currentDiscountLevel];
            discountUpgradeButton.interactable = canUpgrade;
        }
    }

    // Handles speed upgrade purchase
    void UpgradeSpeed()
    {
        // Check if already at maximum level
        if (currentSpeedLevel >= maxUpgradeLevel)
        {
            ShowMessage("Speed is already maxed out!", Color.yellow);
            return;
        }

        int cost = speedCosts[currentSpeedLevel];

        // Check if player has enough money
        if (playerStats != null && playerStats.GetMoney() >= cost)
        {
            playerStats.AddMoney(-cost); // Deduct money
            currentSpeedLevel++; // Increase upgrade level

            // Apply speed boost to player movement
            if (playerController != null)
            {
                float newSpeed = 5f * speedBonuses[currentSpeedLevel - 1]; // Calculate new speed (base 5 * multiplier)
                playerController.speed = newSpeed; // Update player's speed
                ShowMessage($"Speed increased! New speed: {newSpeed}", Color.green);
            }

            UpdateUI(); // Refresh UI
        }
        else
        {
            ShowMessage($"Not enough money! Need: {cost}", Color.red);
        }
    }

    // Handles discount upgrade purchase
    void UpgradeDiscount()
    {
        // Check if already at maximum level
        if (currentDiscountLevel >= maxUpgradeLevel)
        {
            ShowMessage("Discount is already maxed out!", Color.yellow);
            return;
        }

        int cost = discountCosts[currentDiscountLevel];

        // Check if player has enough money
        if (playerStats != null && playerStats.GetMoney() >= cost)
        {
            playerStats.AddMoney(-cost); // Deduct money
            currentDiscountLevel++; // Increase upgrade level

            // Store the new discount value
            currentDiscount = discountValues[currentDiscountLevel - 1];
            ShowMessage($"Weapon upgrade discount: {currentDiscount * 100}%!", Color.green);

            UpdateUI(); // Refresh UI
        }
        else
        {
            ShowMessage($"Not enough money! Need: {cost}", Color.red);
        }
    }

    // Shows a temporary message on the UI
    void ShowMessage(string message, Color color)
    {
        if (messageText != null)
        {
            messageText.text = message;
            messageText.color = color;
            Invoke(nameof(ClearMessage), messageDuration); // Auto-clear after duration
        }
        else
        {
            Debug.Log(message); // Fallback to console if no UI reference
        }
    }

    // Clears the message text
    void ClearMessage()
    {
        if (messageText != null)
            messageText.text = "";
    }

    // Closes the upgrade menu (called by close button)
    public void CloseUpgradeMenu()
    {
        gameObject.SetActive(false); // Hide the panel
        Time.timeScale = 1f; // Resume game time
    }

    // Public getters for external access
    public int GetSpeedLevel() => currentSpeedLevel;
    public int GetDiscountLevel() => currentDiscountLevel;
    public float GetCurrentDiscount() => currentDiscount;
}