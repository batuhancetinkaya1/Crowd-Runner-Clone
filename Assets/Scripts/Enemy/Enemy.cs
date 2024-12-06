using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] internal Transform enemyParent;
    [SerializeField] private EnemyAnimControl enemyAnimControl;

    [Header("Text")]
    [SerializeField] private TMP_Text crowdCounterText;

    private void Awake()
    {
        EnemyFightHandler.Instance.RegisterEnemy(this, enemyParent, crowdCounterText);
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
        int runnersCount = enemyParent.childCount; // Runner say�s�
        int runnersPerRow = 10; // Her s�rada ka� runner olaca��

        float minX = -4.5f; // X eksenindeki minimum pozisyon
        float maxX = 4.5f;  // X eksenindeki maksimum pozisyon
        float zStart = 0f; // En uzak s�ran�n Z konumu
        float zStep = 1f; // S�ralar aras�ndaki mesafe (negatif, bize do�ru)

        float xStep = (maxX - minX) / (runnersPerRow - 1); // X eksenindeki mesafe

        for (int i = 0; i < runnersCount; i++)
        {
            Transform runner = enemyParent.GetChild(i);
            Transform enemy = runner.Find("Enemy");
            Animator enemyAnimator = enemy.GetComponent<Animator>();
            enemyAnimControl.FightPrep(enemyAnimator);

            int row = i / runnersPerRow; // Ka��nc� s�rada oldu�umuzu hesapla
            int col = i % runnersPerRow; // S�radaki konumu hesapla

            float x = maxX - col * xStep; // X konumu
            float z = zStart + row * zStep; // Z konumu (bize do�ru)

            runner.localPosition = new Vector3(x, 0, z); // Runner'� yeni pozisyona ta��
        }
    }

}
