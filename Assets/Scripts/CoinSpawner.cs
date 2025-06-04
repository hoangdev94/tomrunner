using UnityEngine;
using System.Collections.Generic;

public class CoinSpawner : MonoBehaviour
{
    public GameObject coinPrefab;
    public GameObject[] trainPrefabs;
    public bool spawnTrain = true;
    public bool spawnTrap = true;
    public GameObject[] trapPrefabs;

    public Transform[] spawnPoints; // 3 vị trí đường (coin + tàu dùng chung)

    public int minCoins = 3;
    public int maxCoins = 6;
    public float spacing = 2f;

    void Start()
    {
        SpawnCoinorTrainorTrap();
    }

    void SpawnCoinorTrainorTrap()
    {
        if (spawnPoints.Length < 3 || trainPrefabs.Length == 0 || coinPrefab == null) return;

        // Tạo danh sách chỉ số 0, 1, 2
        List<int> indices = new List<int> { 0, 1, 2 };

        // Random vị trí sử dụng
        int chosenIndex = indices[Random.Range(0, indices.Count)];
        Transform spawnPoint = spawnPoints[chosenIndex];

        // Random: 0 = coin, 1 = tàu
        int spawnType = Random.Range(0, 3);

        switch (spawnType)
        {
            case 0:
                SpawnCoinLine(spawnPoint);
                break;
            case 1:
                if (spawnTrain) 
                    SpawnTrain(spawnPoint);
                else
                    SpawnCoinLine(spawnPoint);
                break;
            case 2:
                SpawnTrap(spawnPoint);
                break;
        }
    }

    void SpawnCoinLine(Transform spawnPoint)
    {
        int coinCount = Random.Range(minCoins, maxCoins + 1);
        Vector3 startPos = spawnPoint.position;

        for (int i = 0; i < coinCount; i++)
        {
            Vector3 spawnPos = startPos + new Vector3(0, 0, i * spacing);
            Instantiate(coinPrefab, spawnPos, Quaternion.identity, transform);
        }

    }

    void SpawnTrain(Transform spawnPoint)
    {
        if (spawnTrain)
        {
            GameObject train = trainPrefabs[Random.Range(0, trainPrefabs.Length)];

            Vector3 spawnPos = spawnPoint.position;
            // Đảm bảo y = 0
            spawnPos.y = 0f;
            Instantiate(train, spawnPos, spawnPoint.rotation, transform);

        }
        else return;

    }
    void SpawnTrap(Transform spawnPoint)
    {

        if (spawnTrap) 
        {
            GameObject trap = trapPrefabs[Random.Range(0, trapPrefabs.Length)];
            Vector3 spawnPos = spawnPoint.position;
            spawnPos.y = 0f;

            GameObject spawnedTrap = Instantiate(trap, spawnPos, spawnPoint.rotation, transform);
            spawnedTrap.transform.localScale = Vector3.one;
        }
    }

}
