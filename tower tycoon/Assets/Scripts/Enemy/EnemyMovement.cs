using UnityEngine;

/// <summary> Движение врага по точкам пути / Enemy movement along waypoints </summary>
public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private Transform[] waypoints;
    private int currentIndex;
    private Enemy enemy;
    private EnemyView enemyView;

    public void InitPath(Transform pathRoot, Enemy enemy)
    {
        this.enemy = enemy;
        enemyView = GetComponent<EnemyView>();

        if (pathRoot == null) return;
        waypoints = new Transform[pathRoot.childCount];
        for (int i = 0; i < pathRoot.childCount; i++)
            waypoints[i] = pathRoot.GetChild(i);

        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            currentIndex = 1;
        }
    }

    private void Update()
    {
        if (waypoints == null || currentIndex >= waypoints.Length || enemy == null || !enemy.IsAlive)
            return;

        Transform target = waypoints[currentIndex];
        Vector2 dir = (target.position - transform.position).normalized;
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (dir != Vector2.zero)
            transform.right = dir;

        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentIndex++;
            if (currentIndex >= waypoints.Length)
            {
                enemyView?.ReachEnd();
            }
        }
    }
}