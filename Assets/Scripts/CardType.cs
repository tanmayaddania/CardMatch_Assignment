using UnityEngine;

public enum CardType
{
    Apple,
    Banana,
    Mango,
    Watermelon,
    Grapes,
    Strawberry
}

[System.Serializable]
public struct CardData
{
    public CardType type;
    public Sprite frontSprite;
}