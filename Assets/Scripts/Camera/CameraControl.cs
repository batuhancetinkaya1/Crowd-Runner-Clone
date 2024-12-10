using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SocialPlatforms;

public class CameraControl : MonoBehaviour
{

    [Header("Target Reference")]
    [SerializeField] private Transform playerTransform;


    [Header("Offset Settings")]
    [SerializeField] private Vector3 runningOffset = new Vector3(0, 5, -10);
    [SerializeField] private float runRotationY = 0;
    [SerializeField] private float runRotationX = 45;


    [Header("Smoothing Parameters")]
    [SerializeField] private float smoothSpeed = 5f;
    [SerializeField] private bool lockY = true;


    [Header("FightingPhase")]
    [SerializeField] private Vector3 fightingOffset = new Vector3(5, 10, -5);
    [SerializeField] private float fightRotationY = -45;
    [SerializeField] private float fightRotationX = 45;

    private Vector3 runPosition;
    private Vector3 smoothedPosition;
    private Vector3 fightPosition;

    private void LateUpdate()
    {
        if (GameManager.Instance.CurrentState == GameManager.GameState.Game)
        {
            RunningPhase();
        }
        else if (GameManager.Instance.CurrentState == GameManager.GameState.FightPrep || GameManager.Instance.CurrentState == GameManager.GameState.Fight)
        {
            FightingPhase();
        }
    }

    private void RunningPhase()
    {
        runPosition = playerTransform.position + runningOffset;

        if (lockY)
        {
            runPosition.y = transform.position.y;
        }
        smoothedPosition = Vector3.Lerp(transform.position, runPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        Quaternion targetRotation = Quaternion.Euler(runRotationX, runRotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }

    private void FightingPhase()
    {
        fightPosition = playerTransform.position + fightingOffset;

        smoothedPosition = Vector3.Lerp(transform.position, fightPosition, smoothSpeed * Time.deltaTime);
        transform.position = smoothedPosition;

        Quaternion targetRotation = Quaternion.Euler(fightRotationX, fightRotationY, 0);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, smoothSpeed * Time.deltaTime);
    }
}
