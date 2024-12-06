using System.Collections;
using TMPro;
using UnityEngine;

public class EnemyFightHandler : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private Enemy registeredEnemy;
    [SerializeField] private EnemyAnimControl enemyAnimControl;
    [SerializeField] private TMP_Text crowdCounterText;

    private const float FIGHT_DISTANCE = -5f;
    private const float ROW_ADVANCE_DISTANCE = 1f;
    private const int RUNNERS_PER_FIGHT = 10;

    private int currentFightRow = 0;
    private bool isFightInProgress = false;

    public static EnemyFightHandler Instance { get; private set; }

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

    internal void RegisterEnemy(Enemy enemy, Transform enemyParent, TMP_Text enemyCrowdCounterText)
    {
        registeredEnemy = enemy;
        crowdCounterText = enemyCrowdCounterText;

        // Dynamically calculate enemy count based on DoorBonusHandler
        int dynamicEnemyCount = CalculateDynamicEnemyCount();
        for (int i = 0; i < dynamicEnemyCount; i++)
        {
            Instantiate(enemyPrefab, enemyParent);
        }
        UpdateCrowdCounterText(enemyParent);
        FightPrep(enemy);
    }

    private int CalculateDynamicEnemyCount()
    {
        int potentialMin = DoorBonusHandler.Instance.potentialMinCrowd;
        int potentialMax = DoorBonusHandler.Instance.potentialMaxCrowd;
        return Mathf.RoundToInt((potentialMin + potentialMax) / 2f);
    }

    private void UpdateCrowdCounterText(Transform enemyParent)
    {
        if (crowdCounterText != null)
        {
            crowdCounterText.text = enemyParent.childCount.ToString();
        }
    }

    public void FightPrep(Enemy enemy)
    {
        currentFightRow = 0;
        isFightInProgress = false;
        enemy.CrowdDistribution(GameManager.GameState.FightPrep);
    }

    public void FightEnemy()
    {
        if (isFightInProgress) return;

        int totalRunners = registeredEnemy.enemyParent.childCount;
        int startIndex = currentFightRow * RUNNERS_PER_FIGHT;
        int endIndex = Mathf.Min(startIndex + RUNNERS_PER_FIGHT, totalRunners);

        if (startIndex >= totalRunners)
        {
            return;
        }

        StartCoroutine(ExecuteRowFight(startIndex, endIndex));
    }

    private IEnumerator ExecuteRowFight(int startIndex, int endIndex)
    {
        isFightInProgress = true;

        for (int i = startIndex; i < endIndex; i++)
        {
            Transform enemyRunner = registeredEnemy.enemyParent.GetChild(i);
            Transform enemyObject = enemyRunner.Find("Enemy");
            Animator enemyAnimator = enemyObject.GetComponent<Animator>();

            // Run forward
            enemyAnimControl.Run(enemyAnimator);
            StartCoroutine(MoveRunner(enemyRunner, Vector3.back * FIGHT_DISTANCE));
            yield return new WaitForSeconds(0.5f);
        }

        yield return new WaitForSeconds(1f);

        // Execute fight and destruction
        for (int i = startIndex; i < endIndex; i++)
        {
            Transform enemyRunner = registeredEnemy.enemyParent.GetChild(i);
            Transform enemyObject = enemyRunner.Find("Enemy");
            Animator enemyAnimator = enemyObject.GetComponent<Animator>();

            enemyAnimControl.Fight(enemyAnimator);
            yield return new WaitForSeconds(0.5f);

            if (CanDestroyPlayerRunner())
            {
                DestroyPlayerRunner();
                Destroy(enemyRunner.gameObject);
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
        FightEnemy(); // Continue to next row
    }

    private void AdvanceRemainingRunners()
    {
        int totalRunners = registeredEnemy.enemyParent.childCount;
        for (int i = 0; i < totalRunners; i++)
        {
            Transform runner = registeredEnemy.enemyParent.GetChild(i);
            Transform enemyObject = runner.Find("Enemy");
            Animator enemyAnimator = enemyObject.GetComponent<Animator>();

            enemyAnimControl.FightPrep(enemyAnimator);
            runner.localPosition += Vector3.back * ROW_ADVANCE_DISTANCE;
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

    public int GetEnemyRunnerCount()
    {
        return registeredEnemy.enemyParent.childCount;
    }

    public bool CanDestroyEnemyRunner()
    {
        return registeredEnemy.enemyParent.childCount > 0;
    }

    public void DestroyEnemyRunner()
    {
        Transform enemyToDestroy = registeredEnemy.enemyParent.GetChild(0);
        Destroy(enemyToDestroy.gameObject);
    }

    private bool CanDestroyPlayerRunner()
    {
        return FindObjectOfType<PlayerCrowdSystemControl>().runnerParent.childCount > 0;
    }

    private void DestroyPlayerRunner()
    {
        Transform playerToDestroy = FindObjectOfType<PlayerCrowdSystemControl>().runnerParent.GetChild(0);
        Destroy(playerToDestroy.gameObject);
    }
}