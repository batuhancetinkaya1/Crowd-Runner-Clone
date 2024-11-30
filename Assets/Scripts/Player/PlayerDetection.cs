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
                if(detectCollider[i].TryGetComponent(out Doors doors))
                {
                    int bonusAmount = doors.GetBonusAmount(transform.position.x);
                    BonusType bonusType = doors.GetBonusType(transform.position.x);

                    playerCrowdSystemControl.ApplyBonus(bonusType, bonusAmount);

                    doors.GetComponent<Collider>().enabled = false;
                }
            }
        }
    }
}
