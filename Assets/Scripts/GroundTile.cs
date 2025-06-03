using UnityEngine;

public class GroundTile : MonoBehaviour
{
    private bool hasSpawned = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasSpawned)
        {
            Debug.Log("Player entered: " + gameObject.name);
            GroundSpawner.Instance.SpawnNextPath();
            hasSpawned = true;
            Destroy(transform.root.gameObject,2f);
        }
    }
}
