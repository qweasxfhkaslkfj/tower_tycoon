using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponModificationShopUI : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private TextMeshProUGUI messageText;
    [SerializeField] private float messageDuration = 2f;

    [Header("Explosive Mod UI")]
    [SerializeField] private Button explosivePurchaseButton;
    [SerializeField] private TextMeshProUGUI explosiveCostText;
    [SerializeField] private TextMeshProUGUI explosiveStatusText;

    [Header("Freeze Mod UI")]
    [SerializeField] private Button freezePurchaseButton;
    [SerializeField] private TextMeshProUGUI freezeCostText;
    [SerializeField] private TextMeshProUGUI freezeStatusText;

    [Header("Close Button")]
    [SerializeField] private Button closeButton;

    private WeaponModificationShopLogic shopLogic;
    private PlayerStats playerStats;

    void Start()
    {
        shopLogic = GetComponent<WeaponModificationShopLogic>();

        if (shopLogic == null)
            shopLogic = GetComponentInParent<WeaponModificationShopLogic>();

        if (shopLogic == null)
        {
            Debug.LogError("WeaponModificationShopLogic not found!");
            return;
        }

        shopLogic.OnDataChanged += UpdateUI;
        shopLogic.OnMessageShow += ShowMessage;

        if (explosivePurchaseButton != null)
            explosivePurchaseButton.onClick.AddListener(() => shopLogic.PurchaseExplosiveMod());

        if (freezePurchaseButton != null)
            freezePurchaseButton.onClick.AddListener(() => shopLogic.PurchaseFreezeMod());

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);

        UpdateUI();
    }

    void UpdateUI()
    {
        // Проверяем, что shopLogic существует
        if (shopLogic == null) return;

        // Обновление денег
        if (moneyText != null && playerStats != null)
        {
            moneyText.text = $"{playerStats.GetMoney()}";
        }

        // Взрывная модификация
        if (explosiveCostText != null)
        {
            if (!shopLogic.HasExplosiveMod)
                explosiveCostText.text = $"Цена: {shopLogic.GetExplosiveCost()}";
            else
                explosiveCostText.text = "КУПЛЕНО";
        }

        if (explosiveStatusText != null)
        {
            explosiveStatusText.text = shopLogic.HasExplosiveMod ? "✓ АКТИВНО" : "НЕ КУПЛЕНО";
            explosiveStatusText.color = shopLogic.HasExplosiveMod ? Color.green : Color.gray;
        }

        if (explosivePurchaseButton != null)
        {
            explosivePurchaseButton.interactable = shopLogic.CanPurchaseExplosive();

            var buttonText = explosivePurchaseButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
                buttonText.text = shopLogic.HasExplosiveMod ? "КУПЛЕНО" : "КУПИТЬ";
        }

        // Модификация заморозки
        if (freezeCostText != null)
        {
            if (!shopLogic.HasFreezeMod)
                freezeCostText.text = $"Цена: {shopLogic.GetFreezeCost()}";
            else
                freezeCostText.text = "КУПЛЕНО";
        }

        if (freezeStatusText != null)
        {
            freezeStatusText.text = shopLogic.HasFreezeMod ? "✓ АКТИВНО" : "НЕ КУПЛЕНО";
            freezeStatusText.color = shopLogic.HasFreezeMod ? Color.green : Color.gray;
        }

        if (freezePurchaseButton != null)
        {
            freezePurchaseButton.interactable = shopLogic.CanPurchaseFreeze();
            var buttonText = freezePurchaseButton.GetComponentInChildren<TextMeshProUGUI>();
            if (buttonText != null)
                buttonText.text = shopLogic.HasFreezeMod ? "КУПЛЕНО" : "КУПИТЬ";
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

    public void CloseMenu()
    {
        gameObject.SetActive(false);
        Time.timeScale = 1f;
    }

    public void SetPlayerStats(PlayerStats stats)
    {
        playerStats = stats;
        if (shopLogic == null)
        {
            shopLogic = GetComponent<WeaponModificationShopLogic>();
            if (shopLogic == null)
                shopLogic = GetComponentInParent<WeaponModificationShopLogic>();
        }
        UpdateUI();
    }
}


