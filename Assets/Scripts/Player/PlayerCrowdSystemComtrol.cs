using System;
using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerCrowdSystemControl : MonoBehaviour
{
    [Header("Fermat Spiral Configuration")]
    [SerializeField] private float goldenAngle = 137.5f; // Golden angle in degrees
    [SerializeField] private float spreadFactor = 0.5f; // Controls the spread of the spiral
    [SerializeField] internal Transform runnerParent;
    [SerializeField] private GameObject runnerPrefab;

    [Header("Fight Formation Configuration")]
    [SerializeField] private PlayerAnimControl playerAnimControl;
    [SerializeField] private int runnersPerRow = 10;
    [SerializeField] private float minX = -4.5f; // X eksenindeki minimum pozisyon
    [SerializeField] private float maxX = 4.5f;  // X eksenindeki maksimum pozisyon
    [SerializeField] private float zStart = 0f; // En uzak sýranýn Z konumu
    [SerializeField] private float zStep = -1f;

    [Header("Text")]
    [SerializeField] private TMP_Text crowdCounterText;
    public static PlayerCrowdSystemControl Instance { get; private set; }

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
    public void CrowdDistribution(GameManager.GameState gameState)
    {
        switch (gameState)
        {
            case GameManager.GameState.Game:
                DistributeFermatSpiral();
                break;
            case GameManager.GameState.FightPrep:
                DistributeFightFormation();
                break;
        }
    }

    private void DistributeFermatSpiral()
    {
        for (int i = 0; i < runnerParent.childCount; i++)
        {
            runnerParent.GetChild(i).localPosition = GetRunnerLocalPosition(i);
        }
    }

    private void DistributeFightFormation()
    {

        this.transform.position = new Vector3(0, this.transform.position.y, this.transform.position.z);

        int runnersCount = runnerParent.childCount; // Runner sayýsý

        float xStep = (maxX - minX) / (runnersPerRow - 1); // X eksenindeki mesafe

        for (int i = 0; i < runnersCount; i++)
        {
            Transform runner = runnerParent.GetChild(i); // i. Runner nesnesini al
            Transform player = runner.Find("Player"); // Runner altýnda "Player" adlý Transform'u bul
            Animator playerAnimator = player.GetComponent<Animator>(); // Player'ýn Animator bileþenine eriþ
            playerAnimControl.FightPrep(playerAnimator); // Animator'ý iþleme gönder


            int row = i / runnersPerRow; // Kaçýncý sýrada olduðumuzu hesapla
            int col = i % runnersPerRow; // Sýradaki konumu hesapla

            float x = minX + col * xStep; // X konumu
            float z = zStart + row * zStep; // Z konumu (bize doðru)

            runner.localPosition = new Vector3(x, 0, z); // Runner'ý yeni pozisyona taþý
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
        CrowdDistribution(GameManager.GameState.Game); // Immediately redistribute
    }

    public void ModifyGoldenAngle(float angleChange = -0.01f)
    {
        goldenAngle += angleChange;
        CrowdDistribution(GameManager.GameState.Game); // Immediately redistribute
    }

    public void CrowdCounterTextUpdater()
    {
        crowdCounterText.text = runnerParent.childCount.ToString();
    }

    public void ApplyBonus(BonusType bonusType, int bonusAmount)
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
        for (int i = 0; i < bonusAmount; i++)
        {
            Instantiate(runnerPrefab, runnerParent);
        }
    }

    private void RemoveRunners(float bonusAmount)
    {
        if (bonusAmount > runnerParent.childCount)
        {
            bonusAmount = runnerParent.childCount;
        }
        int runnerAmount = runnerParent.childCount;
        for (int i = runnerAmount - 1; i >= runnerAmount - bonusAmount + 1; i--)
        {
            Transform runnerToDestroy = runnerParent.GetChild(i);
            runnerToDestroy.SetParent(null);
            Destroy(runnerToDestroy.gameObject);
        }
    }

    internal int GetCrowdCount()
    {
        return runnerParent.childCount;
    }
}