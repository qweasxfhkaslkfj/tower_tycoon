using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] private float speed = 2f;
    private Transform[] waypoints;
    private int currentWaypointIndex;

    public void InitPath(Transform pathRoot)
    {
        if (pathRoot == null) return;
        waypoints = new Transform[pathRoot.childCount];
        for (int i = 0; i < pathRoot.childCount; i++)
            waypoints[i] = pathRoot.GetChild(i);
        if (waypoints.Length > 0)
        {
            transform.position = waypoints[0].position;
            currentWaypointIndex = 1;
        }
    }

    private void Update()
    {
        if (waypoints == null || currentWaypointIndex >= waypoints.Length) return;
        Transform target = waypoints[currentWaypointIndex];
        transform.position = Vector2.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
        if (Vector2.Distance(transform.position, target.position) < 0.05f)
        {
            currentWaypointIndex++;
            if (currentWaypointIndex >= waypoints.Length)
            {
                Enemy enemy = GetComponent<Enemy>();
                if (enemy != null) enemy.ReachEnd();
                Destroy(gameObject);
            }
        }
    }
}