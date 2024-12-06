using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyFightHandler : MonoBehaviour
{
    public static EnemyFightHandler Instance { get; private set; }

    private void Awake()
    {

    }
    private void Start()
    {

    }

    internal void SetStatu(GameManager.GameState gameState)
    {

    }

    internal void RegisterEnemy(Enemy enemy)
    {

    }

    public void SpawnEnemy()
    {

    }

    public void FightPrep(Enemy enemy)
    {
        enemy.CrowdDistribution(GameManager.GameState.FightPrep);
    }

    public void FightEnemy()
    {

    }
}
