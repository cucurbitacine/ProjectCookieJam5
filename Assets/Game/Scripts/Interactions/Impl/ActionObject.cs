using System.Collections;
using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class ActionObject : InteractableObject
    {
        [Header("Action")]
        [SerializeField] private ActionStats m_actionStats;
        
        private Coroutine m_interacting;
        
        public ActionStats GetActionStats()
        {
            return m_actionStats;
        }

        private IEnumerator Interacting(PawnController pawn)
        {
            var actionStats = GetActionStats();
            var pawnStats = pawn.GetStats();

            if (actionStats.CanCharge(pawnStats))
            {
                actionStats.Charge(pawnStats);
                
                OnAttempt(pawn);

                var diceProcess = pawn.GetDice().CreateDiceProcess();
                yield return diceProcess.Await();
                var diceResult = diceProcess.result;
                
                if (diceResult.Evaluate(actionStats.GetDifficulty()))
                {
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

        protected virtual void OnAttempt(PawnController pawn)
        {
        }
        
        protected virtual void OnSuccess(PawnController pawn)
        {
        }
        
        protected virtual void OnFailure(PawnController pawn)
        {
        }
    }
}