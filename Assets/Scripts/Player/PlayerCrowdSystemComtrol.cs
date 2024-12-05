using System;
using TMPro;
using UnityEngine;

public class PlayerCrowdSystemControl : MonoBehaviour
{
    [Header("Fermat Spiral Configuration")]
    [SerializeField] private float goldenAngle = 137.5f; // Golden angle in degrees
    [SerializeField] private float spreadFactor = 0.5f; // Controls the spread of the spiral
    [SerializeField] internal Transform runnerParent;
    [SerializeField] private GameObject runnerPrefab;

    [Header("Fight Formation Configuration")]
    [SerializeField] private float fightFormationWidth = 9f; // Total width for 10 runners
    [SerializeField] private float fightFormationSpacing = 1f; // Spacing between runners in a row
    [SerializeField] private float fightFormationRowHeight = 1f; // Height between rows

    [Header("Text")]
    [SerializeField] private TMP_Text crowdCounterText;

    public void CrowdDistribution(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.Game:
                DistributeFermatSpiral();
                break;
            case GameManager.GameState.Fight:
                DistributeFightFormation();
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

    private void DistributeFightFormation()
    {
        int runnersCount = runnerParent.childCount;
        int runnersPerRow = 10;

        // Calculate the maximum number of complete rows
        int totalRows = Mathf.CeilToInt((float)runnersCount / runnersPerRow);

        for (int i = 0; i < runnersCount; i++)
        {
            // Calculate row and position within row
            int row = i / runnersPerRow;
            int positionInRow = i % runnersPerRow;

            // Calculate x position using formation width and spacing
            float effectiveWidth = Mathf.Min(fightFormationWidth, (runnersPerRow - 1) * fightFormationSpacing);
            float xOffset = Mathf.Lerp(-effectiveWidth / 2f, effectiveWidth / 2f,
                positionInRow / (float)(Mathf.Min(runnersPerRow, runnersCount - row * runnersPerRow) - 1));

            // Calculate z position (rows pushed back)
            float zOffset = -row * fightFormationRowHeight;

            // Position the runner
            Transform runner = runnerParent.GetChild(i);
            runner.localPosition = new Vector3(xOffset, 0, zOffset);
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

    public void AdjustSpread(float newSpreadFactor)
    {
        spreadFactor = newSpreadFactor;
        CrowdDistribution(GameManager.GameState.Game); // Immediately redistribute
    }

    public void ModifyGoldenAngle(float angleChange = -0.01f)
    {
        goldenAngle += angleChange;
        CrowdDistribution(GameManager.GameState.Game); // Immediately redistribute
    }

    public void CrowdCounterTextUpdater()
    {
        crowdCounterText.text = runnerParent.childCount.ToString();
    }

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
        for (int i = runnerAmount - 1; i >= runnerAmount - bonusAmount + 1; i--)
        {
            Transform runnerToDestroy = runnerParent.GetChild(i);
            runnerToDestroy.SetParent(null);
            Destroy(runnerToDestroy.gameObject);
        }
    }

    internal int GetCrowdCount()
    {
        return runnerParent.childCount;
    }
}