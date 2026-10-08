using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Game.Scripts.UI
{
    public class PointerEvent : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] private UnityEvent onEnter = new UnityEvent();
        [SerializeField] private UnityEvent onExit = new UnityEvent();

        public UnityEvent OnEnter => onEnter;
        public UnityEvent OnExit => onExit;

        public void OnPointerEnter(PointerEventData eventData)
        {
            OnEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit.Invoke();
        }
    }
}