using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            GroundSpawner.Instance.SpawnNextPath();
            hasSpawned = true;
            Destroy(transform.root.gameObject,10f);
        }
    }
}
