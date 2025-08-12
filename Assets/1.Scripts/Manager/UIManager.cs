using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private GameObject HubUI;
    [SerializeField] private GameObject GameUI;
    [SerializeField] private Button StartButton;

    void Awake()
    {
        StartButton.onClick.AddListener(() => GameManager.Instance.ChangeGameState(GameState.Playing));
    }

    void OnEnable()
    {
        GameManager.OnGameStateChanged += ShowGameUI;
        GameManager.OnGameStateChanged += ShowMenuUI;
    }
    void OnDisable()
    {
        GameManager.OnGameStateChanged -= ShowGameUI;
        GameManager.OnGameStateChanged -= ShowMenuUI;
    }

    private void ShowMenuUI(GameState state)
    {
        if(state != GameState.Menu) return;

        HubUI.SetActive(true);
        GameUI.SetActive(false);
    }
    private void ShowGameUI(GameState state)
    {
        if (state != GameState.Playing) return;

        GameUI.SetActive(true);
        HubUI.SetActive(false);
    }
}