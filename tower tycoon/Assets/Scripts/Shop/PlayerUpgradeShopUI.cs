using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PlayerUpgradeShopUI : MonoBehaviour
{
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
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float messageDuration = 2f;

    [Header("Close Button")]
    [SerializeField] private Button closeButton; 

    private PlayerUpgradeShopLogic shopLogic;
    private PlayerStats playerStats;

    void Start()
    {
        shopLogic = GetComponent<PlayerUpgradeShopLogic>();

        if (shopLogic == null)
        {
            Debug.LogError("PlayerUpgradeShopLogic not found!");
            return;
        }

        shopLogic.OnDataChanged += UpdateUI;
        shopLogic.OnMessageShow += ShowMessage;

        if (speedUpgradeButton != null)
            speedUpgradeButton.onClick.AddListener(() => shopLogic.UpgradeSpeed());

        if (discountUpgradeButton != null)
            discountUpgradeButton.onClick.AddListener(() => shopLogic.UpgradeDiscount());

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseUpgradeMenu);

        UpdateUI();
    }

    public void UpdateUI()
    {
        if (shopLogic == null) return;

        if (moneyText != null && playerStats != null)
            moneyText.text = $"{playerStats.GetMoney()}";

        if (speedLevelText != null)
            speedLevelText.text = $"{shopLogic.CurrentSpeedLevel}/{shopLogic.MaxUpgradeLevel}";

        if (speedCostText != null)
        {
            int cost = shopLogic.GetSpeedCost();
            speedCostText.text = cost > 0 ? $"{cost}" : "MAX";
        }

        if (speedBonusText != null)
            speedBonusText.text = $"x{shopLogic.GetCurrentSpeedBonus()}";

        if (discountLevelText != null)
            discountLevelText.text = $"{shopLogic.CurrentDiscountLevel}/{shopLogic.MaxUpgradeLevel}";

        if (discountCostText != null)
        {
            int cost = shopLogic.GetDiscountCost();
            discountCostText.text = cost > 0 ? $"{cost}" : "MAX";
        }

        if (discountBonusText != null)
            discountBonusText.text = $"-{shopLogic.GetCurrentDiscountPercent()}%";

        if (speedUpgradeButton != null)
            speedUpgradeButton.interactable = shopLogic.CanUpgradeSpeed();

        if (discountUpgradeButton != null)
            discountUpgradeButton.interactable = shopLogic.CanUpgradeDiscount();
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
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
        if (shopLogic != null)
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            PlayerController controller = player?.GetComponent<PlayerController>();
            shopLogic.Initialize(stats, controller);
        }
        UpdateUI();
    }
}