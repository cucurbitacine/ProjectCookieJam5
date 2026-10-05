using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Interactions
{
    [DisallowMultipleComponent]
    public class ToggleState : MonoBehaviour
    {
        [SerializeField] private bool m_toggle;
        [Space]
        [SerializeField] private UnityEvent turnOn = new UnityEvent();
        [SerializeField] private UnityEvent turnOff = new UnityEvent();

        public bool GetValue()
        {
            return m_toggle;
        }
        
        public void SetValue(bool value)
        {
            if (m_toggle == value) return;

            m_toggle = value;
            
            if (value)
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
            SetValue(!m_toggle);
        }
    }
}