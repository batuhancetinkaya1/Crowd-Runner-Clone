using System;
using TMPro;
using UnityEngine;

public class PlayerCrowdSystemControl : MonoBehaviour
{
    [Header("Fermat Spiral Configuration")]
    [SerializeField] private float goldenAngle = 137.5f; // Golden angle in degrees
    [SerializeField] private float spreadFactor = 0.5f; // Controls the spread of the spiral

    [SerializeField] private Transform runnerParent;
    [SerializeField] private GameObject runnerPrefab;

    [Header("Text")]
    [SerializeField ]private TMP_Text crowdCounterText;

    public void CrowdDistribution(GameManager.GameState gameState)
    {
        if(gameState == GameManager.GameState.Game)
        {
            for (int i = 0; i < runnerParent.childCount; i++)
            {
                runnerParent.GetChild(i).localPosition = GetRunnerLocalPosition(i);
            }
        }
        else if(gameState == GameManager.GameState.Fight)
        {

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
        CrowdDistribution(); // Immediately redistribute
    }

    public void ModifyGoldenAngle(float angleChange = -0.01f)
    {
        goldenAngle += angleChange;
        CrowdDistribution(); // Immediately redistribute
    }

    public void CrowdCounterTextUpdater()
    {
        crowdCounterText.text = runnerParent.childCount.ToString();
    }


    internal void FightingCrowdDistrubition()
    {
        int temp = runnerParent.childCount;
        for (int i = 0; i < runnerParent.childCount; i++)
        {
            if(temp % 10 == 1)
            {
                runnerParent.GetChild(i).localPosition = 
            }
            temp--;
        }
    }

    internal void ApplyBonus(BonusType bonusType, int bonusAmount)
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
        for(int i = 0; i < bonusAmount; i++)
        {
            Instantiate(runnerPrefab, runnerParent);
        }
    }

    private void RemoveRunners(float bonusAmount)
    {
        if(bonusAmount > runnerParent.childCount)
        {
            bonusAmount = runnerParent.childCount;
        }

        int runnerAmount = runnerParent.childCount;

        for(int i = runnerAmount - 1; i >= runnerAmount - bonusAmount + 1; i--)
        {
            Transform runnerToDestroy = runnerParent.GetChild(i);
            runnerToDestroy.SetParent(null);
            Destroy(runnerToDestroy.gameObject);
        }
    }


}
