using System.Collections.Generic;
using UnityEngine;

/// <summary> Пул объектов для снарядов / Projectile object pool </summary>
public class ObjectPool : MonoBehaviour, IProjectileFactory
{
    [SerializeField] private GameObject projectilePrefab;
    [SerializeField] private int initialSize = 10;

    private Queue<ProjectileView> pool = new();

    private void Awake()
    {
        for (int i = 0; i < initialSize; i++)
        {
            ProjectileView obj = CreateNew();
            obj.gameObject.SetActive(false);
            pool.Enqueue(obj);
        }
    }

    public ProjectileView Create()
    {
        if (pool.Count == 0)
        {
            ProjectileView newObj = CreateNew();
            newObj.gameObject.SetActive(false);
            pool.Enqueue(newObj);
        }
        ProjectileView proj = pool.Dequeue();
        proj.gameObject.SetActive(true);
        return proj;
    }

    public static void Return(GameObject obj)
    {
        ProjectileView proj = obj.GetComponent<ProjectileView>();
        if (proj != null && Instance != null)
        {
            obj.SetActive(false);
            Instance.pool.Enqueue(proj);
        }
        else
        {
            Destroy(obj);
        }
    }

    private ProjectileView CreateNew()
    {
        GameObject obj = Instantiate(projectilePrefab, transform);
        ProjectileView view = obj.GetComponent<ProjectileView>();
        if (view == null)
            view = obj.AddComponent<ProjectileView>();
        return view;
    }

    private static ObjectPool Instance;
    private void OnEnable() => Instance = this;
}