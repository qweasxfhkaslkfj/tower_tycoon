using UnityEngine;

/// <summary>
/// Двигает врага по набору точек пути / Moves enemy along waypoints.
/// Точки берутся из дочерних объектов pathRoot.
/// </summary>
public class EnemyMovement : MonoBehaviour
{
    [Header("Скорость / Movement speed")]
    [SerializeField] private float speed = 2f;

    private Transform[] waypoints; 
    private int currentWaypointIndex;
    private bool moving;

    /// <summary>
    /// Инициализация: получает путь от родительского pathRoot / Init: gets waypoints from pathRoot.
    /// Вызывается из Enemy.Init или в Start.
    /// </summary>
    public void InitPath(Transform pathRoot)
    {
        if (pathRoot == null)
        {
            Debug.LogError("EnemyMovement: pathRoot is null");
            enabled = false;
            return;
        }

        waypoints = new Transform[pathRoot.childCount];
        for (int i = 0; i < pathRoot.childCount; i++)
        {
            waypoints[i] = pathRoot.GetChild(i);
        }

        if (waypoints.Length == 0)
        {
            Debug.LogWarning($"EnemyMovement: нет точек пути в {pathRoot.name}");
            enabled = false;
            return;
        }

        transform.position = waypoints[0].position;
        currentWaypointIndex = 1;
        moving = true;
    }

    private void Update()
    {
        if (!moving || waypoints == null || currentWaypointIndex >= waypoints.Length)
            return;

        Transform target = waypoints[currentWaypointIndex];
        Vector2 direction = (target.position - transform.position).normalized;

        if (direction != Vector2.zero)
            transform.right = direction;

        transform.position = Vector2.MoveTowards(
            transform.position,
            target.position,
            speed * Time.deltaTime);

        if (((Vector2)transform.position - (Vector2)target.position).sqrMagnitude < 0.01f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                moving = false;
                Enemy enemy = GetComponent<Enemy>();
                if (enemy != null)
                    enemy.ReachEnd();
                Destroy(gameObject);
            }
        }
    }
}