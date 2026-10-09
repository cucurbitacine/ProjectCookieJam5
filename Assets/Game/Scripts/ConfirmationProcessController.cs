using System;
using System.Collections;
using Game.Scripts.Pawns;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Game.Scripts
{
    public class ConfirmationProcessController : MonoBehaviour
    {
        [SerializeField] private bool m_showing;
        
        [Space]
        [SerializeField] private InputActionReference m_confirmAction;
        [SerializeField] private InputActionReference m_cancelAction;
        
        [Space]
        [SerializeField] private UnityEvent<bool> m_onConfirmationStarted = new UnityEvent<bool>();
        
        private PawnController m_pawn;

        public event Action<bool> Confirmed;
        
        public void Show(PawnController pawn)
        {
            if (m_showing) return;
            m_showing = true;
            pawn.Mute(true);
            
            m_pawn = pawn;
            
            m_onConfirmationStarted.Invoke(true);

            StartCoroutine(OnNextFrame(SetCallbacks));
        }

        public void Confirm()
        {
            if (!m_showing) return;
            m_showing = false;
            m_pawn.Mute(false);
            
            Confirmed?.Invoke(true);
            
            m_onConfirmationStarted.Invoke(false);

            StartCoroutine(OnNextFrame(RemoveCallbacks));
        }

        public void Cancel()
        {
            if (!m_showing) return;
            m_showing = false;
            m_pawn.Mute(false);
            
            Confirmed?.Invoke(false);
            
            m_onConfirmationStarted.Invoke(false);
            
            StartCoroutine(OnNextFrame(RemoveCallbacks));
        }

        private IEnumerator OnNextFrame(Action action)
        {
            yield return new WaitForEndOfFrame();
            
            action?.Invoke();
        }
        
        private void OnConfirm(InputAction.CallbackContext context)
        {
            if (m_showing && context.ReadValueAsButton())
            {
                Confirm();
            } 
        }

        private void OnCancel(InputAction.CallbackContext context)
        {
            if (m_showing && context.ReadValueAsButton())
            {
                Cancel();
            } 
        }

        private void SetCallbacks()
        {
            m_confirmAction.action.started += OnConfirm;
            m_cancelAction.action.started += OnCancel;
        }

        private void RemoveCallbacks()
        {
            m_confirmAction.action.started -= OnConfirm;
            m_cancelAction.action.started -= OnCancel;
        }
    }
}