using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [SerializeField] private Text _coinText;
    [SerializeField] private GameObject _gameOverWindow;
    [SerializeField] private GameObject _pauseWindow;

    private static GameManager _instance;
    private PlayerController _player;
    private bool _gameStarted;

    private void Awake()
    {
        _instance = this;
        _player = FindObjectOfType<PlayerController>();
    }
    
    private void Start()
    {
        var gameData = GameData.Load();
        if (gameData != null)
        {
            _coinText.text = gameData.CoinCount.ToString();
        }

        _gameOverWindow.transform.Find("RetryBtn").GetComponent<Button>().onClick.AddListener(() => SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex));
        _pauseWindow.transform.Find("ResumeBtn").GetComponent<Button>().onClick.AddListener(ResumeGame);
        _pauseWindow.transform.Find("MainMenuBtn").GetComponent<Button>().onClick.AddListener(() =>
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(0);
        });
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !_gameOverWindow.activeSelf)
        {
            if (Time.timeScale == 0)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
    }

    public static void AddCoin()
    {
        _instance._coinText.text = (int.Parse(_instance._coinText.text) + 1).ToString();
    }

    public static void ShowGameOverWindow()
    {
        _instance._gameOverWindow.SetActive(true);
    }

    public static void GoToNextLevel()
    {
        var nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        var gameData = new GameData
        {
            SceneIndex = nextSceneIndex,
            CoinCount = int.Parse(_instance._coinText.text)
        };
        GameData.Save(gameData);
        SceneManager.LoadScene(nextSceneIndex);
    }

    public void StartGame()
    {
        _player.MoveEnabled = true;
        _gameStarted = true;
    }

    private void ResumeGame()
    {
        _pauseWindow.SetActive(false);
        _player.MoveEnabled = _gameStarted;
        Time.timeScale = 1;
    }

    private void PauseGame()
    {
        _pauseWindow.SetActive(true);
        _player.MoveEnabled = false;
        Time.timeScale = 0;
    }
}