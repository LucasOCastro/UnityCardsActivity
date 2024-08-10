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
        [SerializeField] private float dragCardHeightOffset;
        [SerializeField] private List<Card> cards = new();
        
        /// <summary> Índice da carta a qual o mouse está em cima, deve ser destacada. </summary>
        private int _highlightedIndex = -1;

        /// <summary> Posição local da carta sendo arrastada. </summary>
        private float? _draggedLocalX;
        
        /// <summary>
        /// A largura total, da borda esquerda da primeira carta à borda direita da última carta.
        /// </summary>
        private float TotalWidth => cards.Count * cardWidth + (cards.Count - 1) * cardSpacing;

        /// <summary>
        /// A distância entre um centro da carta e outro.
        /// </summary>
        private float OffsetBetweenCardCenters => cardWidth + cardSpacing;

        /// <summary>
        /// O centro da carta de índice 0.
        /// </summary>
        private float LeftmostCenterX => (-TotalWidth + cardWidth) * .5f; 
        
        /// <returns>
        /// O X em posição local da carta no índice passado.
        /// </returns>
        private float GetCenterXForCardIndex(int cardIndex) => LeftmostCenterX + cardIndex * OffsetBetweenCardCenters;
        
        private void Update()
        {
            PositionCards();
            _highlightedIndex = -1;
            _draggedLocalX = null;
        }

        /// <summary>
        /// Atualiza a posição de cada carta contida na linha,
        /// aplicando deslocamentos de destaque e seleção.
        /// </summary>
        private void PositionCards()
        {
            if (cards.Count == 0) return;
            
            Vector3 pos = new(LeftmostCenterX, 0, 0);
            Vector3 offsetBetweenCards = new(OffsetBetweenCardCenters, 0, 0);
            for (int i = 0; i < cards.Count; i++)
            {
                cards[i].AdjustToCardLine(this);
                var card = cards[i].transform;
                
                //Se tem uma carta sendo arrastada, afasta as cartas em volta pra mostrar onde vai ser inserido.
                if (_draggedLocalX == null)
                    card.localPosition = pos;
                else if (pos.x < _draggedLocalX)
                    card.localPosition = pos - new Vector3(cardWidth * .5f, 0, 0);
                else if (pos.x > _draggedLocalX)
                    card.localPosition = pos + new Vector3(cardWidth * .5f, 0, 0);
                
                //Se o mouse está em cima da carta, levanta ela um pouco pra indicar que vai ser selecionada.
                if (i == _highlightedIndex)
                    card.position -= transform.forward * hoverCardHeightOffset;

                pos += offsetBetweenCards;
            }
        }
        
        public void AddCard(Card newCard)
        {
            newCard.AdjustToCardLine(this);
            cards.Add(newCard);
        }

        #region MOUSE_HANDLING
        
        public void HandleMouse(Vector3 collisionPoint, bool click, ref Card dragged)
        {
            if (cards.Count == 0) return;
            
            // Calcular o índice da carta baseado na posição do mouse na linha
            float localX = transform.InverseTransformPoint(collisionPoint).x;
            float normalizedX = localX / TotalWidth + .5f;
            int index = (int)(normalizedX * cards.Count);

            if (dragged)
                HandleDragCard(localX, index, click, ref dragged);
            else 
                HandleHover(index, click, ref dragged);
        }
        
        private void HandleDragCard(float localX, int closestIndex, bool click, ref Card dragged) 
        {
            _draggedLocalX = localX;
            dragged.transform.localPosition = new(localX, 0, -dragCardHeightOffset);
            if (!click) return;
            
            //Se o mouse está à direita da carta mais próxima, vai inserir no próximo índice
            int newIndex = Mathf.Clamp(closestIndex, 0, cards.Count - 1);
            if (localX > GetCenterXForCardIndex(closestIndex))
                newIndex++;
            
            cards.Insert(newIndex, dragged); 
            dragged = null;
            _draggedLocalX = null;
        }

        private void HandleHover(int index, bool click, ref Card dragged)
        {
            if (index < 0 || index >= cards.Count) return;

            if (click)
            {
                dragged = cards[index];
                _draggedLocalX = GetCenterXForCardIndex(index);
                cards.RemoveAt(index);
            }
            else _highlightedIndex = index;
        }
        
        #endregion
    }
}