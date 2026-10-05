using System;
using UnityEngine;

namespace Game.Scripts.Interactions
{
    [DisallowMultipleComponent]
    public class InteractableObject : MonoBehaviour
    {
        [Header("Interactable")]
        [SerializeField] private bool m_hovered;
        [SerializeField] private bool m_selected;
        [SerializeField] private bool m_interacted;

        public bool IsHovered => m_hovered;
        public bool IsSelected => m_selected;
        public bool IsInteracted => m_interacted;
        
        public Action<GameObject, InteractState, bool> StateChanged;

        public virtual bool HasAccess(GameObject actor)
        {
            return true;
        }
        
        public void Hover(GameObject actor, bool value)
        {
            m_hovered = value;

            OnHover(actor, value);
            
            StateChanged?.Invoke(actor, InteractState.Hover, value);
        }

        public void Select(GameObject actor, bool value)
        {
            m_selected = value;
            
            OnSelect(actor, value);
            
            StateChanged?.Invoke(actor, InteractState.Select, value);
        }

        public void Interact(GameObject actor, bool value)
        {
            m_interacted = value;
            
            OnInteract(actor, value);
            
            StateChanged?.Invoke(actor, InteractState.Interact, value);
        }
        
        protected virtual void OnHover(GameObject actor, bool value) { }
        protected virtual void OnSelect(GameObject actor, bool value) { }
        protected virtual void OnInteract(GameObject actor, bool value) { }
    }
    
    public enum InteractState
    {
        Hover,
        Select,
        Interact,
    }
}