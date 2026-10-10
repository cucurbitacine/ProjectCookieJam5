using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Interactions.Impl
{
    public class ActionObjectEvent : MonoBehaviour
    {
        [SerializeField] private UnityEvent onAttempt = new UnityEvent();
        [SerializeField] private UnityEvent onSuccess = new UnityEvent();
        [SerializeField] private UnityEvent onFailure = new UnityEvent();

        private ActionObject m_actionObject;

        private void OnAttempt() => onAttempt.Invoke();
        private void OnSuccess() => onSuccess.Invoke();
        private void OnFailure() => onFailure.Invoke();
        
        private void Awake()
        {
            m_actionObject = GetComponentInParent<ActionObject>();
        }

        private void OnEnable()
        {
            if (m_actionObject)
            {
                m_actionObject.onAttempt += OnAttempt;
                m_actionObject.onSuccess += OnSuccess;
                m_actionObject.onFailure += OnFailure;
            }
        }

        private void OnDisable()
        {
            if (m_actionObject)
            {
                m_actionObject.onAttempt -= OnAttempt;
                m_actionObject.onSuccess -= OnSuccess;
                m_actionObject.onFailure -= OnFailure;
            }
        }
    }
}