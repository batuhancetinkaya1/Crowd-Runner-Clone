using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Fermat Spiral Configuration")]
    [SerializeField] internal Transform enemyParent;
    [SerializeField] internal EnemyAnimControl enemyAnimControl;
    [SerializeField] private float goldenAngle = 137.5f; // Golden angle in degrees
    [SerializeField] private float spreadFactor = 0.5f; // Controls the spread of the spiral
    [SerializeField] float speed = 1f;
    [SerializeField] private Doors door;

    [Header("Text")]
    [SerializeField] private TMP_Text crowdCounterText;

    private void Awake()
    {
        EnemyFightHandler.Instance.RegisterEnemy(this, enemyParent, crowdCounterText, door);
        CrowdCounterTextUpdater();
    }

    public void MoveForward()
    {
        transform.position -= Vector3.forward * speed * Time.deltaTime;
    }

    public void CrowdDistribution(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.Game:
                DistributeFermatSpiral();
                break;
            case GameManager.GameState.FightPrep:
                DistributeFermatSpiral();
                break;
            case GameManager.GameState.Fight:
                DistributeFermatSpiral();
                break;
        }
    }

    private void DistributeFermatSpiral()
    {
        for (int i = 0; i < enemyParent.childCount; i++)
        {
            enemyParent.GetChild(i).localPosition = GetRunnerLocalPosition(i);
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

    public void ModifyGoldenAngle(float angleChange = -0.01f)
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Game)
        {
            goldenAngle += angleChange;
            CrowdDistribution(GameManager.GameState.Game); // Immediately redistribute
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameState.Fight)
        {
            goldenAngle = 137.5f;
            CrowdDistribution(GameManager.GameState.Fight); // Immediately redistribute
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameState.FightPrep)
        {
            goldenAngle = 137.5f;
            CrowdDistribution(GameManager.GameState.FightPrep);
        }
    }

    public void DetectObjects()
    {
        // Yarýçap içerisindeki Collider'larý bul
        Collider[] detectColliders = Physics.OverlapSphere(transform.position, GetCrowRadius() + 30);

        if (detectColliders.Length > 0)
        {
            for (int i = 0; i < detectColliders.Length; i++)
            {
                if (detectColliders[i].CompareTag("Player"))
                {
                    GameManager.Instance.SetGameState(GameManager.GameState.FightPrep);
                }
            }
        }
    }

    public float GetCrowRadius()
    {
        float radius = spreadFactor * Mathf.Sqrt(GetCrowdCount());
        return radius;
    }

    public void CrowdCounterTextUpdater()
    {
        crowdCounterText.text = enemyParent.childCount.ToString();
    }

    internal int GetCrowdCount()
    {
        return enemyParent.childCount;
    }

    public void SetSpeed(float amount)
    {
        speed = amount;
    }
}
