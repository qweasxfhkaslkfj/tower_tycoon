using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Управляет всеми врагами на сцене: создание, учёт по путям, восстановление после смерти.
/// Manages all enemies: spawning, tracking per path, respawn on death.
/// </summary>
public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance { get; private set; }

    [System.Serializable]
    public class PathInfo
    {
        [Tooltip("Корневой объект пути (используется для привязки турелей)")]
        public Transform pathRoot;
        [Tooltip("Точка появления врагов / Spawn point")]
        public Transform spawnPoint;
    }

    [Header("Настройки путей / Path Settings")]
    [SerializeField] private PathInfo[] pathInfos;

    [Header("Префаб врага / Enemy Prefab")]
    [SerializeField] private GameObject enemyPrefab;

    [Header("Лимит врагов на путь / Max enemies per path")]
    [SerializeField] private int maxEnemiesPerPath = 10;

    private Dictionary<Transform, List<Enemy>> pathEnemies = new Dictionary<Transform, List<Enemy>>();

    private void Awake()
    {
        Instance = this;
        foreach (var info in pathInfos)
        {
            if (info.pathRoot != null)
                pathEnemies[info.pathRoot] = new List<Enemy>();
        }
    }

    private void Start()
    {
        foreach (var info in pathInfos)
        {
            for (int i = 0; i < maxEnemiesPerPath; i++)
                SpawnEnemy(info.pathRoot, info.spawnPoint);
        }
    }

    /// <summary>
    /// Получить список всех живых врагов на указанном пути.
    /// Get all alive enemies on a specific path.
    /// </summary>
    public List<Enemy> GetEnemiesOnPath(Transform pathRoot)
    {
        pathEnemies.TryGetValue(pathRoot, out List<Enemy> list);
        return list ?? new List<Enemy>(); // На случай, если путь не зарегистрирован
    }

    /// <summary>
    /// Вызывается врагом при смерти. Удаляет из списка и немедленно создаёт нового.
    /// Called by enemy on death. Removes from list and immediately spawns a replacement.
    /// </summary>
    public void OnEnemyDeath(Enemy deadEnemy, Transform pathRoot)
    {
        if (!pathEnemies.TryGetValue(pathRoot, out List<Enemy> enemies))
            return;

        enemies.Remove(deadEnemy);

        Transform spawnPoint = GetSpawnPoint(pathRoot);
        if (spawnPoint != null)
        {
            SpawnEnemy(pathRoot, spawnPoint);
        }
        else
        {
            Debug.LogWarning($"EnemyManager: не найдена точка спавна для пути {pathRoot.name}");
        }
    }

    /// <summary>
    /// Создать одного врага на пути, добавить в список.
    /// Spawn one enemy, add to path list.
    /// </summary>
    private void SpawnEnemy(Transform pathRoot, Transform spawnPoint)
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("EnemyManager: enemyPrefab не назначен!");
            return;
        }

        GameObject obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        Enemy enemy = obj.GetComponent<Enemy>();

        if (enemy == null)
        {
            Debug.LogError("EnemyManager: префаб врага не содержит компонент Enemy!");
            Destroy(obj);
            return;
        }

        enemy.Init(this, pathRoot);

        if (pathEnemies.ContainsKey(pathRoot))
            pathEnemies[pathRoot].Add(enemy);
    }

    /// <summary>
    /// Находит точку спавна для заданного пути.
    /// Find spawn point for a given path root.
    /// </summary>
    private Transform GetSpawnPoint(Transform pathRoot)
    {
        foreach (var info in pathInfos)
        {
            if (info.pathRoot == pathRoot)
                return info.spawnPoint;
        }
        return null;
    }
}