using UnityEngine;

namespace CardGameActivity
{
    /// <summary>
    /// Asset que armazena as informações e stats de uma carta.
    /// </summary>
    [CreateAssetMenu(menuName = "Create CardDef", fileName = "NewCard", order = 0)]
    public class CardDef : ScriptableObject
    {
        [SerializeField] private string title;
        
        public string Title => title;
    }
}