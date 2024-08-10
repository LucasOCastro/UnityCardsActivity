using UnityEngine;

namespace CardGameActivity
{
    /// <summary>
    /// Representa um jogador para lidar com input.
    /// </summary>
    public class Player : MonoBehaviour
    {
        [SerializeField] private CardLine tableCards;
        [SerializeField] private CardLine handCards;
        
        [Header("Ray Casting")]
        [SerializeField] private Camera cam;
        [SerializeField] private float maxClickDistance = 100;
        [SerializeField] private LayerMask clickLayerMask;

        private Card _selectedCard;
        
        private void Update()
        {
            HandleMousePos();
        }

        private void HandleMousePos()
        {
            // TODO consider using Plane intersection
            var ray = cam.ScreenPointToRay(Input.mousePosition);
            if (!Physics.Raycast(ray, out var collision, maxClickDistance, clickLayerMask))
                return;

            if (collision.collider.TryGetComponent<CardLine>(out var cardLine))
            {
                HandleCardLineMousePos(cardLine, collision.point);
            }
            else if (collision.collider.TryGetComponent<CardStack>(out var stack))
            {
                HandleStackMousePos(stack);
            }
        }

        private void HandleStackMousePos(CardStack stack)
        {
            if (_selectedCard || !Input.GetMouseButtonDown(0)) return;

            var newCard = stack.GetCard();
            if (newCard == null) return;
            handCards.AddCard(newCard);
        }

        private void HandleCardLineMousePos(CardLine cardLine, Vector3 point)
        {
            bool isClick = Input.GetMouseButtonDown(0);
            if (_selectedCard) _selectedCard.AdjustToCardLine(cardLine);
            cardLine.HandleMouse(point, isClick, ref _selectedCard);
        }
    }
}