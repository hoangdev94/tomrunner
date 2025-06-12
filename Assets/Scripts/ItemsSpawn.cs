using System.Collections;
using UnityEngine;

public class ItemsSpawn : MonoBehaviour
{
    [Header("Prefabs/Tags")]
    public string coinTag = "Coin";
    public string magnetTag = "Magnet";
    public string[] trainTags;
    public string[] trapTags;

    [Header("Spawn Settings")]
    public bool spawnTrain = true;
    public bool spawnTrap = true;
    public Transform[] spawnPoints;

    [Header("Coin Settings")]
    public int minCoins = 2;
    public int maxCoins = 5;
    public float spacing = 2f;

    private static readonly int[] indexBuffer = { 0, 1, 2 };

    [SerializeField] private int maxActiveMagnets = 3;

    IEnumerator Start()
    {
        yield return new WaitForSeconds(0.1f);
        SpawnCoinTrainTrap();
    }
    void SpawnCoinTrainTrap()
    {
        if (spawnPoints.Length < 3 || ObjectPool.Instance == null) return;

        Shuffle(indexBuffer);

        for (int i = 0; i < 2; i++)
        {
            Transform spawnPoint = spawnPoints[indexBuffer[i]];
            float rand = Random.value;

            if (rand < 0.5f)
            {
                SpawnCoinLine(spawnPoint);
            }
            else if (rand < 0.75f && spawnTrain)
            {
                SpawnFromPoolRandomTag(trainTags, spawnPoint.position);
            }
            else if (rand < 0.99f && spawnTrap)
            {
                SpawnFromPoolRandomTag(trapTags, spawnPoint.position);
            }

            else 
            {
                if (ObjectPool.Instance.CountActiveObjects(magnetTag) < maxActiveMagnets)
                SpawnFromPoolByTag(magnetTag, spawnPoint.position);
            }
        }
    }



    void SpawnCoinLine(Transform spawnPoint)
    {
        int available = ObjectPool.Instance.CountAvailableObjects(coinTag);
        if (available <= 0) return;

        int coinCount = Mathf.Min(Random.Range(minCoins, maxCoins + 1), available);

        Vector3 start = spawnPoint.position;
        for (int i = 0; i < coinCount; i++)
        {
            Vector3 pos = start + new Vector3(0, 0, i * spacing);
            ObjectPool.Instance.SpawnFromPool(coinTag, pos, Quaternion.identity);
        }
    }

    void SpawnFromPoolRandomTag(string[] tags, Vector3 position)
    {
        if (tags == null || tags.Length == 0) return;
        position.y = 0;
        string tag = tags[Random.Range(0, tags.Length)];
        SpawnFromPoolByTag(tag, position);
    }

    void SpawnFromPoolByTag(string tag, Vector3 position)
    {
        if (string.IsNullOrEmpty(tag)) return;

        GameObject obj = ObjectPool.Instance.SpawnFromPool(tag, position, Quaternion.identity);
        if (obj != null)
        {
            obj.transform.localScale = Vector3.one;
        }
        else
        {
            Debug.LogWarning($"[ItemsSpawn] ObjectPool returned NULL for tag '{tag}' at position {position}");
        }
    }

    // Fisher–Yates shuffle
    void Shuffle(int[] array)
    {
        for (int i = array.Length - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (array[i], array[j]) = (array[j], array[i]);
        }
    }
}
