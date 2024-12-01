using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorBonusHandler : MonoBehaviour
{
    public static DoorBonusHandler Instance { get; private set; }

    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystemControl;
    [SerializeField] private List<Doors> doorsList;

    [Header("Deneme")]
    private BonusType rightDoorBonusType = BonusType.Division;
    private int rightDoorAmount = 5;

    private BonusType leftDoorBonusType;
    private int leftDoorAmount;

    private void Awake()
    {
        // Singleton pattern implementation
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
            BonusManagement(door);
        }
    }


    /// <summary>
    /// we have to decide a logic here. we have to consider current crowd count, maxcrowdcount.
    /// </summary>
    /// <param name="door"></param>
    internal void BonusManagement(Doors door)
    {
        door.SetBonusAmount(rightDoorAmount, leftDoorAmount);
        door.SetBonusType(rightDoorBonusType, leftDoorBonusType);
    }


}
