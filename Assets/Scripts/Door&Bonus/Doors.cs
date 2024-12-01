using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor.UIElements;
using UnityEngine;

public class Doors : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private SpriteRenderer rightDoorRenderer;
    [SerializeField] private SpriteRenderer leftDoorRenderer;
    [SerializeField] private TMP_Text rightDoorText;
    [SerializeField] private TMP_Text leftDoorText;

    [Header("Settings")]
    [SerializeField] private BonusType rightDoorBonusType;
    [SerializeField] private int rightDoorAmount;

    [SerializeField] private BonusType leftDoorBonusType;
    [SerializeField] private int leftDoorAmount;

    [SerializeField] private Color bonusColor;
    [SerializeField] private Color penaltyColor;

    private void Awake()
    {
        // Register this door with the singleton DoorBonusHandler
        if (DoorBonusHandler.Instance != null)
        {
            DoorBonusHandler.Instance.RegisterDoor(this);
        }
        else
        {
            Debug.LogWarning("DoorBonusHandler instance not found when registering door!");
        }
    }

    private void Start()
    {
        ConfigureDoors();
    }

    private void ConfigureDoors()
    {
        switch (rightDoorBonusType)
        {
            case BonusType.Addition:
                rightDoorRenderer.color = bonusColor;
                rightDoorText.text = "+" + rightDoorAmount;
                break;
            case BonusType.Difference:
                rightDoorRenderer.color = penaltyColor;
                rightDoorText.text = "-" + rightDoorAmount;
                break;
            case BonusType.Product:
                rightDoorRenderer.color = bonusColor;
                rightDoorText.text = "x" + rightDoorAmount;
                break;
            case BonusType.Division:
                rightDoorRenderer.color = penaltyColor;
                rightDoorText.text = "/" + rightDoorAmount;
                break;
        }


        switch (leftDoorBonusType)
        {
            case BonusType.Addition:
                leftDoorRenderer.color = bonusColor;
                leftDoorText.text = "+" + leftDoorAmount;
                break;
            case BonusType.Difference:
                leftDoorRenderer.color = penaltyColor;
                leftDoorText.text = "-" + leftDoorAmount;
                break;
            case BonusType.Product:
                leftDoorRenderer.color = bonusColor;
                leftDoorText.text = "x" + leftDoorAmount;
                break;
            case BonusType.Division:
                leftDoorRenderer.color = penaltyColor;
                leftDoorText.text = "/" + leftDoorAmount;
                break;
        }
    }

    internal void SetBonusAmount(int rightAmount, int leftAmount)
    {
        rightDoorAmount = rightAmount;
        leftDoorAmount = leftAmount;
    }

    internal void SetBonusType(BonusType rightBonusType, BonusType leftBonusType)
    {
        rightDoorBonusType = rightBonusType;
        leftDoorBonusType = leftBonusType;
    }

    internal int GetBonusAmount(float x)
    {
        if(x > 0)
        {
            return rightDoorAmount;
        }
        else
        {
            return leftDoorAmount;
        }
    }

    internal BonusType GetBonusType(float x)
    {
        if (x > 0)
        {
            return rightDoorBonusType;
        }
        else
        {
            return leftDoorBonusType;
        }
    }
}
