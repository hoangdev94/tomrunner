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
        GameManager.Instance.UpdateRoadText(distanceTravelled);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);
            GameManager.Instance.AddScore(1);
            
        }
    }

}
