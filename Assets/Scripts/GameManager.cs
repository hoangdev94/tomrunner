using TMPro;
using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; set; }

    private int totalScore;
    private int currentScore = 0;
    [SerializeField]  TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI distance;

    [SerializeField] GameObject gameoverUI;
    public bool isGameover = false;

    void Start()
    {
        if(Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }

        
    }

    // Update is called once per frame
    void Update()
    {
        
        UpdateScore();
    }
    public void AddScore(int score)
    {
        currentScore += score;
        Debug.Log(currentScore);
    }
    void UpdateScore()
    {
        scoreText.text = currentScore.ToString();
    }
    public void UpdateRoadText(float currentDistance)
    {
        distance.text = Mathf.FloorToInt(currentDistance).ToString() + " m";
    }
    public void GameOver()
    {
        isGameover = true;
        totalScore += currentScore;
        if (gameoverUI != null) gameoverUI.SetActive(true);
        StartCoroutine(StopGameAfterGameOver(3));
    }
    public bool IsGameOver()
    {
        return isGameover;
    }
    public void RestartGame()
    {
        isGameover = false;
        currentScore = 0;
        UpdateScore();
        Time.timeScale = 1;
        SceneManager.LoadScene("MainGame");

    }
    private IEnumerator StopGameAfterGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        Time.timeScale = 0;
    }

    public void GoToMenu()
    {
        Time.timeScale = 1;
        isGameover = false;
        SceneManager.LoadScene("Menu");
    }

}
