using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI totalCoinText;
    [SerializeField] private TextMeshProUGUI distanceText;
    [SerializeField] private TextMeshProUGUI topDistanceText;
    [SerializeField] private TextMeshProUGUI itemsText;
    [SerializeField] private GameObject gameoverUI;
    [SerializeField] private GameObject pauseUI;
    [SerializeField] private GameObject itemsUI;
    [SerializeField] private Image timeMagnetBar;
    [SerializeField] private GameObject shopUI;
    [SerializeField] private GameObject settingUI;
    [SerializeField] private GameObject itemsButton;
    [SerializeField] private TextMeshProUGUI priceGold;
    [Header("Audio Settings")]
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private AudioSource musicAudio;
    [SerializeField] private AudioSource sfxAudio;

    private int totalCoin;
    private int currentScore;
    private int topDistance;
    private bool isGameover;
    private int magnetItems;

    private float timeMagnet = 10f;
    private float currentMagnetTime;
    private int magnetCount;

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
        musicSlider.value = musicAudio.volume;
        sfxSlider.value = sfxAudio.volume;
        musicSlider.onValueChanged.AddListener(SetVolumeBackGround);
        sfxSlider.onValueChanged.AddListener(SetVolumeSFX);
    }
    private void LoadPlayerPrefs()
    {
        topDistance = PlayerPrefs.GetInt("TopDistance", 0);
        totalCoin = PlayerPrefs.GetInt("TotalCoin", 0);
        magnetItems = PlayerPrefs.GetInt("MagnetItems", 0);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadPlayerPrefs();
        FindUIReferences();
        UpdateAllUI();
        UpdateItemsUI();
    }

    private void Update()
    {
        UpdateScoreText();
        UpdateMagnetEffect();
    }

    private void FindUIReferences()
    {
        scoreText = TryFindUI("ScoreText", scoreText);
        distanceText = TryFindUI("DistanceText", distanceText);
        itemsText = TryFindUI("ItemsCountText", itemsText);
        topDistanceText = TryFindUI("TopDistanceText", topDistanceText);
        totalCoinText = TryFindUI("Coin_Value", totalCoinText);
        priceGold = TryFindUI("PriceGold", priceGold);
        timeMagnetBar = TryFindUI("SliceTimeMagnet", timeMagnetBar);
        gameoverUI = TryFindAndInitPanel("GameOverUI", gameoverUI, false);
        pauseUI = TryFindAndInitPanel("Pause", pauseUI, false);
        musicAudio = TryFindUI("MusicBackground", musicAudio);
        sfxAudio = TryFindUI("SFX", sfxAudio);
        musicSlider = TryFindUI("AudiBackgroundSlider", musicSlider);
        sfxSlider = TryFindUI("AudioSFXSlider", sfxSlider);
        itemsUI = TryFindAndInitPanel("Items", itemsUI, false);
        shopUI = TryFindAndInitPanel("Shop", shopUI, false);
        settingUI = TryFindAndInitPanel("Settings", settingUI, false);
        itemsButton = TryFindAndInitPanel("ItemsButton", itemsButton, false);
    }

    private T TryFindUI<T>(string name, T current) where T : Component
    {
        return current != null ? current : GameObject.Find(name)?.GetComponent<T>();
    }

    private GameObject TryFindAndInitPanel(string name, GameObject current, bool active)
    {
        if (current != null) return current;
        var obj = GameObject.Find(name);
        if (obj) obj.SetActive(active);
        return obj;
    }

    private void UpdateAllUI()
    {
        UpdateTopDistance();
        UpdateCoinText();
    }

    public void AddScore(int score) => currentScore += score;

    public void AddMagnet(int magnet)
    {
        magnetCount += magnet;
        currentMagnetTime = timeMagnet;
        if (itemsUI != null) itemsUI.SetActive(true);
    }

    private void UpdateMagnetEffect()
    {
        if (magnetCount <= 0) return;
        if (CoinDetected.Instance != null) CoinDetected.Instance.gameObject.SetActive(true);

        currentMagnetTime -= Time.deltaTime;
        if (timeMagnetBar != null) timeMagnetBar.fillAmount = currentMagnetTime / timeMagnet;

        if (currentMagnetTime <= 0f)
        {
            magnetCount = 0;
            currentMagnetTime = 0f;
            if (CoinDetected.Instance != null) CoinDetected.Instance.gameObject.SetActive(false);
            if (itemsUI != null) itemsUI.SetActive(false);
        }
    }

    public void UpdateRoadDistance(float distance)
    {
        int dist = Mathf.FloorToInt(distance);
        if (distanceText != null) distanceText.text = $"{dist} m";

        if (dist > topDistance)
        {
            topDistance = dist;
            PlayerPrefs.SetInt("TopDistance", topDistance);
            PlayerPrefs.Save();
            UpdateTopDistance();
        }
    }

    private void UpdateTopDistance()
    {
        if (topDistanceText != null)
            topDistanceText.text = $"{topDistance} m";
    }

    private void UpdateScoreText()
    {
        if (scoreText != null)
            scoreText.text = currentScore.ToString();
    }

    private void UpdateCoinText()
    {
        if (totalCoinText != null)
            totalCoinText.text = totalCoin.ToString();
    }

    public void GameOver()
    {
        isGameover = true;
        totalCoin += currentScore;
        currentScore = 0;

        if (gameoverUI != null) gameoverUI.SetActive(true);

        PlayerPrefs.SetInt("TotalCoin", totalCoin);
        PlayerPrefs.Save();

        UpdateTopDistance();
    }

    public void RestartGame()
    {
        AudioManager.Instance.Touch();
        isGameover = false;
        currentScore = 0;
        Time.timeScale = 1;
        SceneManager.LoadScene("MainGame");
    }

    public void StartGame()
    {
        AudioManager.Instance.Touch();
        SceneManager.LoadScene("MainGame");
    }

    public void GoToMenu()
    {
        AudioManager.Instance.Touch();
        isGameover = false;
        Time.timeScale = 1;
        SceneManager.LoadScene("MenuGame");
    }

    public void Setting()
    {
        AudioManager.Instance.Touch();
        if (settingUI != null) settingUI.SetActive(true);
    }

    public void CloseSetting()
    {
        AudioManager.Instance.Touch();
        if (settingUI != null) settingUI.SetActive(false);
    }

    public void GoToShop()
    {
        AudioManager.Instance.Touch();
        if (shopUI != null)
        {
            Debug.Log("Shop opened");
            shopUI.SetActive(true);
            UpdateCoinText();
        }
    }

    public void BuyItems()
    {
        if (int.TryParse(priceGold.text, out int price))
        {
            if (totalCoin >= price)
            {
             
                totalCoin -= price;
                magnetItems++;
                PlayerPrefs.SetInt("MagnetItems", magnetItems);
                PlayerPrefs.Save();

                UpdateCoinText();
                UpdateItemsUI();
                Debug.Log("Item purchased!");
                AudioManager.Instance.BuyItems();
            }
            else
            {
                Debug.Log("Not enough coins.");
            }
        }
        else
        {
            Debug.Log("Invalid price format.");
        }
    }

    private void UpdateItemsUI()
    {
        if (itemsButton != null)
        {
            itemsButton.SetActive(magnetItems > 0);
            UpdateItemsText();
        }
    }

    private void UpdateItemsText()
    {
        if (itemsText != null)
            itemsText.text = magnetItems.ToString();
    }

    public void ShopToMenu()
    {
        AudioManager.Instance.Touch();
        if (shopUI != null) shopUI.SetActive(false);
    }

    public void UseItems()
    {
        AudioManager.Instance.PickMagnet();

        if (magnetItems <= 0)
        {
            
            return;
        }

        magnetItems--;
        PlayerPrefs.SetInt("MagnetItems", magnetItems);
        PlayerPrefs.Save();
        AddMagnet(1);
        UpdateItemsUI();

  
    }
    public void AddGold()
    {
        totalCoin += 1000;
        UpdateCoinText();
    }
   public void PauseGame()
    {
        pauseUI.SetActive(true);
        Time.timeScale = 0;
    }
    public void ContinueGame()
    {

        Time.timeScale = 1;
        pauseUI.SetActive(false);
    }
    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    public void SetVolumeBackGround(float volume)
    {
        musicAudio.volume = volume;
    }
    public void SetVolumeSFX(float volume)
    {
        sfxAudio.volume = volume;
    }
}