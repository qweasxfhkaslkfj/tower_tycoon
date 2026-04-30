using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет всеми турелями: ручное/автоматическое переключение / Manages all turrets
/// </summary>
public class TurretManager : MonoBehaviour
{
    public static TurretManager Instance { get; private set; }

    private List<Turret> allTurrets = new List<Turret>();
    private Turret lastPlayerTurret;

    private void Awake() => Instance = this;

    /// <summary> Зарегистрировать новую турель / Register a new turret </summary>
    public void RegisterTurret(Turret turret, Transform pathRoot)
    {
        allTurrets.Add(turret);
        SetAsLastPlayerTurret(turret);
    }

    private void SetAsLastPlayerTurret(Turret newTurret)
    {
        foreach (var t in allTurrets)
            t.IsAutomatic = true;

        lastPlayerTurret = newTurret;
        newTurret.IsAutomatic = false;
    }

    /// <summary> Обновить состояние близости игрока / Update player proximity for turrets </summary>
    public void UpdatePlayerProximity(Vector2 playerPos, float radius)
    {
        float sqrRadius = radius * radius;
        foreach (var turret in allTurrets)
        {
            if (turret == lastPlayerTurret)
            {
                turret.IsPlayerNearby =
                    ((Vector2)turret.transform.position - playerPos).sqrMagnitude <= sqrRadius;
            }
            else
            {
                turret.IsPlayerNearby = false;
            }
        }
    }
}