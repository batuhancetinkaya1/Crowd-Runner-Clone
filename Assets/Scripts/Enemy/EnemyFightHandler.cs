using UnityEngine;
using TMPro;
using System.Collections;
using System.Collections.Generic;

public class EnemyFightHandler : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] public Enemy registeredEnemy;
    [SerializeField] private EnemyAnimControl enemyAnimControl;
    [SerializeField] internal Transform registeredEnemyParent;

    [SerializeField] private Doors fightingDoor;

    internal int dynamicEnemyCount = 0;

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

    private int CalculateDynamicEnemyCount()
    {
        int potentialMin = (int)(DoorBonusHandler.Instance.potentialMinCrowd * 0.6f);
        int potentialMax = (int)(DoorBonusHandler.Instance.potentialMaxCrowd * 0.4f);
        return Mathf.RoundToInt((potentialMin + potentialMax));
    }

    internal void RegisterEnemy(Enemy enemy, Transform enemyParent, TMP_Text enemyCrowdCounterText, Doors door)
    {
        registeredEnemy = enemy;
        registeredEnemyParent = enemyParent;

        dynamicEnemyCount = CalculateDynamicEnemyCount();
        AddRunners(dynamicEnemyCount);


        fightingDoor = door;
        fightingDoor.SetBonusType(BonusType.Difference, BonusType.Difference);
        fightingDoor.SetBonusAmount(dynamicEnemyCount +1 , dynamicEnemyCount + 1);
        fightingDoor.ConfigureDoors();
        DoorBonusHandler.Instance.doorsList.Add(fightingDoor);
    }

    private void AddRunners(float bonusAmount)
    {
        for (int i = 0; i < bonusAmount; i++)
        {
            Instantiate(enemyPrefab, registeredEnemyParent);
        }
    }

    public void RemoveRunners(Transform runnerToDestroy)
    {
        runnerToDestroy.SetParent(null);
        Destroy(runnerToDestroy.gameObject);
    }

    public void ResetEnemy()
    {
        if (registeredEnemy != null)
        {
            Destroy(registeredEnemy.gameObject);
        }
        registeredEnemy = null;
        registeredEnemyParent = null;
        DoorBonusHandler.Instance.EnemyDoorDeleter();
    }
    
    public void ResetEnemyAtEnd()
    {
        if (registeredEnemy != null)
        {
            Destroy(registeredEnemy.gameObject);
        }
        registeredEnemy = null;
        registeredEnemyParent = null;
    }
}