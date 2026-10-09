using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.UI
{
    public class MouseEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent onEnter = new UnityEvent();
        [SerializeField] private UnityEvent onExit = new UnityEvent();

        public UnityEvent OnEnter => onEnter;
        public UnityEvent OnExit => onExit;
        
        private void OnMouseEnter()
        {
            OnEnter.Invoke();
        }

        private void OnMouseExit()
        {
            OnExit.Invoke();
        }
    }
}