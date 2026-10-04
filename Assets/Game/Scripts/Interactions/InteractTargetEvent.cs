using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Interactions
{
    [RequireComponent(typeof(InteractTarget))]
    public class InteractTargetEvent : MonoBehaviour
    {
        [SerializeField] private InteractState m_interactState;
        [Space]
        [SerializeField] private UnityEvent<GameObject> m_onEnter = new UnityEvent<GameObject>();
        [SerializeField] private UnityEvent<GameObject> m_onExit = new UnityEvent<GameObject>();
        
        private InteractTarget m_target;


        private void OnStateChanged(GameObject actor, InteractState state, bool value)
        {
            if (m_interactState != state) return;
            
            if (value)
            {
                m_onEnter?.Invoke(actor);
            }
            else
            {
                m_onExit?.Invoke(actor);
            }
        }
        
        private void Awake()
        {
            m_target = GetComponent<InteractTarget>();
        }

        private void OnEnable()
        {
            m_target.StateChanged += OnStateChanged;
        }
        
        private void OnDisable()
        {
            m_target.StateChanged -= OnStateChanged;
        }
    }
}