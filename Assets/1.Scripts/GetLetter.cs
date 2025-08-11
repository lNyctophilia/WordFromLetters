using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GetLetter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text letterText;
    [SerializeField] private Button restartButton;

    [Header("Settings")]
    [SerializeField] private string[] letters;
    [SerializeField] private float waitingDuration = 0.6f;

    public static event Action OnLetterSelected;
    public static event Action OnGameRestarted;
    public static GetLetter Instance;

    private void Awake()
    {
        Initialize(GameState.Menu);
    }
    private void Initialize(GameState state)
    {
        if (state != GameState.Menu) return;

        Instance = this;

        if (!letterText || !restartButton)
        {
            Debug.LogError("Referanslar tanımlanmamış");
            return;
        }

        string turkishAlphabet = "ABCÇDEFGĞHIİJKLMNOÖPRSŞTUÜVYZ";
        letters = new string[turkishAlphabet.Length];

        for (int i = 0; i < turkishAlphabet.Length; i++)
        {
            letters[i] = turkishAlphabet[i].ToString();
        }

        letterText.text = "Waiting...";

        SetButtonOnClicks();
    }


    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnsubscribeEvents();


    private void SubscribeEvents()
    {
        GameManager.OnGameStateChanged += Initialize;
        GameManager.OnGameStateChanged += StartGame;
        Answer.OnPlayerAnswered += Answered;
        Score.OnGameFinished += OnGameFinished;
    }
    private void UnsubscribeEvents()
    {
        GameManager.OnGameStateChanged -= Initialize;
        GameManager.OnGameStateChanged -= StartGame;
        Answer.OnPlayerAnswered -= Answered;
        Score.OnGameFinished -= OnGameFinished;
    }


    private void StartGame(GameState state)
    {
        if (state != GameState.Playing) return;

        restartButton.gameObject.SetActive(false);

        StopCoroutine(StartGameCoroutine());
        StartCoroutine(StartGameCoroutine());
    }
    private IEnumerator StartGameCoroutine()
    {
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "3...";
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "2...";
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "1...";
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "Start";
        yield return new WaitForSecondsRealtime(waitingDuration);
        int randomChoice = UnityEngine.Random.Range(0, letters.Length);
        letterText.text = letters[randomChoice];

        OnLetterSelected?.Invoke();
    }
    private void Answered(string player, bool isCorrect)
    {
        restartButton.gameObject.SetActive(true);
    }
    private void OnGameFinished()
    {
        restartButton.gameObject.SetActive(false);
    }
    private void SetButtonOnClicks()
    {
        restartButton.onClick.RemoveAllListeners();
        restartButton.onClick.AddListener(() =>
        {
            StartGame(GameState.Playing);
            OnGameRestarted?.Invoke();
        });
    }
}
