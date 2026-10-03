using UnityEngine;

namespace Game.Scripts.Pawns
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PawnMovement))]
    [RequireComponent(typeof(PawnRotation))]
    public class PawnController : MonoBehaviour
    {
        private PawnMovement m_movement;
        private PawnRotation m_rotation;

        public PawnMovement GetMovement()
        {
            return m_movement;
        }
        
        public PawnRotation GetRotation()
        {
            return m_rotation;
        }
        
        private void SetupPawn()
        {
            m_movement = GetComponent<PawnMovement>();
            m_rotation = GetComponent<PawnRotation>();
        }

        private void UpdatePawn(float deltaTime)
        {
            var rotation = GetRotation();
            var movement = GetMovement();
            var move = movement.GetMove();
            rotation.ChangeDirection(move);
        }
        
        private void Awake()
        {
            SetupPawn();
        }

        private void LateUpdate()
        {
            UpdatePawn(Time.deltaTime);
        }
    }
}