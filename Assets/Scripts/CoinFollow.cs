using UnityEngine;

public class CoinFollow : MonoBehaviour
{
    public float moveSpeed = 60f;
    private bool shouldFollow = false;

    void Update()
    {
        if (shouldFollow)
        {
           
            Vector3 target = PlayerController.Instance.coinTargetPoint.position;
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);


        }
    }

    public void StartFollowing()
    {
        shouldFollow = true;
    }
}
