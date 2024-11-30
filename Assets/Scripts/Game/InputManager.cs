using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private PlayerInputControl playerInputControl;
    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystemComtrol;
    [SerializeField] private PlayerDetection playerDetection;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private EnemyFightHandler enemyFightHandler;
    [SerializeField] private PlayerFightHandler playerFightHandler;
    private void Awake()
    {
        playerCrowdSystemComtrol.CrowdCounterTextUpdater();
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOn && GameManager.Instance.CurrentState == GameManager.GameState.Game)
        {
            //Crowd Distrubitoun
            playerCrowdSystemComtrol.CrowdDistribution(GameManager.GameState.Game);
            playerCrowdSystemComtrol.ModifyGoldenAngle(); //AngleChanger
            tileManager.TileSpawner();

            //Player Move
            playerInputControl.NormalRun();
            PlayerInputHandler();
            playerDetection.DetectDoor();
        }
        else if(GameManager.Instance.IsFighting && GameManager.Instance.CurrentState == GameManager.GameState.Fight)
        {
            playerCrowdSystemComtrol.FightingCrowdDistrubition();
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
        else if(Input.GetMouseButton(0))
        {
            playerInputControl.LineChangeDemandHandler();
        }
        else if(Input.GetMouseButtonUp(0))
        {
            playerInputControl.LineChangeDemandFinisher();
        }
    }
}
