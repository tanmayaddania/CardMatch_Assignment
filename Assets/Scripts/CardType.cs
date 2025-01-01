using UnityEngine;

public enum CardType
{
    Apple,
    Tree,
    Candy,
    Watermelon
}

[System.Serializable]
public struct CardData
{
    public CardType type;
    public Sprite sprite;
}
