using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class DoorBonusHandler : MonoBehaviour
{
    public static DoorBonusHandler Instance { get; private set; }

    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystemControl;
    [SerializeField] private List<Doors> doorsList = new List<Doors>();

    [Header("Crowd Settings")]
    private const int IdealMinCrowd = 120;
    private const int IdealMaxCrowd = 180;
    private const int MaxCrowd = 300;
    private const int MinCrowd = 10;

    [Header("Bonus Configuration")]
    // Addition, Product, Difference, Division
    [SerializeField] private float[] initialStateWeights = { 0.5f, 0.5f, 0.0f, 0.0f };
    [SerializeField] private float[] criticalStateWeights = { 0.4f, 0.5f, 0.08f, 0.02f };
    [SerializeField] private float[] smallStateWeights = { 0.35f, 0.3f, 0.15f, 0.10f };
    [SerializeField] private float[] idealStateWeights = { 0.30f, 0.20f, 0.30f, 0.20f };
    [SerializeField] private float[] crowdedStateWeights = { 0.15f, 0.1f, 0.35f, 0.3f };
    [SerializeField] private float[] overCrowdedStateWeights = { 0.08f, 0.02f, 0.4f, 0.5f };

    [Header("Bonus Calculation Parameters")]
    private int potentialMaxCrowd = 1;
    private int potentialMinCrowd = 1;

    private int currentCrowd = 1;




    [Header("DENEME TEST")]
    [SerializeField] int i = 0;
    [SerializeField] bool isStart = true;
    int initialMin = 70;
    int initialMax = 150;

    int maxbonusamount = 40;
    int minBonusAmount = 2;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void RegisterDoor(Doors door)
    {
        if (!doorsList.Contains(door))
        {
            if (isStart)
            {
                InitialDoorRegister(door);
            }
            else if (!isStart)
            {
                DynamicDoorRegister(door);
            }

        }
    }

    private void InitialDoorRegister(Doors door)
    {
        BonusType leftBonusType = GetRandomBonusType(CrowdState.Initial);
        BonusType rightBonusType = GetRandomBonusType(CrowdState.Initial);

        initialStateWeights[0] -= 0.02f;
        initialStateWeights[0] -= 0.08f;
        initialStateWeights[0] += 0.08f;
        initialStateWeights[0] += 0.02f;

        int leftBonusAmount = CalculateBonusAmount(leftBonusType);
        int rightBonusAmount = CalculateBonusAmount(rightBonusType);

        door.SetBonusType(leftBonusType, rightBonusType);
        door.SetBonusAmount(leftBonusAmount, rightBonusAmount);
        doorsList.Add(door);

        Debug.Log("Potential Maximum Crowd: " + potentialMaxCrowd);
        Debug.Log("Potential Minimum Crowd: " + potentialMinCrowd);

        i++;
        if (i >= 5) isStart = false; ResetPotantialCrowd();
    }


    private void DynamicDoorRegister(Doors door)
    {
        AssignBonuses(door);
    }

    private void AssignBonuses(Doors door)
    {
        ResetPotantialCrowd();

        CrowdState currentState = DetermineCrowdState(potentialMinCrowd, potentialMaxCrowd);

        // Randomly determine bonus types for left and right gates
        BonusType leftBonusType = GetRandomBonusType(currentState);
        BonusType rightBonusType = GetRandomBonusType(currentState);

        // Calculate bonus amounts for each gate based on type 
        int leftBonusAmount = CalculateBonusAmount(leftBonusType);
        int rightBonusAmount = CalculateBonusAmount(rightBonusType);

        //update game stats
        UpdatePotentialCrowd(leftBonusType, rightBonusType, leftBonusAmount, rightBonusAmount);  //potansiyel crowd güncellemesi yapýlmalý min ve max. 

        // Assign bonuses to the door
        door.SetBonusType(leftBonusType, rightBonusType);
        door.SetBonusAmount(leftBonusAmount, rightBonusAmount);
        doorsList.Add(door);

        Debug.Log("Potential Maximum Crowd: " + potentialMaxCrowd);
        Debug.Log("Potential Minimum Crowd: " + potentialMinCrowd);
    }

    private CrowdState DetermineCrowdState(int potentialMinCrowd, int potentialMaxCrowd)
    {
        int crowd = (int)(potentialMinCrowd * 0.4f + potentialMaxCrowd * 0.6f);

        if (crowd <= MinCrowd + 50) return CrowdState.Critical;
        if (crowd < IdealMinCrowd) return CrowdState.Small;
        if (crowd >= IdealMinCrowd && crowd <= IdealMaxCrowd) return CrowdState.Ideal;
        if (crowd > IdealMaxCrowd && crowd <= MaxCrowd - 50) return CrowdState.Crowded;
        return CrowdState.OverCrowded;
    }

    private void ResetPotantialCrowd()
    {
        currentCrowd = playerCrowdSystemControl.GetCrowdCount();

        potentialMaxCrowd = currentCrowd;
        potentialMinCrowd = currentCrowd;

        foreach (Doors door in doorsList)
        {
            UpdatePotentialCrowd(door.GetBonusType(-1), door.GetBonusType(1), door.GetBonusAmount(-1), door.GetBonusAmount(1));
        }
    }

    private BonusType GetRandomBonusType(CrowdState state)
    {
        switch (state)
        {
            case CrowdState.Initial:
                return WeightedRandom(initialStateWeights);
            case CrowdState.Critical:
                return WeightedRandom(criticalStateWeights);
            case CrowdState.Small:
                return WeightedRandom(smallStateWeights);
            case CrowdState.Ideal:
                return WeightedRandom(idealStateWeights);
            case CrowdState.Crowded:
                return WeightedRandom(crowdedStateWeights);
            case CrowdState.OverCrowded:
                return WeightedRandom(overCrowdedStateWeights);
            default:
                return BonusType.Addition;
        }
    }

    private int CalculateBonusAmount(BonusType type)
    {
        int amount = 1;
        if (!isStart)
        {
            switch (type)
            {
                case BonusType.Addition:

                    amount = Random.Range(
                        Mathf.Max(minBonusAmount, MinCrowd - potentialMinCrowd),
                        Mathf.Min(maxbonusamount, MaxCrowd - potentialMaxCrowd));

                    if(potentialMaxCrowd + amount > MaxCrowd)
                    {
                        amount = 0;
                    }
                    return amount;

                case BonusType.Product:

                    amount = Random.Range(
                        Mathf.Max(minBonusAmount, MinCrowd / potentialMinCrowd),
                        Mathf.Min(maxbonusamount, MaxCrowd / potentialMaxCrowd));

                    if(amount * potentialMaxCrowd > MaxCrowd)
                    {
                        amount = 1;
                    }

                    return amount;

                case BonusType.Difference:

                    amount = Random.Range(
                        Mathf.Max(minBonusAmount, MinCrowd - potentialMinCrowd),
                        Mathf.Min(maxbonusamount, MaxCrowd - potentialMaxCrowd));

                    if(potentialMinCrowd - amount < MinCrowd)
                    {
                        amount = 0;
                    }

                    return amount;

                case BonusType.Division:

                    amount = Random.Range(
                        Mathf.Max(minBonusAmount, MinCrowd / potentialMinCrowd),
                        Mathf.Min(maxbonusamount, MaxCrowd / potentialMaxCrowd));

                    if(potentialMinCrowd / amount < MinCrowd)
                    {
                        amount = 1;
                    }

                    return amount;
                default:
                    return 0;
            }
        }
        else /*if(isStart)*/
        {
            switch (type)
            {
                case BonusType.Addition:

                    amount = Random.Range(
                        Mathf.Max(minBonusAmount, initialMin - potentialMinCrowd),
                        Mathf.Min(maxbonusamount, initialMax - potentialMaxCrowd));

                    if (potentialMaxCrowd + amount > initialMax)
                    {
                        amount = 0;
                    }
                    return amount;

                case BonusType.Product:

                    amount = Random.Range(
                        Mathf.Max(minBonusAmount, initialMin / potentialMinCrowd),
                        Mathf.Min(maxbonusamount, initialMax / potentialMaxCrowd));

                    if (amount * potentialMaxCrowd > initialMax)
                    {
                        amount = 1;
                    }

                    return amount;

                case BonusType.Difference:

                    amount = Random.Range(
                        Mathf.Max(minBonusAmount, initialMin - potentialMinCrowd),
                        Mathf.Min(maxbonusamount, initialMax - potentialMaxCrowd));

                    if (potentialMinCrowd - amount < initialMin)
                    {
                        amount = 0;
                    }

                    return amount;

                case BonusType.Division:

                    amount = Random.Range(
                        Mathf.Max(minBonusAmount, initialMin / potentialMinCrowd),
                        Mathf.Min(maxbonusamount, initialMax / potentialMaxCrowd));

                    if (potentialMinCrowd / amount < initialMin)
                    {
                        amount = 1;
                    }

                    return amount;
                default:
                    return 0;
            }
        }
    }

    private BonusType WeightedRandom(float[] stateWeights)
    {
        float totalWeight = 0f;
        foreach (float weight in stateWeights)
        {
            totalWeight += weight;
        }

        float randomValue = Random.Range(0f, totalWeight);

        float cumulativeWeight = 0f;
        for (int i = 0; i < stateWeights.Length; i++)
        {
            cumulativeWeight += stateWeights[i];
            if (randomValue <= cumulativeWeight)
            {
                return (BonusType)i;
            }
        }
        return BonusType.Addition;
    }


    private void UpdatePotentialCrowd(BonusType leftBonusType, BonusType rightBonusType, int leftBonusAmount, int rightBonusAmount)
    {
        int[] potentialValues = new int[4]
        {
        ApplyBonus(potentialMaxCrowd, leftBonusType, leftBonusAmount),
        ApplyBonus(potentialMinCrowd, leftBonusType, leftBonusAmount),
        ApplyBonus(potentialMaxCrowd, rightBonusType, rightBonusAmount),
        ApplyBonus(potentialMinCrowd, rightBonusType, rightBonusAmount)
        };

        potentialMaxCrowd = Mathf.Max(potentialValues);
        potentialMinCrowd = Mathf.Max(1, potentialValues.Min());
    }

    private int ApplyBonus(int currentValue, BonusType bonusType, int bonusAmount)
    {
        return bonusType switch
        {
            BonusType.Addition => currentValue + bonusAmount,
            BonusType.Difference => currentValue - bonusAmount,
            BonusType.Product => currentValue * bonusAmount,
            BonusType.Division => (int)(currentValue / Mathf.Max(bonusAmount, 1)),
            _ => currentValue
        };
    }

    internal void RemoveFromList(Doors door)
    {
        doorsList.Remove(door);
    }
}