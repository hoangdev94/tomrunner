using UnityEngine;

public class GroundTile : MonoBehaviour
{
    public float despawnDistance = 50f;
    private bool hasSpawned = false;

    private void Update()
    {
        // Chỉ return khi player chạy đủ xa
        if (transform.position.z < PlayerController.Instance.transform.position.z - 50f)
        {
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }

    public void OnPlayerTriggered()
    {
        if (hasSpawned)
            return;

        hasSpawned = true;
        for (int i = 0; i < 2; i++)
        {
            GroundSpawner.Instance.SpawnNextPath();
        }
    }
}
