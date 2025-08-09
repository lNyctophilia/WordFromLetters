using UnityEngine;
using UnityEngine.UI;

public class Answer : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text letterText;
    [SerializeField] private Button correctButton;
    [SerializeField] private Button wrongButton;

    public static Answer Instance;

    private void Awake()
    {
        Instance = this;
    }

    public void SetButtonsActive(bool active)
    {
        correctButton.gameObject.SetActive(active);
        wrongButton.gameObject.SetActive(active);
    }
    public void ButtonClick(string buttonName)
    {
        string pressedPlayer = PlayerClick.Instance.pressedPlayer;
        string unpressedPlayer = PlayerClick.Instance.unpressedPlayer;

        PlayerClick.Instance.SetEnablePressedText(false);

        if (buttonName == "correct")
        {
            Score.Instance.SetScore(pressedPlayer);
        }
        else if (buttonName == "wrong")
        {
            Score.Instance.SetScore(unpressedPlayer);
        }
    }
}
