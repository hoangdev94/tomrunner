using UnityEngine;

public class WaterPlaneTile : MonoBehaviour
{
    private bool hasSpawned = false;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            WaterPlaneSpawn.Instance.SpawnNextWaterPlane();
            Debug.Log("WaterPlane");
            hasSpawned = true;
            Destroy(transform.root.gameObject, 5f);
        }
    }
}
