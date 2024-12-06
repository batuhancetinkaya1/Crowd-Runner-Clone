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
        if (GameManager.Instance.CurrentState == GameManager.GameState.Game)
        {
            // Crowd Distribution
            playerCrowdSystemControl.CrowdDistribution(GameManager.GameState.Game);
            playerCrowdSystemControl.ModifyGoldenAngle(); // Angle Changer
            //tileManager.TileSpawner();

            // Player Move
            playerInputControl.NormalRun();
            PlayerInputHandler();
            playerDetection.DetectObjects();
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameState.FightPrep)
        {
            // Distribute crowd for fight formation
            playerCrowdSystemControl.CrowdDistribution(GameManager.GameState.FightPrep);
            playerFightHandler.FightPrep();
            //enemyFightHandler.FightPrep();
            StartCoroutine(FightTransaction());
        }
        else if(GameManager.Instance.CurrentState == GameManager.GameState.Fight)
        {
            enemyFightHandler.FightEnemy();
            playerFightHandler.FightPlayer();
        }
    }

    private IEnumerator FightTransaction()
    {
        GameManager.Instance.SetGameState(GameManager.GameState.Fight);
        yield return new WaitForSeconds(3f);
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