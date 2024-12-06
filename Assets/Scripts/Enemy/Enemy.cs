using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private Transform enemyParent;
    [SerializeField] private GameObject enemyPrefab;

    private void Awake()
    {
        EnemyFightHandler.Instance.RegisterEnemy(this);
    }

    public void CrowdDistribution(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.FightPrep:
                DistributeFightFormation();
                break;
        }
    }

    private void DistributeFightFormation()
    {
        Debug.Log("fight from playercrowdsystem");

        int runnersCount = enemyParent.childCount; // Runner say�s�
        int runnersPerRow = 10; // Her s�rada ka� runner olaca��

        float minX = -4.5f; // X eksenindeki minimum pozisyon
        float maxX = 4.5f;  // X eksenindeki maksimum pozisyon
        float zStart = enemyParent.position.z; // En uzak s�ran�n Z konumu
        float zStep = 1f; // S�ralar aras�ndaki mesafe (negatif, bize do�ru)

        float xStep = (maxX - minX) / (runnersPerRow - 1); // X eksenindeki mesafe

        for (int i = 0; i < runnersCount; i++)
        {
            Transform runner = enemyParent.GetChild(i);

            int row = i / runnersPerRow; // Ka��nc� s�rada oldu�umuzu hesapla
            int col = i % runnersPerRow; // S�radaki konumu hesapla

            float x = maxX - col * xStep; // X konumu
            float z = zStart + row * zStep; // Z konumu (bize do�ru)

            runner.localPosition = new Vector3(x, 0, z); // Runner'� yeni pozisyona ta��
        }
    }

}
