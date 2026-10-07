using UnityEngine;

namespace Game.Scripts.Interactions
{
    [CreateAssetMenu(menuName = "Create InteractableObjectAsset", fileName = "InteractableObjectAsset", order = 0)]
    public class InteractableObjectAsset : ScriptableObject
    {
        [SerializeField] private string m_interactName;
        [Multiline]
        [SerializeField] private string m_interactDescription;
        
        public string GetInteractName()
        {
            return m_interactName;
        } 
        
        public string GetInteractDescription()
        {
            return m_interactDescription;
        } 
    }
}