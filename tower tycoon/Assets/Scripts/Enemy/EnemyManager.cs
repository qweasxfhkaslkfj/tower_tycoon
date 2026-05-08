using System.Collections.Generic;
using UnityEngine;

/// <summary> Управление врагами: спавн, учёт по путям, предоставление данных / Enemy management </summary>
public class EnemyManager : MonoBehaviour, IEnemyProvider
{
    public static EnemyManager Instance { get; private set; }

    [System.Serializable]
    public class PathInfo
    {
        public Transform pathRoot;
        public Transform spawnPoint;
    }

    [SerializeField] private PathInfo[] pathInfos;
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private int maxEnemiesPerPath = 10;

    private Dictionary<Transform, List<EnemyView>> pathEnemies = new();

    private void Awake() => Instance = this;

    private void Start()
    {
        foreach (var info in pathInfos)
            for (int i = 0; i < maxEnemiesPerPath; i++)
                SpawnEnemy(info.pathRoot, info.spawnPoint);
    }

    private void SpawnEnemy(Transform pathRoot, Transform spawnPoint)
    {
        GameObject obj = Instantiate(enemyPrefab, spawnPoint.position, Quaternion.identity);
        EnemyView view = obj.GetComponent<EnemyView>();
        if (view == null)
        {
            Debug.LogError("Enemy prefab missing EnemyView component");
            Destroy(obj);
            return;
        }
        view.Init(this, pathRoot);
        if (!pathEnemies.ContainsKey(pathRoot))
            pathEnemies[pathRoot] = new List<EnemyView>();
        pathEnemies[pathRoot].Add(view);
    }

    /// <summary> Уведомление о смерти врага / Enemy death notification </summary>
    public void OnEnemyDeath(EnemyView deadView, Transform pathRoot)
    {
        if (pathRoot == null) return;
        if (pathEnemies.TryGetValue(pathRoot, out var list))
            list.Remove(deadView);
        // Найти точку спавна
        Transform spawnPoint = GetSpawnPoint(pathRoot);
        if (spawnPoint != null)
            SpawnEnemy(pathRoot, spawnPoint);
    }

    private Transform GetSpawnPoint(Transform pathRoot)
    {
        foreach (var info in pathInfos)
            if (info.pathRoot == pathRoot) return info.spawnPoint;
        return null;
    }

    // IEnemyProvider
    public List<Enemy> GetEnemiesInRange(Vector2 point, float range)
    {
        List<Enemy> result = new();
        float sqrRange = range * range;
        foreach (var kvp in pathEnemies)
        {
            foreach (var view in kvp.Value)
            {
                if (view.Enemy.IsAlive && ((Vector2)view.transform.position - point).sqrMagnitude <= sqrRange)
                    result.Add(view.Enemy);
            }
        }
        return result;
    }
}