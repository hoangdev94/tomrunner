using UnityEngine;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public Transform[] spawnPoints; // Gán 3 vị trí spawnCoin vào đây

    public int minCoins = 3;
    public int maxCoins = 6;
    public float spacing = 2f;

    void Start()
    {
        SpawnCoinLine();
    }

    void SpawnCoinLine()
    {
        if (spawnPoints.Length == 0 || coinPrefab == null) return;

        // Chọn ngẫu nhiên 1 vị trí spawn trong số 3 cái
        Transform chosenPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];

        int coinCount = Random.Range(minCoins, maxCoins + 1);
        Vector3 startPos = chosenPoint.position;

        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPos = startPos + new Vector3(0, 0, i * spacing);
            Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform);
        }
    }
}
