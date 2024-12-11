using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoveControl : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] internal float speed = 5f;
    [SerializeField] private float slideSpeed = 5f;

    [SerializeField] private Vector3 currentPosition;

    [SerializeField] private PlayerInputControl playerInputControl;
    [SerializeField] private PlayerCrowdSystemControl playerCrowdSystemControl;

    [SerializeField] private int roadWidth = 10;

    public void MoveForward()
    {
        transform.position += Vector3.forward * speed * Time.deltaTime;
    }

    public void SlideSide()
    {
        float xScreenDifference = Input.mousePosition.x - playerInputControl.clickedScreenPosition.x;

        xScreenDifference /= Screen.width;
        xScreenDifference *= slideSpeed;

        currentPosition = transform.position;
        currentPosition.x = playerInputControl.clickedPlayerPosition.x + xScreenDifference;

        currentPosition.x = Mathf.Clamp(currentPosition.x, -roadWidth / 2 + playerCrowdSystemControl.GetCrowRadius(), roadWidth / 2 - playerCrowdSystemControl.GetCrowRadius());

        transform.position = currentPosition;
    }

    public void SetSpeed(float amount)
    {
        speed = amount;
    }
}
