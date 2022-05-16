using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Text;
using System.Threading.Tasks;


public static class CardsEvents
{
    public static LoadSpriteEnd loadSpriteEnd;
    public static GenerateCards generateCards;
    public static ButtonClickMagic buttonClickMagic;
    public static UpdateCardPosition updateCardPosition;
    public static OnDestroyCard destroyCard;
    public static OnRestart restartGame;
    public static LockedInput lockedInput;
    public static InsertCardOnTable insertCardOnTable;

    public delegate void LoadSpriteEnd();
    public delegate void GenerateCards(int countCards);
    public delegate void ButtonClickMagic();
    public delegate void UpdateCardPosition();
    public delegate void OnDestroyCard(GameObject card);
    public delegate void OnRestart();
    public delegate void LockedInput(bool value);
    public delegate void InsertCardOnTable(GameObject card);
}
 