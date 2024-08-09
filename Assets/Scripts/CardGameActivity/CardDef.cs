using UnityEngine;

namespace CardGameActivity
{
    public class CardDef : ScriptableObject
    {
        [SerializeField] private string title;
        
        public string Title => title;
    }
}