using UnityEngine;

public class Coin : MonoBehaviour
{
    public static Coin Instance;
    private void Awake()
    {
        Instance = this;
    }


    void Update()
    {
        
    }
}
