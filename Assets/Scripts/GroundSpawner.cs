using System.Collections;
using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public static GroundSpawner Instance;
    public GameObject[] paths;
    private Transform lastEndPoint;
    [SerializeField] GameObject first;

    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }

      
    }
    private IEnumerator Start()
    {
        Debug.Log("[GroundSpawner] Start called.");

        // Đợi ObjectPool khởi tạo xong
        yield return null;

        lastEndPoint = first.transform.Find("EndPoint");
        Debug.Log(lastEndPoint);

        if (lastEndPoint == null)
        {
            Debug.LogError($"{first.name} thiếu EndPoint – kiểm tra lại prefab!");
            yield break;
        }

        for (int i = 0; i < 5; i++)
        {
            SpawnNextPath();
        }
    }

    public void SpawnInitialPath()
    {
        lastEndPoint = first.transform.Find("EndPoint");
        if (lastEndPoint == null)
            Debug.Log($"{first.name} thiếu EndPoint – kiểm tra lại prefab!");
    }

    GameObject RandomPath() => paths[Random.Range(0, paths.Length)];

    public void SpawnNextPath()
    {
        GameObject prefab = RandomPath();
        string tag = prefab.name;
        GameObject next = ObjectPool.Instance.SpawnFromPool(tag, Vector3.zero, Quaternion.identity);
        Transform startPoint = next.transform.Find("StartPoint");

        if (startPoint != null)
        {
            // Lấy offset tương đối đúng
            Vector3 offset = next.transform.position - startPoint.position;
            next.transform.position = lastEndPoint.position + offset;
        }
        else
        {
            next.transform.position = lastEndPoint.position;
        }

        lastEndPoint = next.transform.Find("EndPoint");

        if (lastEndPoint == null)
        {
            Debug.LogError($"{prefab.name} thiếu EndPoint");
        }
    }
}

