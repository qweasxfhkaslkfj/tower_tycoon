using System.Collections.Generic;
using UnityEngine;

/// <summary> Предоставляет список врагов в заданной области / Provides enemies in range </summary>
public interface IEnemyProvider
{
    List<Enemy> GetEnemiesInRange(Vector2 point, float range);
}