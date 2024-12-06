using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    [SerializeField] private bool isSliding;
    [SerializeField] private bool isRun;
    [SerializeField] private bool isFight;
    [SerializeField] private bool isIdle;
    public bool IsSliding => isSliding;

    public void SlidingTrue()
    {
        isSliding = true;
    }

    public void SlidingFalse()
    {
        isSliding = false;
    }

    public void RunTrue()
    {

    }

    public void RunFalse()
    {

    }

    public void FightTrue()
    {
        
    }

    public void FightFalse()
    {

    }

    public void IdleTrue()
    {

    }

    public void IdleFalse()
    {

    }
}
