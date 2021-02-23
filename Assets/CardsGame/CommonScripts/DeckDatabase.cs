using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bhabbhi;
[CreateAssetMenu(fileName = "DeckDatabase", menuName = "Cards Game/New DeckDataBase")]
public class DeckDatabase : ScriptableObject
{
    public List<CardDeck> cardDecks = new List<CardDeck>();
}
[System.Serializable]
public class CardDeck
{
    public string Name;
    public Sprite BackCover;
    public List<Card> Cards = new List<Card>();
}