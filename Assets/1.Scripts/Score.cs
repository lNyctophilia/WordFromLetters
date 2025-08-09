using UnityEngine;
using UnityEngine.UI;

public class Score : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Text scoreText;
    [SerializeField] public GameObject scoreTable;

    [Header("Settings")]
    [SerializeField] public int scorePlayer1 = 0;
    [SerializeField] public int scorePlayer2 = 0;
    [SerializeField] public int scoreToWin = 6;

    public static Score Instance;

    void Awake()
    {
        Instance = this;
        scoreTable.SetActive(false);
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        scoreText.text = $"{scorePlayer1} : {scorePlayer2}";
    }

    public void SetScore(string PlayerName)
    {
        scoreTable.SetActive(true);

        if (PlayerName == "Player1")
        {
            scorePlayer1++;
        }
        else if (PlayerName == "Player2")
        {
            scorePlayer2++;

        }
        scoreText.text = $"{scorePlayer1} : {scorePlayer2}";

        if (scorePlayer1 >= scoreToWin)
        {
            PlayerClick.Instance.player1PressedText.gameObject.SetActive(true);
            PlayerClick.Instance.player1PressedText.text = "Mavi Kazandı!";
        }
        else if (scorePlayer2 >= scoreToWin)
        {
            PlayerClick.Instance.player2PressedText.gameObject.SetActive(true);
            PlayerClick.Instance.player2PressedText.text = "Kırmızı Kazandı!";
        }
        else Debug.Log("Hata burda");

        Answer.Instance.SetButtonsActive(false);
        GetLetter.Instance.RestartButton.gameObject.SetActive(true);
    }
    public void ResetScore()
    {
        scoreTable.SetActive(false);
        scorePlayer1 = 0;
        scorePlayer2 = 0;
        scoreText.text = $"{scorePlayer1} : {scorePlayer2}";
        PlayerClick.Instance.SetEnablePressedText(false);
    }
    public void FinishGame()
    {
        scoreTable.SetActive(false);
        GetLetter.Instance.letterText.text = "Waiting...";
        GetLetter.Instance.RestartButton.gameObject.SetActive(true);
        ResetScore();
    }
}