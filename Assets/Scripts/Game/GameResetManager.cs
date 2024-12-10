using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameResetManager : MonoBehaviour
{
    public static GameResetManager Instance { get; private set; }

    [Header("References")]
    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystem;
    [SerializeField] private TileManager tileManager;
    [SerializeField] private DoorBonusHandler doorBonusHandler;
    [SerializeField] private EnemyFightHandler enemyFightHandler;

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
        // First, pause the game and show death panel
        Time.timeScale = 0f;
        UIManager.Instance.ShowDeathPanel();

        // Wait for one frame to ensure UI updates
        //yield return null;

        // Clean up existing objects and systems
        CleanupSystems();

        // Reset time scale
        Time.timeScale = 1f;

        isResetting = false;

        // Reload the scene
        //SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        yield break;
    }

    private void CleanupSystems()
    {
        // Clear door bonus handler
        if (doorBonusHandler != null)
        {
            doorBonusHandler.doorsList.Clear();
        }

        // Clear enemy fight handler
        if (enemyFightHandler != null)
        {
            enemyFightHandler.ResetEnemyAtEnd();
        }

        // Reset tile manager
        if (tileManager != null)
        {
            tileManager.ClearTiles();
        }

        // Reset player crowd system
        if (playerCrowdSystem != null)
        {
            for (int i = playerCrowdSystem.runnerParent.childCount - 1; i >= 0; i--)
            {
                Destroy(playerCrowdSystem.runnerParent.GetChild(i).gameObject);
            }
        }
    }
}