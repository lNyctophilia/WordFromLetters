using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Localization.Settings;
using UnityEngine.UI;

public class GetLetter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text letterText;
    [SerializeField] private Button restartButton;

    [Header("Localization")]
    [SerializeField] private LocalizedStringVar localizedStartingText = new LocalizedStringVar("Key_Starting");
    [SerializeField] private LocalizedStringVar localizedAlphabetText = new LocalizedStringVar("Key_Alphabet");
    [SerializeField] private string startingText;

    [Header("Settings")]
    [SerializeField] private List<string> letters = new List<string>();
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
            Debug.LogError("References not assigned");
            return;
        }

        localizedAlphabetText.Get((value) =>
        {
            letters = StringToList(value);
        });
        localizedStartingText.Get((value) =>
        {
            startingText = value;
        });

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
        LocalizationSettings.SelectedLocaleChanged += OnLocaleChanged;
    }
    private void UnsubscribeEvents()
    {
        GameManager.OnGameStateChanged -= Initialize;
        GameManager.OnGameStateChanged -= StartGame;
        Answer.OnPlayerAnswered -= Answered;
        Score.OnGameFinished -= OnGameFinished;
        LocalizationSettings.SelectedLocaleChanged -= OnLocaleChanged;
    }
    private void OnLocaleChanged(UnityEngine.Localization.Locale locale)
    {
        localizedAlphabetText.Get((value) =>
        {
            letters = StringToList(value);
        });
        localizedStartingText.Get((value) =>
        {
            startingText = value;
        });
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
        letterText.text = "3...";
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "2...";
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "1...";
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = startingText;
        yield return new WaitForSecondsRealtime(waitingDuration);
        int randomChoice = UnityEngine.Random.Range(0, letters.Count);
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
    private List<string> StringToList(string alphabet)
    {
        List<string> result = new List<string>();
        foreach (char c in alphabet)
        {
            result.Add(c.ToString());
        }
        return result;
    }
}