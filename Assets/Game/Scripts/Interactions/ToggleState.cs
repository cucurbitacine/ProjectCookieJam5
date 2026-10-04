using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Interactions
{
    [DisallowMultipleComponent]
    public class ToggleState : MonoBehaviour
    {
        [SerializeField] private bool toggle;
        [Space]
        [SerializeField] private UnityEvent turnOn = new UnityEvent();
        [SerializeField] private UnityEvent turnOff = new UnityEvent();

        public void Toggle(bool value)
        {
            if (toggle == value) return;
            toggle = value;
            if (toggle)
            {
                turnOn.Invoke();
            }
            else
            {
                turnOff.Invoke();
            }
        }

        public void Switch()
        {
            Toggle(!toggle);
        }
    }
}