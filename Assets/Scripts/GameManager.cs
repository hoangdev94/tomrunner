using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI topDistanceText;
    [SerializeField] private GameObject gameoverUI;

    private int totalScore = 0;
    private int currentScore = 0;
    private int topDistance = 0;
    private bool isGameover = false;

    public bool IsGameOver() => isGameover;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Start()
    {
        topDistance = PlayerPrefs.GetInt("TopDistance", 0);
        UpdateTopDistance();
        FindUIReferences(); 
    }
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainGame")
        {
            FindUIReferences();
        }
    }
    private void FindUIReferences()
    {
        if (scoreText == null)
        {
            var obj = GameObject.Find("ScoreText");
            if (obj) scoreText = obj.GetComponent<TextMeshProUGUI>();
        }

        if (distanceText == null)
        {
            var obj = GameObject.Find("DistanceText");
            if (obj) distanceText = obj.GetComponent<TextMeshProUGUI>();
        }

        if (topDistanceText == null)
        {
            var obj = GameObject.Find("TopDistanceText");
            if (obj) topDistanceText = obj.GetComponent<TextMeshProUGUI>();
        }

        if (gameoverUI == null)
        {
            var obj = GameObject.Find("GameOverUI");
            if (obj)
            {
                gameoverUI = obj;
                gameoverUI.SetActive(false);
            }
            
        }
    }
    private void Update()
    {
        UpdateScoreText();
    }

    public void AddScore(int score)
    {
        currentScore += score;
        Debug.Log("Score: " + currentScore);
    }

    public void UpdateRoadDistance(float distance)
    {
        int dist = Mathf.FloorToInt(distance);

        if (distanceText != null)
            distanceText.text = $"{dist} m";

        if (dist > topDistance)
        {
            topDistance = dist;
            PlayerPrefs.SetInt("TopDistance", topDistance);
            PlayerPrefs.Save();
            UpdateTopDistance();
        }
    }

    public void UpdateTopDistance()
    {
        if (topDistanceText != null)
            topDistanceText.text = $"{topDistance} m";
    }

    public void GameOver()
    {
        isGameover = true;
        totalScore += currentScore;

        if (gameoverUI != null)
            gameoverUI.SetActive(true);
        UpdateTopDistance();
    }
   
    public void RestartGame()
    {
        isGameover = false;
        currentScore = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainGame");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("MainGame");
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        isGameover = false;
        SceneManager.LoadScene("MenuGame");
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
    }


    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
}
