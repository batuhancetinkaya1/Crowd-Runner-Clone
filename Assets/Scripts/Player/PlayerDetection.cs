using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] PlayerCrowdSystemControl playerCrowdSystemControl;

    public void DetectObjects()
    {
        // Yar��ap i�erisindeki Collider'lar� bul
        Collider[] detectColliders = Physics.OverlapSphere(transform.position, playerCrowdSystemControl.GetCrowRadius());

        if (detectColliders.Length > 0)
        {
            for (int i = 0; i < detectColliders.Length; i++)
            {
                // E�er obje bir kap�ysa
                if (detectColliders[i].TryGetComponent(out Doors door))
                {
                    int bonusAmount = door.GetBonusAmount(transform.position.x);
                    BonusType bonusType = door.GetBonusType(transform.position.x);

                    playerCrowdSystemControl.ApplyBonus(bonusType, bonusAmount);

                    door.GetComponent<Collider>().enabled = false;
                    DoorBonusHandler.Instance.RemoveFromList(door);
                }
                // E�er obje bir d��mansa
                else if (detectColliders[i].CompareTag("Enemy"))
                {
                    GameManager.Instance.SetGameState(GameManager.GameState.FightPrep);
                    detectColliders[i].enabled = false;
                }
            }
        }
    }
}
