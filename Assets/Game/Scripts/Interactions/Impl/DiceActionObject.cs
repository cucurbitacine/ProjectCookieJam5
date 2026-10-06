using System.Collections;
using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public abstract class DiceActionObject : InteractableObject
    {
        [Header("Action")]
        [SerializeField] private ActionStats m_actionStats;
        [SerializeField] private bool m_donePerGame;
        [SerializeField] private bool m_donePerDay;
        
        private Coroutine m_interacting;
        
        public ActionStats GetActionStats()
        {
            return m_actionStats;
        }

        public bool HasDoneFlag()
        {
            return m_donePerGame || m_donePerDay;
        }

        public override bool HasAccess(GameObject actor)
        {
            return !HasDoneFlag() && base.HasAccess(actor);
        }

        private IEnumerator Interacting(PawnController pawn)
        {
            var actionStats = GetActionStats();
            var pawnStats = pawn.GetStats();
            
            if (!HasDoneFlag() && actionStats.CanCharge(pawnStats))
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