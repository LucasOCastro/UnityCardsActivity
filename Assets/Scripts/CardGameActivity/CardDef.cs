using UnityEngine;

namespace CardGameActivity
{
    [CreateAssetMenu(menuName = "Create CardDef", fileName = "NewCard", order = 0)]
    public class CardDef : ScriptableObject
    {
        [SerializeField] private string title;
        
        public string Title => title;
    }
}