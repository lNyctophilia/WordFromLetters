using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerClick : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Text player1PressedText;
    [SerializeField] public Text player2PressedText;

    [Space(10)]

    [SerializeField] private Button player1Button;
    [SerializeField] private Button player2Button;

    [Header("Settings")]
    [SerializeField] private float textAnimDuration = 0.3f;
    [SerializeField] private Vector3[] textScales = { Vector3.one, Vector3.one * 1.2f };

    public static event Action<string> OnPlayerClick;

    public static PlayerClick Instance;

    private void Awake()
    {
        Initialize(GameState.Menu);
    }
    private void Initialize(GameState state)
    {
        if (state != GameState.Menu) return;

        Instance = this;

        if (!player1PressedText || !player2PressedText || !player1Button || !player2Button)
        {
            Debug.LogError("Referanslar tanımlanmamış");
            return;
        }

        SetButtonOnClicks();
        SetEnableButtons(false);
        AnimateTexts(true);
        SetEnablePressedTexts(false, "All");

        player1PressedText.text = "Mavi Bastı!";
        player2PressedText.text = "Kırmızı Bastı!";
    }


    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnubscribeEvents();


    private void SubscribeEvents()
    {
        GameManager.OnGameStateChanged += Initialize;
        GetLetter.OnGameRestarted += RestartGame;
        GetLetter.OnLetterSelected += OnLetterSelected;
        Score.OnGameFinished += RestartGame;
    }
    private void UnubscribeEvents()
    {
        GameManager.OnGameStateChanged -= Initialize;
        GetLetter.OnGameRestarted -= RestartGame;
        GetLetter.OnLetterSelected -= OnLetterSelected;
        Score.OnGameFinished -= RestartGame;
    }


    private void OnLetterSelected()
    {
        SetEnableButtons(true);
    }
    private void ClickButton(string player)
    {
        SetEnablePressedTexts(true, player);
        SetEnableButtons(false);

        OnPlayerClick?.Invoke(player);
    }
    private void RestartGame()
    {
        SetEnablePressedTexts(false, "All");
    }


    // ------------------------- Yardımcılar ---------------------------- //

    private void SetButtonOnClicks()
    {
        player1Button.onClick.RemoveAllListeners();
        player2Button.onClick.RemoveAllListeners();

        player1Button.onClick.AddListener(() => ClickButton("Player1"));
        player2Button.onClick.AddListener(() => ClickButton("Player2"));
    }
    private void SetEnableButtons(bool state)
    {
        player1Button.gameObject.SetActive(state);
        player2Button.gameObject.SetActive(state);
    }
    private void SetEnablePressedTexts(bool state, string playerName)
    {
        switch (playerName)
        {
            case "Player1":
                player1PressedText.gameObject.SetActive(state);
                player2PressedText.gameObject.SetActive(!state);
                break;
            case "Player2":
                player1PressedText.gameObject.SetActive(!state);
                player2PressedText.gameObject.SetActive(state);
                break;
            case "All":
                player1PressedText.gameObject.SetActive(state);
                player2PressedText.gameObject.SetActive(state);
                break;
        }
    }
    private void AnimateTexts(bool animate)
    {
        Animating(player1PressedText, animate);
        Animating(player2PressedText, animate);
    }
    private void Animating(Text text, bool animate)
    {
        if (animate)
            LeanTween.scale(text.rectTransform, textScales[1], textAnimDuration).setLoopPingPong();
        else
        {
            LeanTween.cancel(text.rectTransform.gameObject);
            text.rectTransform.localScale = textScales[0];
        }
    }
}
