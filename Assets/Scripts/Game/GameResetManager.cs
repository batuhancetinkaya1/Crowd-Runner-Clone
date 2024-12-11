using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class GameResetManager : MonoBehaviour
{
    public static GameResetManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystem;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private DoorBonusHandler doorBonusHandler;
    [SerializeField] private EnemyFightHandler enemyFightHandler;
    [SerializeField] private CameraControl cameraControl;
    [SerializeField] private PlayerResetSystem playerResetSystem;

    private bool isResetting = false;

    private void Awake()
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

    public void ResetGame()
    {
        StartCoroutine(ResetSequence());
    }

    private IEnumerator ResetSequence()
    {
        tileManager.isShuttingDown = true;
        CleanupSystems();
        UIManager.Instance.HideDeathPanel();
        Time.timeScale = 1f;

        // Reset tile manager more comprehensively
        //tileManager.ClearTiles();
        yield return new WaitForSeconds(2f);

        tileManager.isShuttingDown = false;
        tileManager.Start(); // Respawn initial tiles

        GameManager.Instance.SetGameState(GameManager.GameState.Game);

        isResetting = false;
    }

    private void CleanupSystems()
    {
        tileManager.ClearTiles();

        doorBonusHandler.doorsList.Clear();
        doorBonusHandler.ResetHandler();

        enemyFightHandler.ResetEnemyAtEnd();

        playerResetSystem.PlayerReset();
        cameraControl.ResetCamera();
    }
}