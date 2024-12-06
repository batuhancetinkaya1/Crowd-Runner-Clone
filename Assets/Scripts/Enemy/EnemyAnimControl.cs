using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimControl : MonoBehaviour
{
    public void FightPrep(Animator animator)
    {
        IdleTrue(animator);
        RunFalse(animator);
        FightFalse(animator);
    }

    public void Fight(Animator animator)
    {
        IdleFalse(animator);
        RunFalse(animator);
        FightTrue(animator);
    }

    public void Run(Animator animator)
    {
        IdleFalse(animator);
        RunTrue(animator);
        FightFalse(animator);
    }

    private void RunTrue(Animator animator)
    {
        animator.SetBool("isRun", true);
    }

    private void RunFalse(Animator animator)
    {
        animator.SetBool("isRun", false);
    }

    private void FightTrue(Animator animator)
    {
        animator.SetBool("isFight", true);
    }

    private void FightFalse(Animator animator)
    {
        animator.SetBool("isFight", false);
    }

    private void IdleTrue(Animator animator)
    {
        animator.SetBool("isIdle", true);
    }

    private void IdleFalse(Animator animator)
    {
        animator.SetBool("isIdle", false);
    }
}
