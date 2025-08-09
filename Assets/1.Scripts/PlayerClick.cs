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
    [SerializeField] public string pressedPlayer;
    [SerializeField] public string unpressedPlayer;

    public static PlayerClick Instance;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        SetEnableButtons(false);
        AnimateText(player1PressedText);
        AnimateText(player2PressedText);
        player1PressedText.gameObject.SetActive(false);
        player2PressedText.gameObject.SetActive(false);
        player1PressedText.text = "Mavi Bastı!";
        player2PressedText.text = "Kırmızı Bastı!";
    }
    public void SetEnableButtons(bool enable)
    {
        player1Button.interactable = enable;
        player2Button.interactable = enable;
    }
    public void SetEnablePressedText(bool enable)
    {
        player1PressedText.gameObject.SetActive(enable);
        player2PressedText.gameObject.SetActive(enable);
    }
    public void ClickButton(string Player)
    {
        player1PressedText.text = "Mavi Bastı!";
        player2PressedText.text = "Kırmızı Bastı!";

        if (Player == "Player1")
        {
            player1PressedText.gameObject.SetActive(true);
            pressedPlayer = "Player1";
            unpressedPlayer = "Player2";
        }
        else if (Player == "Player2")
        {
            player2PressedText.gameObject.SetActive(true);
            pressedPlayer = "Player2";
            unpressedPlayer = "Player1";
        }

        Answer.Instance.SetButtonsActive(true);
        SetEnableButtons(false);
    }
    private void AnimateText(Text text)
    {
        if (text == null)
        {
            Debug.LogError("Text is null");
            return;
        }
        LeanTween.scale(text.rectTransform, textScales[1], 0.3f).setLoopPingPong();
    }
}
