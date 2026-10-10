using System;
using System.Collections;
using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class ActionObject : InteractableObject
    {
        [Header("Action")] 
        [SerializeField] private bool m_donePerGame;
        [SerializeField] private bool m_donePerDay;
        
        private ActionObjectAsset m_actionStats;
        private Coroutine m_interacting;

        public event Action onAttempt;
        public event Action onSuccess;
        public event Action onFailure;
        
        public ActionObjectAsset GetActionPreset()
        {
            if (m_actionStats == null)
            {
                if (GetInteractPreset() is ActionObjectAsset asset)
                {
                    m_actionStats = asset;
                }
            }
            return m_actionStats;
        }

        public bool HasDonePerDayFlag()
        {
            return m_donePerDay;
        }
        
        public bool HasDonePerGameFlag()
        {
            return m_donePerGame;
        }
        
        public bool HasDoneFlag()
        {
            return HasDonePerDayFlag() || HasDonePerGameFlag();
        }

        public override bool HasAccess(GameObject actor)
        {
            if (HasDoneFlag()) return false;

            /*
            if (actor.TryGetComponent(out PawnController pawn))
            {
                var actionStats = GetActionPreset();
                if (!actionStats.CanCharge(pawn))
                {
                    return false;
                }
            }
            */
            
            return base.HasAccess(actor);
        }

        private IEnumerator Interacting(PawnController pawn)
        {
            var actionStats = GetActionPreset();
            
            if (actionStats && !HasDoneFlag() && actionStats.CanCharge(pawn))
            {
                var diceProcess = pawn.GetDice().CreateDiceProcess(actionStats);
                yield return StartCoroutine(diceProcess.Process());

                if (!diceProcess.Canceled)
                {
                    if (actionStats.OncePerDay()) m_donePerDay = true;
                    
                    actionStats.Charge(pawn);
                
                    OnAttempt(pawn);
                    onAttempt?.Invoke();
                    
                    var diceResult = diceProcess.Result;
                    if (diceResult.Evaluate(actionStats.GetDifficulty()))
                    {
                        if (actionStats.OncePerGame()) m_donePerGame = true;
                    
                        actionStats.Reward(pawn);
                    
                        OnSuccess(pawn);
                        onSuccess?.Invoke();
                    }
                    else
                    {
                        OnFailure(pawn);
                        onFailure?.Invoke();
                    }   
                }
            }
            
            m_interacting = null;
        }
        
        protected override void OnInteract(GameObject actor, bool value)
        {
            if (!value) return;
            if (!actor.TryGetComponent(out PawnController pawn)) return;

            if (m_interacting == null)
            {
                m_interacting = StartCoroutine(Interacting(pawn));
            }
        }

        public virtual void OnDayStart(PawnController pawn)
        {
            m_donePerDay = false;
        }
        
        protected virtual void OnAttempt(PawnController pawn) { }
        protected virtual void OnSuccess(PawnController pawn) { }
        protected virtual void OnFailure(PawnController pawn) { }
    }
}