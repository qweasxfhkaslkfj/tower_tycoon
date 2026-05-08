using System;
using UnityEngine;

/// <summary> Бизнес-данные и улучшения турели / Turret business data & upgrades </summary>
[System.Serializable]
public class Turret
{
    public TurretData Data { get; }
    public int Damage { get; private set; }
    public int KillReward { get; private set; }
    public int Level { get; private set; }

    /// <summary> Событие, вызываемое при убийстве врага / Event invoked on enemy kill </summary>
    public event Action<int> OnKillReward;

    public Turret(TurretData data)
    {
        Data = data;
        Damage = data.damage;
        KillReward = data.baseKillReward;
    }

    /// <summary> Стоимость следующего улучшения / Next upgrade cost </summary>
    public int GetUpgradeCost() => Mathf.RoundToInt(50 * Mathf.Pow(1.1f, Level));

    /// <summary> Повысить уровень / Increase level </summary>
    public void Upgrade()
    {
        Level++;
        Damage = Mathf.RoundToInt(Damage * 1.1f);
        KillReward = Mathf.Max(1, Mathf.RoundToInt(KillReward * 1.1f));
    }

    /// <summary> Вызвать начисление награды (вызывается снарядом/врагом) / Trigger kill reward </summary>
    public void AddKillReward()
    {
        OnKillReward?.Invoke(KillReward);
    }
}