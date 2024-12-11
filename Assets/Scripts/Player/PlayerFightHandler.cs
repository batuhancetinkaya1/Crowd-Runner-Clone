using UnityEngine;
using System.Collections;
using TMPro;

public class PlayerFightHandler : MonoBehaviour
{
    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystem;
    [SerializeField] private PlayerAnimControl playerAnimControl;
    [SerializeField] private EnemyFightHandler enemyFightHandler;
    [SerializeField] private PlayerMoveControl playerMoveControl;

    [Header("Fight Speed Settings")]
    [SerializeField] private float gameSpeed = 5f;
    [SerializeField] private float fightPrepSpeed = 5f;
    [SerializeField] private float fightSpeed = 0f;
    //[SerializeField] private float marchspeed = 0.1f;

    [Header("Fight Positioning")]
    [SerializeField] private float initialFightDistance = 1f;
    [SerializeField] private float minFightDistance = 0.1f;
    [SerializeField] private float fightDistanceReductionRate = 0.5f;
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private Vector3 smoothedPosition;


    [SerializeField] private AudioClip bubblePop;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private float loopDelay = 1.0f;


    public bool isFighting = false;

    // Prepare the player for fight by centering and setting speed
    public void FightPrep()
    {
        // Set speed to fight preparation speed
        playerMoveControl.SetSpeed(fightPrepSpeed);
    }

    // Check conditions to initiate the fight
    public void CheckFightConditions()
    {
        // Center the player on the road
        smoothedPosition = Vector3.Lerp(transform.position, new Vector3(0, transform.position.y, transform.position.z), smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        if (isFighting || enemyFightHandler.registeredEnemy == null) return;

        int initialPlayerRunners = playerCrowdSystem.GetCrowdCount();
        int initialEnemyRunners = enemyFightHandler.registeredEnemyParent.childCount;
        int runnersToDestroy = Mathf.Min(initialPlayerRunners, initialEnemyRunners);

        float playerCrowdRadius = playerCrowdSystem.GetCrowRadius();
        float enemyCrowdRadius = enemyFightHandler.registeredEnemy.GetCrowRadius();
        float distanceBetweenCrowds = Mathf.Abs(
            enemyFightHandler.registeredEnemy.transform.position.z - transform.position.z
        );

        if (distanceBetweenCrowds <= (playerCrowdRadius + enemyCrowdRadius + initialFightDistance))
        {
            InitiateFight(runnersToDestroy);
        }
    }

    // Start the fight sequence
    private void InitiateFight(int runnersToDestroy)
    {
        isFighting = true;
        playerMoveControl.SetSpeed(fightSpeed);
        enemyFightHandler.registeredEnemy.SetSpeed(fightSpeed);
        StartCoroutine(DestroyRunnersWithDelay(runnersToDestroy));
    }

    private void PlaySound()
    {
        audioSource.Play();
    }


    // Destroy runners on both sides with delays
    private IEnumerator DestroyRunnersWithDelay(int runnersToDestroy)
    {
        playerMoveControl.SetSpeed(gameSpeed);
        enemyFightHandler.registeredEnemy.SetSpeed(gameSpeed);

        audioSource.clip = bubblePop;
        InvokeRepeating("PlaySound", 0f, bubblePop.length + loopDelay);

        for (int i = 0; i < runnersToDestroy; i++)
        {
            yield return new WaitForSeconds(0.01f);

            if (playerCrowdSystem.runnerParent.childCount > 0 &&
                enemyFightHandler.registeredEnemyParent.childCount > 0)
            {
                Transform playerRunnerToDestroy = playerCrowdSystem.runnerParent.GetChild(
                    playerCrowdSystem.runnerParent.childCount - 1
                );
                Transform enemyRunnerToDestroy = enemyFightHandler.registeredEnemyParent.GetChild(
                    enemyFightHandler.registeredEnemyParent.childCount - 1
                );

                Animator playerRunnerAnimator = playerRunnerToDestroy.GetComponent<Animator>();
                Animator enemyRunnerAnimator = enemyRunnerToDestroy.GetComponent<Animator>();

                if (playerRunnerAnimator != null)
                    playerAnimControl.Fight(playerRunnerAnimator);

                if (enemyRunnerAnimator != null)
                    enemyFightHandler.registeredEnemy.enemyAnimControl.Fight(enemyRunnerAnimator);

                AdjustFightDistance(i + 1, runnersToDestroy);
                enemyFightHandler.RemoveRunners(playerRunnerToDestroy);
                enemyFightHandler.registeredEnemy.CrowdDistribution(GameManager.GameState.Fight);  //?
                enemyFightHandler.RemoveRunners(enemyRunnerToDestroy);
                playerCrowdSystem.CrowdDistribution(GameManager.GameState.Fight);  //?
                playerCrowdSystem.CrowdCounterTextUpdater();
                enemyFightHandler.registeredEnemy.CrowdCounterTextUpdater();
            }
        }

        CancelInvoke();
        CheckFightCompletion();
    }

    // Adjust fight distance dynamically during the fight
    private void AdjustFightDistance(int currentDestroyedRunners, int totalRunnersToDestroy)
    {
        // Calculate progress ratio and target fight distance
        float progressRatio = (float)currentDestroyedRunners / totalRunnersToDestroy;
        float currentFightDistance = Mathf.Lerp(initialFightDistance, minFightDistance,
            progressRatio * fightDistanceReductionRate);

        // Allow a slight overlap at the edges
        float edgeOverlap = 0.1f; // Allow 0.1f beyond their edges

        // Calculate minimum distance including radii and edge overlap
        float minimumDistance = playerCrowdSystem.GetCrowRadius() +
                                enemyFightHandler.registeredEnemy.GetCrowRadius() -
                                edgeOverlap +
                                currentFightDistance;

        Vector3 playerPosition = transform.position;
        Vector3 enemyPosition = enemyFightHandler.registeredEnemy.transform.position;

        Vector3 directionToEnemy = (enemyPosition - playerPosition).normalized;
        float currentDistanceBetween = Vector3.Distance(playerPosition, enemyPosition);

        // If current distance is smaller than required minimum, adjust positions
        if (currentDistanceBetween < minimumDistance)
        {
            float adjustmentDistance = (minimumDistance - currentDistanceBetween) * 0.5f;
            Vector3 adjustmentOffset = directionToEnemy * adjustmentDistance;

            // Adjust player and enemy positions dynamically
            transform.position -= adjustmentOffset;
            enemyFightHandler.registeredEnemy.transform.position += adjustmentOffset;
        }
    }


    // Check fight outcome
    private void CheckFightCompletion()
    {
        int remainingPlayerRunners = playerCrowdSystem.GetCrowdCount();
        int remainingEnemyRunners = enemyFightHandler.registeredEnemyParent.childCount;

        if (remainingPlayerRunners > 0 && remainingEnemyRunners == 0)
        {
            FinishFight(true);
        }
        else if (remainingPlayerRunners <= 0)
        {
            FinishFight(false);
        }
    }

    // Handle fight result
    private void FinishFight(bool playerWon)
    {
        if (playerWon)
        {
            GameManager.Instance.SetGameState(GameManager.GameState.Game);
            isFighting = false;
            playerMoveControl.SetSpeed(gameSpeed);
            enemyFightHandler.registeredEnemy.SetSpeed(gameSpeed);
            enemyFightHandler.ResetEnemy();
        }
        else
        {
            Time.timeScale = 0f;
            UIManager.Instance.ShowDeathPanel();
            GameManager.Instance.SetGameState(GameManager.GameState.GameOver);
        }
    }
}
