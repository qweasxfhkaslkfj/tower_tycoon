using System.Collections.Generic;
using UnityEngine;

/// <summary> Управляет режимами турелей (ручное/авто) / Turret mode manager </summary>
public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance { get; private set; }

    private List<TurretView> allTurrets = new();
    private TurretView lastPlayerTurret;

    private void Awake() => Instance = this;

    /// <summary> Зарегистрировать новую турель / Register new turret </summary>
    public void RegisterTurret(TurretView turretView)
    {
        allTurrets.Add(turretView);
        SetLastPlayerTurret(turretView);
    }

    private void SetLastPlayerTurret(TurretView newTurret)
    {
        foreach (var t in allTurrets)
            t.SetModes(true, false); // все авто

        lastPlayerTurret = newTurret;
        newTurret.SetModes(false, false); // новая пока не ручная, пока игрок не подойдёт
    }

    /// <summary> Обновить близость игрока / Update player proximity </summary>
    public void UpdatePlayerProximity(Vector2 playerPos, float radius)
    {
        float sqrRadius = radius * radius;
        foreach (var turret in allTurrets)
        {
            if (turret == lastPlayerTurret)
            {
                bool near = ((Vector2)turret.transform.position - playerPos).sqrMagnitude <= sqrRadius;
                turret.SetModes(false, near); // ручная, если игрок рядом
            }
            else
            {
                turret.SetModes(true, false);
            }
        }
    }
}