using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class WeaponModificationShop : MonoBehaviour, IInteractableObject
{
    private PlayerStats playerStats;

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

    // Modification states (saved locally)
    private bool hasExplosiveMod = false;
    private bool hasFreezeMod = false;

    // Key for saving data
    private const string EXPLOSIVE_MOD_KEY = "HasExplosiveMod";
    private const string FREEZE_MOD_KEY = "HasFreezeMod";

    // Public getters for other scripts to check what mods are bought
    public bool HasExplosiveMod => hasExplosiveMod;
    public bool HasFreezeMod => hasFreezeMod;
    public UpgradeType upgradeType => UpgradeType.TurretUpgrade;

    void Start()
    {
        if (playerStats == null)
            Debug.LogError("playerStats == null");

        // Load saved modifications
        LoadModifications();

        // Subscribe button events
        if (explosivePurchaseButton != null)
            explosivePurchaseButton.onClick.AddListener(PurchaseExplosiveMod);

        if (freezePurchaseButton != null)
            freezePurchaseButton.onClick.AddListener(PurchaseFreezeMod);

        if (closeButton != null)
            closeButton.onClick.AddListener(CloseMenu);

        UpdateUI();
    }

    void LoadModifications()
    {
        // Load from PlayerPrefs (cached between game sessions)
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
        // Update money display
        if (moneyText != null && playerStats != null)
            moneyText.text = $"{playerStats.GetMoney()}";

        // Update explosive mod UI
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

        // Update freeze mod UI
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

        // Update button states
        UpdateButtonStates();
    }

    void UpdateButtonStates()
    {
        if (explosivePurchaseButton != null)
        {
            explosivePurchaseButton.interactable = !hasExplosiveMod &&
                                                   playerStats != null &&
                                                   playerStats.GetMoney() >= explosiveCost;
        }

        if (freezePurchaseButton != null)
        {
            freezePurchaseButton.interactable = !hasFreezeMod &&
                                                playerStats != null &&
                                                playerStats.GetMoney() >= freezeCost;
        }
    }

    void PurchaseExplosiveMod()
    {
        if (hasExplosiveMod)
        {
            ShowMessage("Взрывная модификация уже куплена!", Color.yellow);
            return;
        }

        if (playerStats != null && playerStats.GetMoney() >= explosiveCost)
        {
            // Deduct money
            playerStats.AddMoney(-explosiveCost);

            // Mark as purchased
            hasExplosiveMod = true;
            SaveModifications();

            // Just log the purchase - you can add your own logic here later
            Debug.Log("[WeaponShop] Explosive mod purchased! (You can add your own effect here)");
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

        if (playerStats != null && playerStats.GetMoney() >= freezeCost)
        {
            // Deduct money
            playerStats.AddMoney(-freezeCost);

            // Mark as purchased
            hasFreezeMod = true;
            SaveModifications();

            // Just log the purchase - you can add your own logic here later
            Debug.Log("[WeaponShop] Freeze mod purchased! (You can add your own effect here)");
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
    }

    public void Interact(PlayerStats playerStats)
    {
        this.playerStats = playerStats;
    }
}