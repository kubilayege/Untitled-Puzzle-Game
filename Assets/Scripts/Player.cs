using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public static UnityEvent onPlayerTryHold;
    public static UnityEvent PlayerHolding;
    public static UnityEvent onPlayerRelease;
    public static UnityAction nPlayerRelease;
    public static Player main;
    public int Gold;
    public int goldAccuiredThisLevel;
    public int comboCount;
    public int lastComboCount;
    public int lastGainedAmount;
    public int lastBreakCount;
    public int breakCount;
    public enum States
    {
        Empty,
        Holding,

        UI
    }

    void Awake()
    {
        main = this;
        GameManager.holdingObject = null;
        onPlayerTryHold = new UnityEvent();
        PlayerHolding = new UnityEvent();
        onPlayerRelease = new UnityEvent();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.playerState == States.Empty && Input.GetMouseButtonDown(0))
        {
            onPlayerTryHold.Invoke();
        }
        if (GameManager.playerState == States.Holding && Input.GetMouseButton(0))
        {
            PlayerHolding.Invoke();
        }
        if (GameManager.playerState == States.Holding && Input.GetMouseButtonUp(0))
        {
            onPlayerRelease.Invoke();
        }
    }

    public void UpdatePlayerGold(int amount)
    {
        if (amount != 0)
        {
            Gold += amount;

            PlayerDataController.SaveData("gold", Gold);
            UIManager.instance.UpdateGoldText();
        }
    }

    public void ResetCombo()
    {
        comboCount = 0;
        breakCount = 0;
    }

    public void ApplyAndResetGoldInfo()
    {
        goldAccuiredThisLevel = 0;
        comboCount = 0;
        breakCount = 0;

    }
}