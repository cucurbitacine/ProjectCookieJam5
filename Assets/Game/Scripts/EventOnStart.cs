using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    [DisallowMultipleComponent]
    public class EventOnStart : MonoBehaviour
    {
        [SerializeField] private UnityEvent onStart = new UnityEvent();
        
        private void Start()
        {
            onStart.Invoke();
        }
    }
}