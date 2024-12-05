using UnityEngine;

public class PlayerDetection : MonoBehaviour
{
    [SerializeField] PlayerCrowdSystemControl playerCrowdSystemControl;

    public void DetectDoor()
    {
        Collider[] detectCollider = Physics.OverlapSphere(transform.position, playerCrowdSystemControl.GetCrowRadius());

        if(detectCollider.Length > 0)
        {
            for(int i = 0; i < detectCollider.Length; i++)
            {
                if(detectCollider[i].TryGetComponent(out Doors door))
                {
                    int bonusAmount = door.GetBonusAmount(transform.position.x);
                    BonusType bonusType = door.GetBonusType(transform.position.x);

                    playerCrowdSystemControl.ApplyBonus(bonusType, bonusAmount);

                    door.GetComponent<Collider>().enabled = false;
                    DoorBonusHandler.Instance.RemoveFromList(door);
                }
            }
        }
    }

    public void DetecEnemy() 
    {

    }
}
