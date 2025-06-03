using UnityEngine;

public class GroundSpawner : MonoBehaviour
{
    public static GroundSpawner Instance;
    public GameObject[] paths;
    private Transform lastEndPoint;
    private void Awake()
    {
        if(Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    void Start()
    {
        GameObject firstPath = Instantiate(paths[Random.Range(0, paths.Length)], Vector3.zero, Quaternion.identity);
        lastEndPoint = firstPath.transform.Find("EndPoint");

        
        for (int i = 1; i < 10; i++)
        {
            SpawnNextPath();
        }
    }

    public void SpawnInitialPath()
    {
        GameObject firstPath = Instantiate(paths[Random.Range(0, paths.Length)], Vector3.zero, Quaternion.identity);
        lastEndPoint = firstPath.transform.Find("EndPoint");
    }

    public void SpawnNextPath()
    {
        int randomIndex = Random.Range(0, paths.Length);
        GameObject newPath = Instantiate(paths[randomIndex], lastEndPoint.position, Quaternion.identity);
        lastEndPoint = newPath.transform.Find("EndPoint");
    }
}

