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
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    instance = player.GetComponent<PlayerStats>();
                    if (instance == null)
                    {
                        instance = player.AddComponent<PlayerStats>();
                    }
                }
                else
                {
                    GameObject obj = new GameObject("PlayerStats");
                    instance = obj.AddComponent<PlayerStats>();
                }
            }
            return instance;
        }
    }

    private int totalMoney = 50;

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
    }

    public void AddEnemyReward()
    {
        AddMoney(rewardPerEnemy);
    }

    void UpdateMoneyUI()
    {
        if (moneyText != null)
            moneyText.text = $"{totalMoney}";
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