using UnityEngine;

namespace CardGameActivity
{
    /// <summary>
    /// Script que armazena um <see cref="CardDeck"/> para providenciar instâncias de cartas.
    /// </summary>
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