using System;
using UnityEngine;
using UnityEngine.UI;

public class Answer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Button correctButton;
    [SerializeField] private Button wrongButton;

    [Header("Settings")]
    [SerializeField] private string pressedPlayer;

    public static event Action<string, bool> OnPlayerAnswered;

    public static Answer Instance;

    private void Awake()
    {
        Initialize(GameState.Menu);
    }
    private void Initialize(GameState state)
    {
        if (state != GameState.Menu) return;

        Instance = this;

        if (!correctButton || !wrongButton)
        {
            Debug.LogError("Referanslar tanımlanmamış");
            return;
        }

        SetButtonOnClicks();
        pressedPlayer = null;
    }


    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnubscribeEvents();


    private void SubscribeEvents()
    {
        GameManager.OnGameStateChanged += Initialize;
        PlayerClick.OnPlayerClick += OnPlayerClick;
    }
    private void UnubscribeEvents()
    {
        GameManager.OnGameStateChanged -= Initialize;
        PlayerClick.OnPlayerClick -= OnPlayerClick;
    }


    private void OnPlayerClick(string player)
    {
        pressedPlayer = player;
        SetButtonsActive(true);
    }
    private void ButtonClick(string buttonName)
    {
        if (pressedPlayer == null) return;

        if (buttonName == "Correct") OnPlayerAnswered?.Invoke(pressedPlayer, true);
        else if (buttonName == "Wrong") OnPlayerAnswered?.Invoke(pressedPlayer, false);

        SetButtonsActive(false);
    }


    // ------------------------ Yardımcılar --------------------------//


    private void SetButtonsActive(bool state)
    {
        correctButton.gameObject.SetActive(state);
        wrongButton.gameObject.SetActive(state);
    }
    private void SetButtonOnClicks()
    {
        correctButton.onClick.RemoveAllListeners();
        wrongButton.onClick.RemoveAllListeners();

        correctButton.onClick.AddListener(() => ButtonClick("Correct"));
        wrongButton.onClick.AddListener(() => ButtonClick("Wrong"));
    }
}
