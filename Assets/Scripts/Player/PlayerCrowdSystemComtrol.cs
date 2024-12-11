using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCrowdSystemControl : MonoBehaviour
{
    [Header("Fermat Spiral Configuration")]
    [SerializeField] private float goldenAngle = 137.5f; // Golden angle in degrees
    [SerializeField] private float spreadFactor = 0.25f; // Controls the spread of the spiral
    [SerializeField] internal Transform runnerParent;
    [SerializeField] private GameObject runnerPrefab;

    [SerializeField] private PlayerAnimControl playerAnimControl;

    [Header("Text")]
    [SerializeField] internal TMP_Text crowdCounterText;
    public static PlayerCrowdSystemControl Instance { get; private set; }

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
    public void CrowdDistribution(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.Game:
                DistributeFermatSpiral();
                break;
            case GameManager.GameState.FightPrep:
                DistributeFermatSpiral();
                break;
            case GameManager.GameState.Fight:
                DistributeFermatSpiral();
                break;
        }
    }

    private void DistributeFermatSpiral()
    {
        for (int i = 0; i < runnerParent.childCount; i++)
        {
            runnerParent.GetChild(i).localPosition = GetRunnerLocalPosition(i);
        }
    }

    private Vector3 GetRunnerLocalPosition(int index)
    {
        // More accurate Fermat spiral calculation
        float t = index + 0.5f; // Adjusted to start from center
        float radius = spreadFactor * Mathf.Sqrt(t);
        float angle = index * goldenAngle * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(angle);
        float z = radius * Mathf.Sin(angle);
        return new Vector3(x, 0, z);
    }

    public float GetCrowRadius()
    {
        float radius = spreadFactor * Mathf.Sqrt(runnerParent.childCount);
        return radius;
    }

    public void ModifyGoldenAngle(float angleChange = -0.01f)
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Game)
        {
            goldenAngle += angleChange;
            CrowdDistribution(GameManager.GameState.Game); // Immediately redistribute
        }
        else if(GameManager.Instance.CurrentState == GameManager.GameState.Fight)
        {
            goldenAngle = 137.5f;
            CrowdDistribution(GameManager.GameState.Fight); // Immediately redistribute
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameState.FightPrep)
        {
            goldenAngle = 137.5f;
            CrowdDistribution(GameManager.GameState.FightPrep);
        }
    }

    #region ApplyBonus
    public void ApplyBonus(BonusType bonusType, int bonusAmount)
    {
        switch (bonusType)
        {
            case BonusType.Addition:
                AddRunners(bonusAmount);
                break;
            case BonusType.Difference:
                RemoveRunners(bonusAmount);
                break;
            case BonusType.Product:
                int runnersToAdd = (runnerParent.childCount * bonusAmount) - runnerParent.childCount;
                AddRunners(runnersToAdd);
                break;
            case BonusType.Division:
                int runnersToRemove = runnerParent.childCount - (runnerParent.childCount / bonusAmount);
                RemoveRunners(runnersToRemove);
                break;
        }
        CrowdCounterTextUpdater();
    }

    private void AddRunners(float bonusAmount)
    {
        for (int i = 0; i < bonusAmount; i++)
        {
            Instantiate(runnerPrefab, runnerParent);
        }
    }

    private void RemoveRunners(float bonusAmount)
    {
        if (bonusAmount > runnerParent.childCount)
        {
            bonusAmount = runnerParent.childCount;
        }
        int runnerAmount = runnerParent.childCount;
        if(runnerAmount - bonusAmount <= 0)
        {
            bonusAmount = runnerAmount - 1;
        }
        for (int i = runnerAmount - 1; i >= runnerAmount - bonusAmount; i--)
        {
            Transform runnerToDestroy = runnerParent.GetChild(i);
            runnerToDestroy.SetParent(null);
            Destroy(runnerToDestroy.gameObject);
        }
    }
    #endregion 

    public void CrowdCounterTextUpdater()
    {
        crowdCounterText.text = runnerParent.childCount.ToString();
    }

    public void CrowdCounterTextUpdater2()
    {
        crowdCounterText.text = 1.ToString();
    }

    internal int GetCrowdCount()
    {
        return runnerParent.childCount;
    }
}