using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] PlayerCrowdSystemControl playerCrowdSystemControl;

    public void DetectObjects()
    {
        // Yarýçap içerisindeki Collider'larý bul
        Collider[] detectColliders = Physics.OverlapSphere(transform.position, playerCrowdSystemControl.GetCrowRadius());

        if (detectColliders.Length > 0)
        {
            for (int i = 0; i < detectColliders.Length; i++)
            {
                // Eðer obje bir kapýysa
                if (detectColliders[i].TryGetComponent(out Doors door))
                {
                    int bonusAmount = door.GetBonusAmount(transform.position.x);
                    BonusType bonusType = door.GetBonusType(transform.position.x);

                    playerCrowdSystemControl.ApplyBonus(bonusType, bonusAmount);

                    door.GetComponent<Collider>().enabled = false;
                    DoorBonusHandler.Instance.RemoveFromList(door);
                }
                // Eðer obje bir düþmansa
                else if (detectColliders[i].CompareTag("Enemy"))
                {
                    GameManager.Instance.SetGameState(GameManager.GameState.FightPrep);
                    detectColliders[i].enabled = false;
                }
            }
        }
    }
}
