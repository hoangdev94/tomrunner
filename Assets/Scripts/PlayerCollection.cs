using UnityEngine;

public class PlayerConllection : MonoBehaviour
{

    private float distanceTravelled = 0f;
    private Vector3 lastPosition;

    private void Start()
    {
        lastPosition = transform.position;
    }
    private void Update()
    {
        float distance = Vector3.Distance(transform.position, lastPosition);
        distanceTravelled += distance;
        lastPosition = transform.position;
        GameManager.Instance.UpdateRoadDistance(distanceTravelled);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
 
            GameManager.Instance.AddScore(1);
            ObjectPool.Instance.ReturnToPool(other.gameObject);
            
        }
        if (other.CompareTag("Magnet"))
        {
            GameManager.Instance.AddMagnet(1);
            ObjectPool.Instance.ReturnToPool(other.gameObject);
        }
    }

}
