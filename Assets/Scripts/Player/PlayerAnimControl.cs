using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimControl : MonoBehaviour
{
    [SerializeField] private bool isSliding;
    public bool IsSliding => isSliding;


    public void SlidingTrue()
    {
        isSliding = true;
    }

    public void SlidingFalse()
    {
        isSliding = false;
    }
}
