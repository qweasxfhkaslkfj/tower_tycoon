using UnityEngine;

public class PlayerProximityUpdater : MonoBehaviour
{
    [SerializeField] private float radius = 2f;

    void Update()
    {
        TurretManager.Instance?.UpdatePlayerProximity(transform.position, radius);
    }
}