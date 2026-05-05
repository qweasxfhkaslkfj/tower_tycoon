using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WeaponModificationShop : MonoBehaviour
{
    [Header("Modification Settings")]
    [SerializeField] private int explosiveCost = 300;
    [SerializeField] private int freezeCost = 350;

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

    private bool hasExplosiveMod = false;
    private bool hasFreezeMod = false;

    private PlayerStats currentPlayerStats;

    private const string EXPLOSIVE_MOD_KEY = "HasExplosiveMod";
    private const string FREEZE_MOD_KEY = "HasFreezeMod";

    void Start()
    {
        LoadModifications();

        if (explosivePurchaseButton != null)
            explosivePurchaseButton.onClick.AddListener(PurchaseExplosiveMod);

        if (freezePurchaseButton != null)
            freezePurchaseButton.onClick.AddListener(PurchaseFreezeMod);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);
    }

    public void Initialize(PlayerStats playerStats)
    {
        currentPlayerStats = playerStats;
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

    public void UpdateUI()
    {
        if (moneyText != null && currentPlayerStats != null)
            moneyText.text = $"{currentPlayerStats.GetMoney()}";
        else if (moneyText != null)
            moneyText.text = "0";

        if (explosiveCostText != null)
        {
            if (!hasExplosiveMod)
                explosiveCostText.text = $"Цена: {explosiveCost}";
            else
                explosiveCostText.text = "КУПЛЕНО";
        }

        if (explosiveStatusText != null)
        {
            if (hasExplosiveMod)
                explosiveStatusText.text = "✓ АКТИВНО";
            else
                explosiveStatusText.text = "НЕ КУПЛЕНО";

            explosiveStatusText.color = hasExplosiveMod ? Color.green : Color.gray;
        }

        if (freezeCostText != null)
        {
            if (!hasFreezeMod)
                freezeCostText.text = $"Цена: {freezeCost}";
            else
                freezeCostText.text = "КУПЛЕНО";
        }

        if (freezeStatusText != null)
        {
            if (hasFreezeMod)
                freezeStatusText.text = "✓ АКТИВНО";
            else
                freezeStatusText.text = "НЕ КУПЛЕНО";

            freezeStatusText.color = hasFreezeMod ? Color.green : Color.gray;
        }

        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        bool hasValidPlayer = currentPlayerStats != null;

        if (explosivePurchaseButton != null)
        {
            explosivePurchaseButton.interactable = !hasExplosiveMod && hasValidPlayer &&
                                                   currentPlayerStats.GetMoney() >= explosiveCost;
        }

        if (freezePurchaseButton != null)
        {
            freezePurchaseButton.interactable = !hasFreezeMod && hasValidPlayer &&
                                                currentPlayerStats.GetMoney() >= freezeCost;
        }
    }

    void PurchaseExplosiveMod()
    {
        if (hasExplosiveMod)
        {
            ShowMessage("Взрывная модификация уже куплена!", Color.yellow);
            return;
        }

        if (currentPlayerStats == null)
        {
            ShowMessage("Error: No player data!", Color.red);
            return;
        }

        if (currentPlayerStats.GetMoney() >= explosiveCost)
        {
            currentPlayerStats.AddMoney(-explosiveCost);
            hasExplosiveMod = true;
            SaveModifications();

            Debug.Log("[WeaponShop] Explosive mod purchased!");
            ShowMessage("Взрывные снаряды куплены! Теперь ваши снаряды взрываются!", Color.green);
            UpdateUI();
        }
        else
        {
            ShowMessage($"Недостаточно денег! Нужно: {explosiveCost}", Color.red);
        }
    }

    void PurchaseFreezeMod()
    {
        if (hasFreezeMod)
        {
            ShowMessage("Модификация заморозки уже куплена!", Color.yellow);
            return;
        }

        if (currentPlayerStats == null)
        {
            ShowMessage("Error: No player data!", Color.red);
            return;
        }

        if (currentPlayerStats.GetMoney() >= freezeCost)
        {
            currentPlayerStats.AddMoney(-freezeCost);
            hasFreezeMod = true;
            SaveModifications();

            Debug.Log("[WeaponShop] Freeze mod purchased!");
            ShowMessage("Заморозка снарядов куплена! Враги будут замораживаться!", Color.green);
            UpdateUI();
        }
        else
        {
            ShowMessage($"Недостаточно денег! Нужно: {freezeCost}", Color.red);
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
        currentPlayerStats = null;
    }

    public bool HasExplosiveMod => hasExplosiveMod;
    public bool HasFreezeMod => hasFreezeMod;
}