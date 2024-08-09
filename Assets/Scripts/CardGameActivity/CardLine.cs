using System.Collections.Generic;
using UnityEngine;

namespace CardGameActivity
{
    public class CardLine : MonoBehaviour
    {
        [SerializeField] private float cardWidth;
        [SerializeField] private float cardSpacing;
    
        [SerializeField]
        private List<Transform> cards = new();

        private void Update()
        {
            PositionCards();
        }

        private void PositionCards()
        {
            if (cards.Count == 0) return;
            
            float totalWidth = cards.Count * cardWidth + (cards.Count - 1) * cardSpacing;
            float leftmostX = (-totalWidth + cardWidth) * .5f;
            Vector3 pos = new(leftmostX, 0, 0);
            Vector3 offsetBetweenCards = new(cardWidth + cardSpacing, 0, 0);
            foreach (var card in cards)
            {
                card.transform.localPosition = pos;
                card.forward = transform.forward;
                pos += offsetBetweenCards;
            }
        }
    }
}