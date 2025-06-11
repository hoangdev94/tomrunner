using UnityEngine;

public class WaterPlaneSpawn : MonoBehaviour
{
    [SerializeField] GameObject waterPlane;
    private Transform lastEndPoint;
    public static WaterPlaneSpawn Instance;
    [SerializeField] GameObject first;
    private void Awake()
    {
        if (Instance == null)
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
        for (int i = 0; i < 2; i++)
        {
            SpawnNextWaterPlane();
        }

    }
    public void SpawnNextWaterPlane()
    {

        GameObject next = ObjectPool.Instance.SpawnFromPool("WaterPlane", Vector3.zero, Quaternion.identity);
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

    }

    
}
