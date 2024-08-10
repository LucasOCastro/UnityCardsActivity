using System.Collections.Generic;
using UnityEngine;

namespace CardGameActivity
{
    [CreateAssetMenu(menuName = "Create Card Deck", fileName = "New CardDeck", order = 0)]
    public class CardDeck : ScriptableObject
    {
        [SerializeField] private List<CardDef> cards = new();
        
        public List<CardDef> Cards => cards;

        public CardDef GetRandomCardDef() => cards.Count > 0 ? cards[Random.Range(0, cards.Count)] : null;
    }
}