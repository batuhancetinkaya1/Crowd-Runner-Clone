using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerResetSystem : MonoBehaviour
{
    [SerializeField] private PlayerMoveControl playerMoveControl;
    [SerializeField] private PlayerFightHandler playerFightHandler;


    public void PlayerReset()
    {
        transform.position = Vector3.zero;

        playerMoveControl.SetSpeed(5f);

        playerFightHandler.isFighting = false;

        PlayerCrowdSystemControl.Instance.ApplyBonus(BonusType.Addition, 1);
        //PlayerCrowdSystemControl.Instance.CrowdCounterTextUpdater2();
        //PlayerCrowdSystemControl.Instance.CrowdCounterTextUpdater();
    }
}
