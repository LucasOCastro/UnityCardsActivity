using System.Collections.Generic;
using UnityEngine;

namespace CardGameActivity
{
    /// <summary>
    /// Classe responsável pelo posicionamento de uma sequência de cartas.
    /// </summary>
    public class CardLine : MonoBehaviour
    {
        [SerializeField] private float cardWidth;
        [SerializeField] private float cardSpacing;
        [SerializeField] private float hoverCardHeightOffset;
    
        [SerializeField] private List<Card> cards = new();

        private int _highlightedIndex = -1;
        
        private float TotalWidth => cards.Count * cardWidth + (cards.Count - 1) * cardSpacing;

        private void Update()
        {
            PositionCards();
            _highlightedIndex = -1;
        }

        private void PositionCards()
        {
            if (cards.Count == 0) return;
            
            // transform.forward = lado oposto às frentes das cartas
            // A frente das cartas está virada para -transform.forward
            float leftmostX = (-TotalWidth + cardWidth) * .5f;
            Vector3 pos = new(leftmostX, 0, 0);
            Vector3 offsetBetweenCards = new(cardWidth + cardSpacing, 0, 0);
            for (int i = 0; i < cards.Count; i++)
            {
                var card = cards[i].transform;
                
                card.localPosition = pos;
                if (i == _highlightedIndex) 
                    card.position -= transform.forward * hoverCardHeightOffset;
                card.forward = transform.forward;

                pos += offsetBetweenCards;
            }
        }

        public void HandleMouse(Vector3 collisionPoint, bool click)
        {
            if (cards.Count == 0) return;
            
            // Calcular o índice da carta baseado na posição relativa do mouse à linha
            float localX = transform.InverseTransformPoint(collisionPoint).x;
            float normalizedX = localX / TotalWidth + .5f;
            int index = (int)(normalizedX * cards.Count);
            if (index < 0 || index >= cards.Count) return;

            // Destacar a carta e executar a ação de click
            _highlightedIndex = index;
            if (click)
            {
                //TODO debug
                Destroy(cards[index].gameObject);
                cards.RemoveAt(index);
            }
        }
    }
}