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
    void Start()
    {
        lastEndPoint = first.transform.Find("EndPoint");
        Debug.Log(lastEndPoint);
        if (lastEndPoint == null)
        {
            Debug.LogError($"{first.name} thiếu EndPoint – kiểm tra lại prefab!");
            return;
        }

        // Tạo thêm các đoạn nối tiếp
        for (int i = 0; i < 20; i++)
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
        GameObject next = Instantiate(prefab);

        Transform startPoint = next.transform.Find("StartPoint");
        if (startPoint != null)
        {
            // Nếu có StartPoint → căn chính xác
            Vector3 offset = next.transform.position - startPoint.position;
            next.transform.position = lastEndPoint.position + offset;
        }
        else
        {
            // Nếu không có → gán luôn vị trí EndPoint của trước
            next.transform.position = lastEndPoint.position;
        }

        lastEndPoint = next.transform.Find("EndPoint");

        if (lastEndPoint == null)
        {
            Debug.LogError($"{prefab.name} thiếu EndPoint");
        }
    }

}

