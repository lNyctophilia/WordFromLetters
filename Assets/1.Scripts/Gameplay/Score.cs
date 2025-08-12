using System;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text scoreText;
    [SerializeField] private GameObject winScreen;
    [SerializeField] private Text winText;
    [SerializeField] private Button homeButton;
    [SerializeField] private GameObject scoreTable;

    [Header("Localization")]
    [SerializeField] private LocalizedStringVar localizedWinTextBlue = new LocalizedStringVar("Key_BlueWin");
    [SerializeField] private LocalizedStringVar localizedWinTextRed = new LocalizedStringVar("Key_RedWin");

    [Header("Settings")]
    [SerializeField] private int scorePlayer1 = 0;
    [SerializeField] private int scorePlayer2 = 0;
    [SerializeField] public int scoreToWin = 6;

    public static event Action OnHomeButtonClicked;
    public static event Action OnGameFinished;
    public static event Action OnPlayerScored;

    public static Score Instance;

    private void Awake()
    {
        Initialize(GameState.Menu);
    }
    private void Initialize(GameState state)
    {
        if (state != GameState.Menu) return;

        Instance = this;

        if (!scoreText || !winScreen || !winText || !scoreTable || !homeButton)
        {
            Debug.LogError("References not assigned");
            return;
        }

        SetOnClickEvents();

        scoreTable.SetActive(false);
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        scoreText.text = $"{scorePlayer1} : {scorePlayer2}";
    }

    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnubscribeEvents();


    private void SubscribeEvents()
    {
        GameManager.OnGameStateChanged += Initialize;
        Answer.OnPlayerAnswered += SetScore;
    }
    private void UnubscribeEvents()
    {
        GameManager.OnGameStateChanged -= Initialize;
        Answer.OnPlayerAnswered -= SetScore;
    }


    private void SetScore(string player, bool isCorrect)
    {
        scoreTable.SetActive(true);

        if (isCorrect)
        {
            UpdateScore(player);
        }
        else
        {
            UpdateScore(player == "Player1" ? "Player2" : "Player1");
        }

        SetScoreText();

        if (scorePlayer1 >= scoreToWin || scorePlayer2 >= scoreToWin)
        {
            EndGame();
        }
        else OnPlayerScored?.Invoke();
    }
    private void EndGame()
    {
        winScreen.gameObject.SetActive(true);
        scoreTable.SetActive(false);
        if (scorePlayer1 >= scoreToWin) localizedWinTextBlue.Get((value) => winText.text = value);
        else localizedWinTextRed.Get((value) => winText.text = value);

        OnGameFinished?.Invoke();
    }
    private void SetScoreText()
    {
        scoreText.text = $"{scorePlayer1} : {scorePlayer2}";
    }
    private void UpdateScore(string player)
    {
        if(player == "Player1") scorePlayer1++;
        else if (player == "Player2") scorePlayer2++;
    }
    private void ResetScore()
    {
        scoreTable.SetActive(false);
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        SetScoreText();
    }
    private void SetOnClickEvents()
    {
        homeButton.onClick.RemoveAllListeners();
        homeButton.onClick.AddListener(() =>
        {
            ResetScore();
            winScreen.gameObject.SetActive(false);
            OnHomeButtonClicked?.Invoke();
        });
    }
}