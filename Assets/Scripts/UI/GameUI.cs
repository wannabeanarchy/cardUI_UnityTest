using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    [SerializeField] private Button buttonMagic;

    private int cardsCount;
    private int loadSpritesCount;

    private bool isLockedInput = false;
     
    void Start()
    { 
        CardsEvents.generateCards += SetCardsCount;
        CardsEvents.lockedInput += SetLockedInput;
    }
    private void OnDestroy()
    {
        CardsEvents.generateCards -= SetCardsCount;
        CardsEvents.lockedInput -= SetLockedInput;
    }

    private void SetCardsCount(int value)
    {
        cardsCount = value;

        if (cardsCount == 0)
            buttonMagic.interactable = false;
    }

    private void SetLockedInput(bool value)
    {
        isLockedInput = value;
    }

    public void OnMagicButtonClick()
    {
        if (isLockedInput)
            return;

        SetLockedInput(true);
        CardsEvents.buttonClickMagic?.Invoke();
    }

    public void OnRestartButtonClick()
    {
        if (isLockedInput)
            return;

        SetLockedInput(true);
        buttonMagic.interactable = true;
        CardsEvents.restartGame?.Invoke();
    }
}
