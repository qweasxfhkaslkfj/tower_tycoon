using System;
using TMPro;
using UnityEngine;

public class PlayerStats : MonoBehaviour
{
    // Serialize Fields
    [SerializeField] private int totalMoney = 50;
    [SerializeField] private int rewardPerEnemy = 10;

    public event Action<int> OnMoneyChanged;

    // Start check
    private void Start()
    {
        if (OnMoneyChanged != null)
            OnMoneyChanged(totalMoney);
    }

    // Add money
    public void AddMoney(int amount)
    {
        totalMoney += amount;
        if (OnMoneyChanged != null)
            OnMoneyChanged(totalMoney);
    }

    // Reward per enemy
    public void AddEnemyReward()
    {
        AddMoney(rewardPerEnemy);
    }

    // Checking for sufficient money
    public bool SpendMoney(int amount)
    {
        if (totalMoney >= amount)
        {
            AddMoney(-amount);
            return true;
        }
        return false;
    }

    // Current money
    public int GetMoney()
    {
        return totalMoney;
    }
}