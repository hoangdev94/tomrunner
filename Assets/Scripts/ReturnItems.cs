using UnityEngine;

public class ReturnItems : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (transform.position.z < PlayerController.Instance.transform.position.z - 5f) 
        {
            ObjectPool.Instance.ReturnToPool(gameObject);
        }
    }

}
