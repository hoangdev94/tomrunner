using UnityEngine;

public class GroundTrigger : MonoBehaviour
{
    GroundTile groundTile;
    private void Awake()
    {
        groundTile = GetComponentInParent<GroundTile>();
        if (groundTile == null)
            Debug.LogError("[GroundTrigger] GroundTile is NULL!");

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") )
        {
            groundTile.OnPlayerTriggered();
        }
    }
}
