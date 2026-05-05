using UnityEngine;
using TMPro;

public class PlayerStats : MonoBehaviour
{
    private static PlayerStats instance;
    public static PlayerStats Instance
    {
        get
        {
            if (instance == null)
            {
                instance = FindObjectOfType<PlayerStats>();
                if (instance == null)
                {
                    GameObject obj = new GameObject("PlayerStats");
                    instance = obj.AddComponent<PlayerStats>();
                }
            }
            return instance;
        }
    }

    private int totalMoney = 400;

    [Header("UI Settings")]
    [SerializeField] private TextMeshProUGUI moneyText;
    [SerializeField] private int rewardPerEnemy = 10;

    void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject); 
        }
        else if (instance != this)
        {
            Destroy(gameObject);
        }
    }

    void Start()
    {
        if (moneyText == null)
            moneyText = GetComponentInChildren<TextMeshProUGUI>();
        UpdateMoneyUI();
    }

    public void AddMoney(int amount)
    {
        totalMoney += amount;
        UpdateMoneyUI();

        if (amount > 0)
            Debug.Log($"Получено {amount} монет! Всего: {totalMoney}");
        else
            Debug.Log($"Потрачено {-amount} монет! Осталось: {totalMoney}");
    }

    public void AddEnemyReward()
    {
        AddMoney(rewardPerEnemy);
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = $"{totalMoney}";
        else
            Debug.LogWarning("Money Text not assigned in the inspector!");
    }

    public int GetMoney() => totalMoney;

    public bool SpendMoney(int amount)
    {
        if (totalMoney >= amount)
        {
            AddMoney(-amount);
            return true;
        }
        return false;
    }

    public static void ResetInstance()
    {
        instance = null;
    }
}
