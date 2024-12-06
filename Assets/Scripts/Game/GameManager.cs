using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private bool isGameOn = true;
    [SerializeField] private bool isFighting = false;
    [SerializeField] private bool isGameOver = false;

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
        CurrentState = newState;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            SetGameState(GameState.FightPrep);
        }
        else if (Input.GetKeyDown(KeyCode.B))
        {
            SetGameState(GameState.Game);
        }
        else if (Input.GetKeyDown(KeyCode.A))
        {
            SetGameState(GameState.Fight);
        }
    }
}