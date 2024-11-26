using System.Collections.Generic;
using UnityEngine;

public class CardListByRarity
{
    public ERarity Rarity;
    public List<Card> Cards;
}

[CreateAssetMenu(fileName = "Database", menuName = "Scriptable Objects/Database")]
public class Database : ScriptableObject
{
    private Dictionary<int, Card> _cardDictionary = new Dictionary<int, Card>();
    private List<CardListByRarity> _cardsByRarity = new List<CardListByRarity>();

    public void Initialize(List<Card> cards)
    {
        foreach (Card card in cards)
        {
            _cardDictionary.Add(card.ID, card);

            CardListByRarity cardListByRarity = _cardsByRarity.Find(_ => _.Rarity == card.Rarity);

            if (cardListByRarity == null)
            {
                cardListByRarity = new CardListByRarity { Rarity = card.Rarity, Cards = new List<Card>() };
                _cardsByRarity.Add(cardListByRarity);
            }

            cardListByRarity.Cards.Add(card);
        }
    }

    public Card GetCard(int id)
    {
        return _cardDictionary[id];
    }

    public Card GetCard(ERarity rarity)
    {
        CardListByRarity cardListByRarity = _cardsByRarity.Find(_ => _.Rarity == rarity);

        if (cardListByRarity == null)
        {
            return null;
        }

        return cardListByRarity.Cards[Random.Range(0, cardListByRarity.Cards.Count)];
    }

    public List<Card> GetAllCards()
    {
        return new List<Card>(_cardDictionary.Values);
    }
}