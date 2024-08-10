using UnityEngine;

namespace CardGameActivity
{
    public class CardStack : MonoBehaviour
    {
        [SerializeField] private CardDeck deck;
        [SerializeField] private Card cardPrefab;

        public Card GetCard()
        {
            if (deck == null) return null;

            var def = deck.GetRandomCardDef();
            if (def == null) return null;

            var card = Instantiate(cardPrefab);
            card.Definition = def;
            return card;
        }
    }
}