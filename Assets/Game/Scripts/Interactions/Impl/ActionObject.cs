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

        public bool HasDoneFlag()
        {
            return m_donePerGame || m_donePerDay;
        }

        public override bool HasAccess(GameObject actor)
        {
            if (HasDoneFlag()) return false;

            if (actor.TryGetComponent(out PawnController pawn))
            {
                var pawnStats = pawn.GetStats();
                var actionStats = GetActionPreset();
                if (!actionStats.CanCharge(pawnStats))
                {
                    return false;
                }
            }
            
            return base.HasAccess(actor);
        }

        private IEnumerator Interacting(PawnController pawn)
        {
            var actionStats = GetActionPreset();
            var pawnStats = pawn.GetStats();
            
            if (actionStats && !HasDoneFlag() && actionStats.CanCharge(pawnStats))
            {
                if (actionStats.OncePerDay()) m_donePerDay = true;
                
                actionStats.Charge(pawnStats);
                
                OnAttempt(pawn);

                var diceProcess = pawn.GetDice().CreateDiceProcess();
                yield return StartCoroutine(diceProcess.Process());
                var diceResult = diceProcess.Result;
                
                if (diceResult.Evaluate(actionStats.GetDifficulty()))
                {
                    if (actionStats.OncePerGame()) m_donePerGame = true;
                    
                    actionStats.Reward(pawnStats);
                    
                    OnSuccess(pawn);
                }
                else
                {
                    OnFailure(pawn);
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