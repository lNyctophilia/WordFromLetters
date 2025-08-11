using System;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private GameState currentState;
    public static event Action<GameState> OnGameStateChanged;
    public static GameManager Instance;


    private void Awake()
    {
        Instance = this;

        ChangeGameState(GameState.Menu);
    }


    private void OnEnable() => SubscribeEvents();
    private void OnDisable() => UnubscribeEvents();


    private void SubscribeEvents()
    {
        Score.OnHomeButtonClicked += EndGame;
    }
    private void UnubscribeEvents()
    {
        Score.OnHomeButtonClicked -= EndGame;
    }


    private void EndGame()
    {
        ChangeGameState(GameState.Menu);
    }
    public void ChangeGameState(GameState state)
    {
        currentState = state;
        OnGameStateChanged?.Invoke(currentState);
    }
}

public enum GameState
{
    Menu,
    Playing,
}