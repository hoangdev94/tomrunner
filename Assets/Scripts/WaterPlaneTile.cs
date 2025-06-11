using UnityEngine;

public class WaterPlaneTile : MonoBehaviour
{
    private bool hasSpawned = false;
    public void OnPlayerTriggered()
    {
        if (hasSpawned)
            return;
        hasSpawned = true;
        WaterPlaneSpawn.Instance.SpawnNextWaterPlane();
        Invoke(nameof(ReturnToPool), 5f);
    }
    void ReturnToPool()
    {
        ObjectPool.Instance.ReturnToPool(gameObject);
    }
}
