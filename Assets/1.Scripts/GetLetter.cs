using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class GetLetter : MonoBehaviour
{
    [Header("References")]
    [SerializeField] public Text letterText;
    [SerializeField]
    private string[] letters = { "A" , "B" , "C", "Ç" , "D", "E", "F", "G",
                                                  "H", "I", "İ" , "J", "K", "L", "M", "N",
                                                  "O", "Ö" , "P", "R", "S", "Ş" , "T", "U", "Ü" ,
                                                  "V", "Y", "Z" };

    [SerializeField] private PlayerClick playerClick;
    [SerializeField] public GameObject RestartButton;

    [Header("Settings")]
    [SerializeField] private float waitingDuration = 1f;

    public static GetLetter Instance;

    private void Awake()
    {
        Instance = this;

        if (letterText == null)
        {
            Debug.LogError("LetterText tanımlanmamış");
            letterText = GameObject.Find("LetterText").GetComponent<Text>();
        }
        if (playerClick == null)
        {
            Debug.LogError("PlayerClick tanımlanmamış");
            playerClick = GameObject.Find("PlayerClick").GetComponent<PlayerClick>();
        }

        if (letterText) letterText.text = "Waiting...";
    }
    public void StartGame()
    {
        if (Score.Instance.scorePlayer1 >= Score.Instance.scoreToWin || Score.Instance.scorePlayer2 >= Score.Instance.scoreToWin)
        {
            Score.Instance.ResetScore();
            Score.Instance.FinishGame();
        }
        else
        {
            StartCoroutine(StartGameCoroutine());
        }
    }

    private IEnumerator StartGameCoroutine()
    {
        playerClick.SetEnablePressedText(false);
        RestartButton.SetActive(false);
        Score.Instance.scoreTable.SetActive(true);

        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "3...";
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "2...";
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "1...";
        yield return new WaitForSecondsRealtime(waitingDuration);
        letterText.text = "Start";
        yield return new WaitForSecondsRealtime(waitingDuration);
        int randomInt = Random.Range(0, letters.Length);
        letterText.text = letters[randomInt];

        playerClick.SetEnableButtons(true);
        RestartButton.SetActive(false);
    }
}
