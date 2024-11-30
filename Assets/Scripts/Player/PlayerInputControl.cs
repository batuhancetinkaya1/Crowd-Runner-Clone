using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInputControl : MonoBehaviour
{
    [SerializeField] private PlayerMoveControl playerMoveControl;
    [SerializeField] private PlayerAnimControl playerAnimControl;

    [SerializeField] public Vector3 clickedPlayerPosition;
    [SerializeField] public Vector3 clickedScreenPosition;

    public void FirstClickHandler()
    {
        clickedScreenPosition = Input.mousePosition;
        clickedPlayerPosition = transform.position;
    }

    public void LineChangeDemandHandler()
    {
        playerAnimControl.SlidingTrue();
        playerMoveControl.SlideSide();
    }

    public void LineChangeDemandFinisher()
    {
        playerAnimControl.SlidingFalse();
    }

    public void NormalRun()
    {
        playerMoveControl.MoveForward();
    }

}
