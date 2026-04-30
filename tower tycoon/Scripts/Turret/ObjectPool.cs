using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Пул объектов для снарядов / Object pool for projectiles.
/// Один на сцене, сам создаётся при первом обращении.
/// </summary>
public class ObjectPool : MonoBehaviour
{
    private Dictionary<GameObject, Queue<GameObject>> poolDict
        = new Dictionary<GameObject, Queue<GameObject>>();

    private static ObjectPool instance;
    public static ObjectPool Instance
    {
        get
        {
            if (instance == null)
            {
                GameObject go = new GameObject("ObjectPool");
                instance = go.AddComponent<ObjectPool>();
                DontDestroyOnLoad(go);
            }
            return instance;
        }
    }

    /// <summary> Создать пул для префаба (вызывается один раз) / Create pool for a prefab </summary>
    public static ObjectPool CreatePool(GameObject prefab, int initialSize)
    {
        ObjectPool pool = Instance;
        if (!pool.poolDict.ContainsKey(prefab))
        {
            Queue<GameObject> queue = new Queue<GameObject>();
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = Instantiate(prefab, pool.transform);
                obj.SetActive(false);
                queue.Enqueue(obj);
            }
            pool.poolDict[prefab] = queue;
        }
        return pool;
    }

    /// <summary> Взять объект из пула / Get object from pool </summary>
    public GameObject Get(GameObject prefab)
    {
        if (!poolDict.TryGetValue(prefab, out Queue<GameObject> queue))
            return null;

        if (queue.Count == 0)
        {
            // Расширение пула / Expand pool
            GameObject newObj = Instantiate(prefab, transform);
            newObj.SetActive(false);
            queue.Enqueue(newObj);
        }

        GameObject obj = queue.Dequeue();
        obj.SetActive(true);
        return obj;
    }

    /// <summary> Вернуть объект в пул / Return object to pool </summary>
    public static void Return(GameObject obj)
    {
        if (obj == null) return;
        obj.SetActive(false);

        foreach (var kvp in Instance.poolDict)
        {
            if (obj.name.StartsWith(kvp.Key.name))
            {
                kvp.Value.Enqueue(obj);
                return;
            }
        }

        // На всякий случай / Fallback destroy
        Destroy(obj);
    }
}