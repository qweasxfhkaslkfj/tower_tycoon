using TMPro;
using UnityEngine;

public class PlayerStatsUI : MonoBehaviour
{
    // Serialize Fields
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private PlayerStats playerStats;

    // Start settings
    private void Start()
    {
        if (playerStats == null)
        {
            Debug.LogError("playerStats == null");
            return;
        }
        if (moneyText == null)
        {
            Debug.LogError("moneyText == null");
            return;
        }

        playerStats.OnMoneyChanged += UpdateMoneyUI;
        UpdateMoneyUI(playerStats.GetMoney());
    }

    // Update money
    private void UpdateMoneyUI(int money)
    {
        moneyText.text = $"{money}";
    }

    // Destroy object
    private void OnDestroy()
    {
        if (moneyText != null)
            playerStats.OnMoneyChanged -= UpdateMoneyUI;
    }
}
