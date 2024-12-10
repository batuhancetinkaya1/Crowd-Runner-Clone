using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInputControl playerInputControl;
    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystemControl;
    [SerializeField] private PlayerDetection playerDetection;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private EnemyFightHandler enemyFightHandler;
    [SerializeField] private PlayerFightHandler playerFightHandler;

    // Flag tanýmlamalarý


    public static InputManager Instance { get; private set; }

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            playerCrowdSystemControl.CrowdCounterTextUpdater();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void Update()
    {
        switch (GameManager.Instance.CurrentState)
        {
            case GameManager.GameState.Game:
                HandleGameState();
                break;
            case GameManager.GameState.FightPrep:
                HandleFightPrepState();
                break;
            case GameManager.GameState.Fight:
                HandleFightState();
                break;
            case GameManager.GameState.GameOver:
                HandleGameOverState();
                break;
        }
    }

    private void HandleGameOverState()
    {
        GameResetManager.Instance.ResetGame();
    }

    private void HandleGameState()
    {
        // Normal oyun state'i iþlemleri
        playerCrowdSystemControl.CrowdDistribution(GameManager.GameState.Game);
        playerCrowdSystemControl.ModifyGoldenAngle();

        playerInputControl.NormalRun();
        PlayerInputHandler();
        playerDetection.DetectObjects();

        //Enemy Part
        if(EnemyFightHandler.Instance.registeredEnemy != null)
        {
            EnemyFightHandler.Instance.registeredEnemy.CrowdDistribution(GameManager.GameState.Game);
            EnemyFightHandler.Instance.registeredEnemy.ModifyGoldenAngle();
            EnemyFightHandler.Instance.registeredEnemy.DetectObjects();
        }
    }

    private void HandleFightPrepState()
    {
        // Fight hazýrlýk state'i
        playerCrowdSystemControl.CrowdDistribution(GameManager.GameState.FightPrep);
        playerCrowdSystemControl.ModifyGoldenAngle();
        playerInputControl.NormalRun();
        playerFightHandler.FightPrep();

        //Enemy Part
        EnemyFightHandler.Instance.registeredEnemy.CrowdDistribution(GameManager.GameState.FightPrep);
        EnemyFightHandler.Instance.registeredEnemy.ModifyGoldenAngle();
        //EnemyFightHandler.Instance.FightPrep();

        // Fight transaction'ýný baþlat

        StartCoroutine(FightTransaction());

    }

    private void HandleFightState()
    {
        playerInputControl.NormalRun();
        EnemyFightHandler.Instance.registeredEnemy.MoveForward();
        playerFightHandler.CheckFightConditions();
    }

    private IEnumerator FightTransaction()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Fight);
        yield return new WaitForSeconds(2f);
    }

    private void PlayerInputHandler()
    {
        if (Input.GetMouseButtonDown(0))
        {
            playerInputControl.FirstClickHandler();
        }
        else if (Input.GetMouseButton(0))
        {
            playerInputControl.LineChangeDemandHandler();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            playerInputControl.LineChangeDemandFinisher();
        }
    }

}