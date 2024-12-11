using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isGameOn = true;
    [SerializeField] private bool isFighting = false;
    [SerializeField] private bool isGameOver = false;
    [SerializeField] private GameState gameState;

    public bool IsGameOn => isGameOn;
    public bool IsFightOn => isFighting;
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
        FightPrep,
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
        Debug.Log("state: " + newState+ " Come From");
        CurrentState = newState;
        gameState = newState;
    }
}