using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isGameOn = true;
    [SerializeField] private bool isFighting = false;
    [SerializeField] private bool isGameOver = false; 
    public bool IsGameOn => isGameOn;
    public bool IsFighting => IsFighting;
    public bool IsGameOver => IsGameOver;
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
}
