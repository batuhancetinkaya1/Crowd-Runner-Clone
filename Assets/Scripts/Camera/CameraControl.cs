using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraControl : MonoBehaviour
{

    [Header("Target Reference")]
    [SerializeField] private Transform playerTransform;
    [Header("Offset Settings")]
    [SerializeField] private Vector3 offset = new Vector3(0, 5, -10);
    [Header("Smoothing Parameters")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private bool lockY = true;

    private Vector3 desiredPosition;
    private Vector3 smoothedPosition;

    private void LateUpdate()
    {
        if(GameManager.Instance.CurrentState == GameManager.GameState.Game)
        {
            RunningPhase();
        }
        else if(GameManager.Instance.CurrentState == GameManager.GameState.Fight)
        {
            FightingPhase();
        }
        
    }

    private void RunningPhase()
    {
        desiredPosition = playerTransform.position + offset;

        if (lockY)
        {
            desiredPosition.y = transform.position.y;
        }
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }

    private void FightingPhase()
    {
        Debug.Log("fighting from camera");
        desiredPosition = playerTransform.position + offset;

        if (lockY)
        {
            desiredPosition.y = transform.position.y;
        }
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;
    }
}
