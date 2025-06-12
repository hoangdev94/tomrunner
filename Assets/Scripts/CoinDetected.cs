using UnityEngine;

public class CoinDetected : MonoBehaviour
{
   

    public static CoinDetected Instance;
    private void Start()
    {
        Instance = this;
        gameObject.SetActive(false);
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Coin"))
        {
            CoinFollow coin = other.GetComponent<CoinFollow>();
            if (coin != null)
            {
                coin.StartFollowing();
            }
        }
    }
}
