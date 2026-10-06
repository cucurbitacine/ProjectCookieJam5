using System.Collections;
using Game.Scripts.Levels;
using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class TeleportationObject : InteractableObject
    {
        [Header("Teleportation")]
        [SerializeField] private Transform m_destination;

        private bool m_teleporting;
        private TeleportationManager m_teleportManager;

        public Transform GetDestination()
        {
            return m_destination;
        }

        public void SetDestination(Transform destination)
        {
            m_destination = destination;
        }
        
        private IEnumerator Teleporting(PawnController pawn)
        {
            m_teleporting = true;

            var teleportation = m_teleportManager.CreateTeleportProcess(pawn, GetDestination());

            yield return StartCoroutine(teleportation.Process());
            
            m_teleporting = false;
        }

        public override bool HasAccess(GameObject actor)
        {
            return !m_teleporting;
        }

        protected override void OnInteract(GameObject actor, bool value)
        {
            if (!value) return;

            if (m_teleporting) return;
            if (GetDestination() == null) return;
            
            if (!actor.TryGetComponent(out PawnController pawn)) return;
            
            StartCoroutine(Teleporting(pawn));
        }
        
        private void Awake()
        {
            m_teleportManager = FindAnyObjectByType<TeleportationManager>();
        }
    }
}
