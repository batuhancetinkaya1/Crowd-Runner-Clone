using UnityEngine;
using TMPro;
using System.Collections;

public class PlayerFightHandler : MonoBehaviour
{
    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystem;
    [SerializeField] private PlayerAnimControl playerAnimControl;
    [SerializeField] private EnemyFightHandler enemyFightHandler;

    private const float FIGHT_DISTANCE = 5f;
    private const float ROW_ADVANCE_DISTANCE = 1f;
    private const int RUNNERS_PER_FIGHT = 10;

    private int currentFightRow = 0;
    private bool isFightInProgress = false;

    public void FightPrep()
    {
        currentFightRow = 0;
        isFightInProgress = false;
    }

    public void FightPlayer()
    {
        if (isFightInProgress) return;

        int totalRunners = playerCrowdSystem.runnerParent.childCount;
        int startIndex = currentFightRow * RUNNERS_PER_FIGHT;
        int endIndex = Mathf.Min(startIndex + RUNNERS_PER_FIGHT, totalRunners);

        if (startIndex >= totalRunners)
        {
            CheckFightOutcome();
            return;
        }

        StartCoroutine(ExecuteRowFight(startIndex, endIndex));
    }

    private IEnumerator ExecuteRowFight(int startIndex, int endIndex)
    {
        isFightInProgress = true;

        for (int i = startIndex; i < endIndex; i++)
        {
            Transform playerRunner = playerCrowdSystem.runnerParent.GetChild(i);
            Transform playerObject = playerRunner.Find("Player");
            Animator playerAnimator = playerObject.GetComponent<Animator>();

            // Run forward
            playerAnimControl.Run(playerAnimator);
            StartCoroutine(MoveRunner(playerRunner, Vector3.forward * FIGHT_DISTANCE));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        // Execute fight and destruction
        for (int i = startIndex; i < endIndex; i++)
        {
            Transform playerRunner = playerCrowdSystem.runnerParent.GetChild(i);
            Transform playerObject = playerRunner.Find("Player");
            Animator playerAnimator = playerObject.GetComponent<Animator>();

            playerAnimControl.Fight(playerAnimator);
            yield return new WaitForSeconds(0.5f);

            // Destroy runner if enemy exists
            if (enemyFightHandler.CanDestroyEnemyRunner())
            {
                enemyFightHandler.DestroyEnemyRunner();
                Destroy(playerRunner.gameObject);
            }
            else
            {
                break;
            }
        }

        // Advance remaining runners
        AdvanceRemainingRunners();

        currentFightRow++;
        isFightInProgress = false;
        FightPlayer(); // Continue to next row
    }

    private void AdvanceRemainingRunners()
    {
        int totalRunners = playerCrowdSystem.runnerParent.childCount;
        for (int i = 0; i < totalRunners; i++)
        {
            Transform runner = playerCrowdSystem.runnerParent.GetChild(i);
            Transform playerObject = runner.Find("Player");
            Animator playerAnimator = playerObject.GetComponent<Animator>();

            playerAnimControl.FightPrep(playerAnimator);
            runner.localPosition += Vector3.forward * ROW_ADVANCE_DISTANCE;
        }
    }

    private IEnumerator MoveRunner(Transform runner, Vector3 direction)
    {
        float elapsedTime = 0;
        float moveDuration = 1f;
        Vector3 startPosition = runner.localPosition;
        Vector3 endPosition = startPosition + direction;

        while (elapsedTime < moveDuration)
        {
            runner.localPosition = Vector3.Lerp(startPosition, endPosition, elapsedTime / moveDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        runner.localPosition = endPosition;
    }

    private void CheckFightOutcome()
    {
        int playerRunners = playerCrowdSystem.runnerParent.childCount;
        int enemyRunners = enemyFightHandler.GetEnemyRunnerCount();

        if (enemyRunners > playerRunners)
        {
            // Enemy wins
            GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
        }
        else
        {
            // Player wins
            GameManager.Instance.SetGameState(GameManager.GameState.Game);
            DoorBonusHandler.Instance.ResetPotantialCrowd();
        }
    }
}