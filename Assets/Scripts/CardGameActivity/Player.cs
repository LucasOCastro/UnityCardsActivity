using UnityEngine;

namespace CardGameActivity
{
    /// <summary>
    /// Represents a player in the game world, containing its camera, cards and handling input.
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

            if (!collision.collider.gameObject.TryGetComponent<CardLine>(out var cardLine))
                return;

            bool isClick = Input.GetMouseButtonDown(0);
            if (_selectedCard) _selectedCard.AdjustToCardLine(cardLine);
            cardLine.HandleMouse(collision.point, isClick, ref _selectedCard);
        }
    }
}