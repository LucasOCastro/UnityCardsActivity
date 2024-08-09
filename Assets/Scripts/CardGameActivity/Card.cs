using TMPro;
using UnityEngine;

namespace CardGameActivity
{
    public class Card : MonoBehaviour
    {
        [SerializeField] private TextMeshPro titleText;
        
        [SerializeField] private CardDef cardDef;

        public CardDef Definition
        {
            get => cardDef;
            set
            {
                cardDef = value;
                UpdateCard();
            }
        }

        private void Awake()
        {
            UpdateCard();
        }

        private void UpdateCard()
        {
            if (cardDef == null) return;
            
            titleText.text = cardDef.Title;
        }
    }
}