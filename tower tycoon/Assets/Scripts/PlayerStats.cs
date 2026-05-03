using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    // Singleton instance for global access from other scripts
    public static PlayerStats Instance;

    private int totalMoney = 0;

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI moneyText; // Reference to UI text displaying money amount
    [SerializeField] private int rewardPerEnemy = 10; // Amount of money rewarded for each killed enemy

    // Called when the script instance is being loaded (before Start)
    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    // Called before the first frame update
    void Start()
    {
        UpdateMoneyUI();
    }

    // Adds specified amount of money to player's total
    public void AddMoney(int amount)
    {
        totalMoney += amount;
        UpdateMoneyUI();
        Debug.Log($"Received {amount} coins! Total: {totalMoney}");
    }

    // Method to add enemy kill reward
    public void AddEnemyReward()
    {
        AddMoney(rewardPerEnemy);
    }

    // Updates the UI text element with current money value
    void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = $"{totalMoney}";
        else
            Debug.LogWarning("Money Text not assigned in the inspector!");
    }

    // Public getter method to retrieve current money amount
    public int GetMoney() => totalMoney;
}