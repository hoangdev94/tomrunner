using UnityEngine;

public class WaterTrigger : MonoBehaviour
{
    WaterPlaneTile waterPlaneTile;
    private void Awake()
    {
        waterPlaneTile = GetComponentInParent<WaterPlaneTile>();

    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            waterPlaneTile.OnPlayerTriggered();
        }
    }
}
