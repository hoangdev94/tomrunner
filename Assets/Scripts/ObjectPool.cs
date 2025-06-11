using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    [System.Serializable]
    public class Pool
    {
        public string tag;
        public GameObject prefab;
        public int size;
    }

    public static ObjectPool Instance { get; private set; }
    public static bool IsReady { get; private set; } = false;

    [Header("Pool Configuration")]
    public List<Pool> pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            poolDictionary = new Dictionary<string, Queue<GameObject>>();
            Debug.Log("<color=cyan>[ObjectPool]</color> Awake: Instance set.");
        }
        else
        {
            Debug.LogWarning("<color=yellow>[ObjectPool]</color> Duplicate instance, destroying this one.");
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        Debug.Log("<color=cyan>[ObjectPool]</color> Start: Initializing pools...");
        foreach (Pool pool in pools)
        {
            if (pool.prefab == null)
            {
                Debug.LogError($"<color=red>[ObjectPool]</color> Pool '{pool.tag}' has NULL prefab!");
                continue;
            }

            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.size; i++)
            {
                GameObject obj = Instantiate(pool.prefab);
                obj.name = pool.tag;
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                objectPool.Enqueue(obj);
            }

            poolDictionary[pool.tag] = objectPool;
            Debug.Log($"<color=green>[ObjectPool]</color> Pool '{pool.tag}' initialized with {pool.size} objects.");
        }

        IsReady = true;
    }

    public GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"<color=red>[ObjectPool]</color> Spawn failed: Pool with tag '{tag}' not found!");
            return null;
        }

        Queue<GameObject> poolQueue = poolDictionary[tag];
        GameObject objectToSpawn = null;

        for (int i = 0; i < poolQueue.Count; i++)
        {
            objectToSpawn = poolQueue.Dequeue();

            if (objectToSpawn == null)
            {
                Debug.LogWarning($"<color=orange>[ObjectPool]</color> Null object in pool '{tag}'");
                continue;
            }

            if (!objectToSpawn.activeInHierarchy)
            {
                objectToSpawn.transform.position = position;
                objectToSpawn.transform.rotation = rotation;
                objectToSpawn.SetActive(true);

                poolQueue.Enqueue(objectToSpawn);
                Debug.Log($"<color=lime>[ObjectPool]</color> Spawned '{tag}' at {position}");
                return objectToSpawn;
            }

            // Still in use, re-enqueue
            poolQueue.Enqueue(objectToSpawn);
        }

        // All objects are active → auto-expand
        Debug.LogWarning($"<color=yellow>[ObjectPool]</color> Pool '{tag}' exhausted. Instantiating new object.");

        Pool poolConfig = pools.Find(p => p.tag == tag);
        if (poolConfig != null)
        {
            GameObject newObj = Instantiate(poolConfig.prefab, position, rotation);
            newObj.name = tag;
            newObj.transform.SetParent(transform);
            newObj.SetActive(true);
            poolQueue.Enqueue(newObj);

            Debug.Log($"<color=lime>[ObjectPool]</color> Auto-expanded pool '{tag}' with new object.");
            return newObj;
        }

        Debug.LogError($"<color=red>[ObjectPool]</color> No pool config found to expand for tag '{tag}'");
        return null;
    }

    public void ReturnToPool(GameObject obj)
    {
        if (obj == null)
        {
            Debug.LogError("[ObjectPool] Cannot return a NULL object to pool.");
            return;
        }

        string tag = obj.name.Replace("(Clone)", "").Trim();

        // Nếu object có dấu (1), (2)... → làm sạch
        int bracketIndex = tag.IndexOf('(');
        if (bracketIndex > 0)
            tag = tag.Substring(0, bracketIndex).Trim();

        if (!poolDictionary.ContainsKey(tag))
        {
            Debug.LogError($"[ObjectPool] Return failed: Tag '{tag}' not found in pool.");
            return;
        }

        obj.SetActive(false);
        obj.name = tag; // Đặt lại đúng tên
        obj.transform.SetParent(transform);
        poolDictionary[tag].Enqueue(obj);
    }
    public int CountAvailableObjects(string tag)
    {
        if (!poolDictionary.ContainsKey(tag)) return 0;

        int count = 0;
        foreach (var obj in poolDictionary[tag])
        {
            if (!obj.activeInHierarchy) count++;
        }
        return count;
    }

    public int CountActiveObjects(string tag)
    {
        if (!poolDictionary.ContainsKey(tag)) return 0;

        int activeCount = 0;
        foreach (var obj in poolDictionary[tag])
        {
            if (obj.activeInHierarchy)
                activeCount++;
        }
        return activeCount;
    }

}
