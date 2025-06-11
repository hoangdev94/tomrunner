using UnityEngine;

public class DestroyAfterPlayerRun : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Invoke(nameof(DestroyGameObject), 5f);
        }
    }
    void DestroyGameObject()
    {
        Destroy(transform.parent.gameObject);
    }
}
