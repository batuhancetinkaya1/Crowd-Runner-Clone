using System.Collections.Generic;
using UnityEngine;

public class DoorBonusHandler : MonoBehaviour
{
    public static DoorBonusHandler Instance { get; private set; }

    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystemControl;
    [SerializeField] private List<Doors> doorsList = new List<Doors>();

    [Header("Crowd Settings")]
    private const int IdealMinCrowd = 45;
    private const int IdealMaxCrowd = 55;
    private const int MaxCrowd = 200;
    private const int MinCrowd = 1;

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
        int currentCrowd = playerCrowdSystemControl.runnerParent.childCount;
        CrowdState currentState = DetermineCrowdState(currentCrowd);

        // Randomly determine bonus types for left and right gates
        BonusType leftBonusType = GetRandomBonusType(currentState);
        BonusType rightBonusType = GetRandomBonusType(currentState);

        // Calculate bonus amounts for each gate based on type and current crowd state
        int leftBonusAmount = CalculateBonusAmount(leftBonusType, currentState, currentCrowd);
        int rightBonusAmount = CalculateBonusAmount(rightBonusType, currentState, currentCrowd);

        // Assign bonuses to the door
        door.SetBonusType(leftBonusType, rightBonusType);
        door.SetBonusAmount(leftBonusAmount, rightBonusAmount);
    }

    private CrowdState DetermineCrowdState(int currentCrowd)
    {
        if (currentCrowd <= MinCrowd + 10) return CrowdState.Critical;
        if (currentCrowd < IdealMinCrowd) return CrowdState.Small;
        if (currentCrowd >= IdealMinCrowd && currentCrowd <= IdealMaxCrowd) return CrowdState.Ideal;
        if (currentCrowd > IdealMaxCrowd && currentCrowd <= MaxCrowd - 10) return CrowdState.Crowded;
        return CrowdState.Overcrowded;
    }

    private BonusType GetRandomBonusType(CrowdState state)
    {
        BonusType[] allBonusTypes = (BonusType[])System.Enum.GetValues(typeof(BonusType));

        // Adjust weights for bonus types based on the crowd state
        switch (state)
        {
            case CrowdState.Critical:
                return WeightedRandom(new[] { BonusType.Addition, BonusType.Product });
            case CrowdState.Small:
                return WeightedRandom(new[] { BonusType.Addition, BonusType.Product, BonusType.Difference });
            case CrowdState.Ideal:
                return WeightedRandom(allBonusTypes);
            case CrowdState.Crowded:
                return WeightedRandom(new[] { BonusType.Difference, BonusType.Division });
            case CrowdState.Overcrowded:
                return WeightedRandom(new[] { BonusType.Difference, BonusType.Division });
            default:
                return BonusType.Addition;
        }
    }

    private int CalculateBonusAmount(BonusType type, CrowdState state, int currentCrowd)
    {
        switch (type)
        {
            case BonusType.Addition:
                return Random.Range(5, 20);
            case BonusType.Difference:
                return Random.Range(-20, -5);
            case BonusType.Product:
                return Random.Range(2, 3); // Moderate multipliers
            case BonusType.Division:
                return Mathf.Max(2, Random.Range(3, 5)); // Avoid division by zero issues
            default:
                return 0;
        }
    }

    private BonusType WeightedRandom(BonusType[] options)
    {
        int randomIndex = Random.Range(0, options.Length);
        return options[randomIndex];
    }
}

public enum CrowdState
{
    Critical,
    Small,
    Ideal,
    Crowded,
    Overcrowded
}
