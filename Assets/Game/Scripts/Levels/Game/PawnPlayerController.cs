using System.Collections;
using CucuTools.LevelSystem;
using Game.Scripts.Pawns;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Levels.Game
{
    public class PawnPlayerController : LevelComponent
    {
        [SerializeField] private PawnController m_pawn;
        [SerializeField] private InputActionReference m_moveAction;
        
        public PawnController GetPawn()
        {
            return m_pawn;
        }
        
        private void OnMove(InputAction.CallbackContext context)
        {
            var move = context.ReadValue<Vector2>();
            var pawn = GetPawn();
            var movement = pawn.GetMovement();
            movement.Move(move.x);
        }
        
        public override void ResolveAll(LevelContext context)
        {
        }

        public override IEnumerator OnStartLevel()
        {
            m_moveAction.action.performed += OnMove;
            m_moveAction.action.canceled += OnMove;
            
            yield break;
        }

        public override void OnDestroyLevel()
        {
            m_moveAction.action.performed -= OnMove;
            m_moveAction.action.canceled -= OnMove;
        }
    }
}