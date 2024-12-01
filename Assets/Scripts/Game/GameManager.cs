using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isGameOn = true;
    [SerializeField] private bool isFighting = false;
    [SerializeField] private bool isGameOver = false;

    public bool IsGameOn => isGameOn;
    public bool IsFighting => isFighting;
    public bool IsGameOver => isGameOver;

    public static GameManager Instance { get; private set; }

    public enum GameState
    {
        Menu,
        Game,
        Pause,
        Death,
        Controls,
        Credits,
        Fight,
        GameOver
    }

    public GameState CurrentState { get; private set; } = GameState.Game;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetGameState(GameState newState)
    {
        CurrentState = newState;
    }

    public void StartFighting()
    {
        isFighting = true;
        CurrentState = GameState.Fight;
    }

    public void StopFighting()
    {
        isFighting = false;
        CurrentState = GameState.Game;
    }
}