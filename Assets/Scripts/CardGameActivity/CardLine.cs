using System.Collections.Generic;
using UnityEngine;

namespace CardGameActivity
{
    public class CardLine : MonoBehaviour
    {
        [SerializeField] private float cardWidth;
        [SerializeField] private float cardSpacing;
    
        private readonly List<Transform> _cards = new();

        private void Awake()
        {
            //TODO debug
            _cards.AddRange(transform.GetComponentsInChildren<Transform>());
            _cards.Remove(transform);
        }

        private void Update()
        {
            PositionCards();
        }

        private void PositionCards()
        {
            if (_cards.Count == 0) return;
            
            float totalWidth = _cards.Count * cardWidth + (_cards.Count - 1) * cardSpacing;
            float leftmostX = (-totalWidth + cardWidth) * .5f;
            Vector2 pos = new(leftmostX, 0);
            Vector2 offsetBetweenCards = new(cardWidth + cardSpacing, 0);
            foreach (var card in _cards)
            {
                card.transform.localPosition = pos;
                pos += offsetBetweenCards;
            }
        }
    }
}