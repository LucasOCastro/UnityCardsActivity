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
        
        private readonly List<Card> _cards = new();
        
        /// <summary> Índice da carta a qual o mouse está em cima, deve ser destacada. </summary>
        private int _highlightedIndex = -1;

        /// <summary> Posição local da carta sendo arrastada. </summary>
        private float? _draggedLocalX;
        
        /// <summary>
        /// A largura total, da borda esquerda da primeira carta até a borda direita da última carta.
        /// </summary>
        private float TotalWidth => _cards.Count * cardWidth + (_cards.Count - 1) * cardSpacing;

        /// <summary>
        /// A distância entre um centro da carta e outro.
        /// </summary>
        private float OffsetBetweenCardCenters => cardWidth + cardSpacing;

        /// <summary>
        /// O centro da carta de índice 0.
        /// </summary>
        private float LeftmostCenterX => (-TotalWidth + cardWidth) * .5f; 
        
        /// <returns>
        /// O X em posição local da carta no índice.
        /// </returns>
        private float GetCenterXForCardIndex(int cardIndex) => LeftmostCenterX + cardIndex * OffsetBetweenCardCenters;

        private void Awake()
        {
            for (int i = 0; i < transform.childCount; i++)
                if (transform.GetChild(i).TryGetComponent<Card>(out var card))
                    _cards.Add(card);
        }

        private void Update()
        {
            PositionCards();
            _highlightedIndex = -1;
            _draggedLocalX = null;
        }

        /// <summary>
        /// Atualiza a posição de cada carta contida na linha, aplicando offsets de destaque.
        /// </summary>
        private void PositionCards()
        {
            if (_cards.Count == 0) return;
            
            Vector3 pos = new(LeftmostCenterX, 0, 0);
            Vector3 offsetBetweenCards = new(OffsetBetweenCardCenters, 0, 0);
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].AdjustToCardLine(this);
                var card = _cards[i].transform;
                
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
            _cards.Add(newCard);
        }

        #region MOUSE_HANDLING
        
        public void HandleMouse(Vector3 collisionPoint, bool click, ref Card dragged)
        {
            // Calcular o índice da carta baseado na posição do mouse na linha
            float localX = transform.InverseTransformPoint(collisionPoint).x;
            float normalizedX = localX / TotalWidth + .5f;
            int index = (int)(normalizedX * _cards.Count);

            if (dragged)
                HandleDragCard(localX, index, click, ref dragged);
            else
                HandleHover(index, click, ref dragged);
        }
        
        /// <summary>
        /// Lida com quando uma carta está sendo arrastada pela linha
        /// para ser inserida em alguma posição, e a insere em caso de click.
        /// </summary>
        private void HandleDragCard(float localX, int closestIndex, bool click, ref Card dragged) 
        {
            _draggedLocalX = localX;
            dragged.transform.localPosition = new(localX, 0, -dragCardHeightOffset);
            if (!click) return;

            if (_cards.Count > 0)
            {
                //Se o mouse está à direita da carta mais próxima, vai inserir no próximo índice
                int newIndex = Mathf.Clamp(closestIndex, 0, _cards.Count - 1);
                if (localX > GetCenterXForCardIndex(newIndex))
                    newIndex++;
                _cards.Insert(newIndex, dragged);
            }
            else _cards.Add(dragged);
            
            dragged = null;
            _draggedLocalX = null;
        }

        /// <summary>
        /// Lida com quando o mouse está sendo passado em cima das cartas para destacar
        /// a carta que será selecionada, e selecioná-la em caso de click.
        /// </summary>
        private void HandleHover(int index, bool click, ref Card dragged)
        {
            if (index < 0 || index >= _cards.Count) return;

            if (click)
            {
                dragged = _cards[index];
                _draggedLocalX = GetCenterXForCardIndex(index);
                _cards.RemoveAt(index);
            }
            else _highlightedIndex = index;
        }
        
        #endregion
    }
}