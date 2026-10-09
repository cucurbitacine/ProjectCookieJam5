using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class BedObject : InteractableObject
    {
        [Header("Bed")]
        [SerializeField] private ConfirmationProcessController m_confirmation;

        public ConfirmationProcessController SleepConfirmation => m_confirmation;
        
        protected override void OnInteract(GameObject actor, bool value)
        {
            if (!value) return;
            if (!actor.TryGetComponent(out PawnController pawn)) return;

            m_confirmation.Show(pawn);
        }
    }
}