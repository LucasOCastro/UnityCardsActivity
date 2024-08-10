using TMPro;
using UnityEngine;

namespace CardGameActivity
{
    /// <summary>
    /// Script que representa uma instância real de um <see cref="cardDef"/>,
    /// renderizando as informações da definição.
    /// </summary>
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

        /// <summary>
        /// Troca o parent, rotaciona e escala a carta para adequar a uma fileira.
        /// </summary>
        public void AdjustToCardLine(CardLine line)
        {
            transform.parent = line.transform;
            transform.forward = line.transform.forward;
            transform.localScale = Vector3.one;
        }
    }
}