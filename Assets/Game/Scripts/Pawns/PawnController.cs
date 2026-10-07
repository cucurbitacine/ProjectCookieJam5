using Game.Scripts.Interactions;
using UnityEngine;

namespace Game.Scripts.Pawns
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(PawnMovement))]
    [RequireComponent(typeof(PawnRotation))]
    [RequireComponent(typeof(PawnStats))]
    [RequireComponent(typeof(DiceController))]
    [RequireComponent(typeof(InteractActor))]
    public class PawnController : MonoBehaviour
    {
        private PawnMovement m_movement;
        private PawnRotation m_rotation;
        private PawnStats m_stats;
        private DiceController m_dice;
        private InteractActor m_actor;

        public PawnMovement GetMovement()
        {
            return m_movement;
        }
        
        public PawnRotation GetRotation()
        {
            return m_rotation;
        }
        
        public PawnStats GetStats()
        {
            return m_stats;
        }
        
        public DiceController GetDice()
        {
            return m_dice;
        }
        
        public InteractActor GetActor()
        {
            return m_actor;
        }
        
        public void Mute(bool mute)
        {
            GetMovement().Mute(mute);
            GetActor().Mute(mute);
            GetRotation().Mute(mute);

            enabled = !mute;
        }
        
        private void SetupPawn()
        {
            m_movement = GetComponent<PawnMovement>();
            m_rotation = GetComponent<PawnRotation>();
            m_stats = GetComponent<PawnStats>();
            m_dice = GetComponent<DiceController>();
            m_actor = GetComponent<InteractActor>();
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

        private void Start()
        {
            GetDice().ChangePawn(this);
        }

        private void LateUpdate()
        {
            UpdatePawn(Time.deltaTime);
        }
    }
}