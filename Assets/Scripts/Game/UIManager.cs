using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    [Header("UI Elements")]
    [SerializeField] private GameObject deathPanel;
    //[SerializeField] private Button restartButton;
    //[SerializeField] private PlayerResetSystem playerResetSystem;

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

    private void Start()
    {
        //if (restartButton != null)
        //{
        //    restartButton.onClick.AddListener(OnRestartButtonPressed);
        //}

        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }

    public void ShowDeathPanel()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(true);
        }
    }

    public void HideDeathPanel()
    {
        if (deathPanel != null)
        {
            deathPanel.SetActive(false);
        }
    }

    public void OnRestartButtonPressed()
    {
        GameResetManager.Instance.ResetGame();
    }
}