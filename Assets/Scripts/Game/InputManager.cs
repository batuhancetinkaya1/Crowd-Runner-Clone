using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInputControl playerInputControl;
    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystemControl;
    [SerializeField] private PlayerDetection playerDetection;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private EnemyFightHandler enemyFightHandler;
    [SerializeField] private PlayerFightHandler playerFightHandler;

    private void Awake()
    {
        playerCrowdSystemControl.CrowdCounterTextUpdater();
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOn && GameManager.Instance.CurrentState == GameManager.GameState.Game)
        {
            // Crowd Distribution
            playerCrowdSystemControl.CrowdDistribution(GameManager.GameState.Game);
            playerCrowdSystemControl.ModifyGoldenAngle(); // Angle Changer
            //tileManager.TileSpawner();

            // Player Move
            playerInputControl.NormalRun();
            PlayerInputHandler();
            playerDetection.DetectDoor();
        }
        else if (GameManager.Instance.IsFighting && GameManager.Instance.CurrentState == GameManager.GameState.Fight)
        {
            // Distribute crowd for fight formation
            playerCrowdSystemControl.CrowdDistribution(GameManager.GameState.Fight);

            enemyFightHandler.FightEnemy();
            playerFightHandler.FightPlayer();
        }
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