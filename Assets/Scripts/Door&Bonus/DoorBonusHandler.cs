using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class DoorBonusHandler : MonoBehaviour
{
    public static DoorBonusHandler Instance { get; private set; }

    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystemControl;
    [SerializeField] private List<Doors> doorsList = new List<Doors>();

    [Header("Crowd Settings")]
    private const int IdealMinCrowd = 80;
    private const int IdealMaxCrowd = 120;
    private const int MaxCrowd = 200;
    private const int MinCrowd = 1;

    [Header("Bonus Configuration")]
    // Addition, Product, Difference, Division
    [SerializeField] private float[] criticalStateWeights = { 0.4f, 0.5f, 0.08f, 0.02f };
    [SerializeField] private float[] smallStateWeights ={ 0.35f, 0.3f, 0.15f, 0.10f };
    [SerializeField] private float[] idealStateWeights =  { 0.30f, 0.20f, 0.30f, 0.20f };
    [SerializeField] private float[] crowdedStateWeights =  { 0.15f, 0.1f, 0.35f, 0.3f };
    [SerializeField] private float[] overCrowdedStateWeights =  { 0.08f, 0.02f, 0.4f, 0.5f };

    [Header("Bonus Calculation Parameters")]
    private int potentialMaxCrowd = 1;
    private int potentialMinCrowd = 1;

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
            doorsList.Add(door);
            AssignBonuses(door);
        }
    }

    private void AssignBonuses(Doors door)
    {
        CrowdState currentState = DetermineCrowdState(potentialMaxCrowd);

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

        Debug.Log("Potential Maximum Crowd: " + potentialMaxCrowd);
        Debug.Log("Potential Minimum Crowd: " + potentialMinCrowd);
    }

    private CrowdState DetermineCrowdState(int potentialMaxCrowd)
    {
        if (potentialMaxCrowd <= MinCrowd + 40) return CrowdState.Critical;
        if (potentialMaxCrowd < IdealMinCrowd) return CrowdState.Small;
        if (potentialMaxCrowd >= IdealMinCrowd && potentialMaxCrowd <= IdealMaxCrowd) return CrowdState.Ideal;
        if (potentialMaxCrowd > IdealMaxCrowd && potentialMaxCrowd <= MaxCrowd - 40) return CrowdState.Crowded;
        return CrowdState.OverCrowded;
    }

    private BonusType GetRandomBonusType(CrowdState state)
    {
        switch (state)
        {
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
        switch (type)
        {
            case BonusType.Addition:
                return Random.Range(Mathf.Min(potentialMinCrowd - MinCrowd, 5), Mathf.Min(50, MaxCrowd - potentialMaxCrowd));

            case BonusType.Product:
                return Random.Range(1, MaxCrowd / potentialMaxCrowd);

            case BonusType.Difference:

                return Random.Range(Mathf.Min(MaxCrowd - potentialMaxCrowd, 5), Mathf.Min(50, potentialMaxCrowd - MinCrowd));

            case BonusType.Division:

                return Random.Range(1, potentialMinCrowd);

            default:
                return 0;
        }
    }


    private BonusType WeightedRandom(float[] stateWeights)
    {
        int number1 = (int)(Random.Range(0, 1000) * stateWeights[0]);
        int number2 = (int)(Random.Range(0, 1000) * stateWeights[1]);
        int number3 = (int)(Random.Range(0, 1000) * stateWeights[2]);
        int number4 = (int)(Random.Range(0, 1000) * stateWeights[3]);

        int maxNumber = Mathf.Max(number1, number2, number3, number4);
        return maxNumber switch
        {
            _ when maxNumber == number1 => BonusType.Addition,
            _ when maxNumber == number2 => BonusType.Product,
            _ when maxNumber == number3 => BonusType.Division,
            _ when maxNumber == number4 => BonusType.Difference,
            _ => BonusType.Division // Default fallback
        };
    }

    private void UpdatePotentialCrowd(BonusType leftBonusType, BonusType rightBonusType, int leftBonusAmount, int rightBonusAmount)
    {
        int[] possibilities = new int[4]
        {
        MaxCalculatePotentialCrowd(leftBonusType, leftBonusAmount),
        MinCalculatePotentialCrowd(leftBonusType, leftBonusAmount),

        MaxCalculatePotentialCrowd(rightBonusType, rightBonusAmount),
        MinCalculatePotentialCrowd(rightBonusType, rightBonusAmount)
        };

        potentialMaxCrowd = Mathf.Max(possibilities);
        potentialMinCrowd = Mathf.Max(1,Mathf.Min(possibilities));
    }

    private int MaxCalculatePotentialCrowd(BonusType bonusType, int bonusAmount)
    {
        return bonusType switch
        {
            BonusType.Addition => potentialMaxCrowd + bonusAmount,
            BonusType.Difference => potentialMaxCrowd - bonusAmount,
            BonusType.Product => potentialMaxCrowd * bonusAmount,
            BonusType.Division => potentialMaxCrowd / Mathf.Max(bonusAmount, 1),
            _ => potentialMaxCrowd
        };
    }

    private int MinCalculatePotentialCrowd(BonusType bonusType, int bonusAmount)
    {
        return bonusType switch
        {
            BonusType.Addition => potentialMinCrowd + bonusAmount,
            BonusType.Difference => potentialMinCrowd - bonusAmount,
            BonusType.Product => potentialMinCrowd * bonusAmount,
            BonusType.Division => potentialMinCrowd / Mathf.Max(bonusAmount, 1),
            _ => potentialMinCrowd
        };
    }
}