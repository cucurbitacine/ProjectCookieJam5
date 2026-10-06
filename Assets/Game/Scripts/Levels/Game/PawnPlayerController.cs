using System;
using System.Collections;
using CucuTools.LevelSystem;
using Game.Scripts.Interactions;
using Game.Scripts.Pawns;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Levels.Game
{
    public class PawnPlayerController : LevelComponent
    {
        [Header("Movement")]
        [SerializeField] private PawnController m_pawn;
        [SerializeField] private InputActionReference m_moveAction;
        
        [Header("Interact")]
        [SerializeField] private Vector3 m_actorOffset;
        [SerializeField] private InputActionReference m_interactAction;
        
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

        private void OnInteract(InputAction.CallbackContext context)
        {
            var interact = context.ReadValueAsButton();
            GetPawn().GetActor().Interact(interact);
        }
        
        private Vector3 GetInteractPoint()
        {
            var pawn = GetPawn();
            var rotation = pawn.GetRotation();
            var direction = rotation.GetDirection();
            var pawnPosition = pawn.transform.position;
            var point = new Vector2(pawnPosition.x + direction * m_actorOffset.x, pawnPosition.y + m_actorOffset.y);
            return point;
        }
        
        private void UpdateInteraction()
        {
            var interactPoint = GetInteractPoint();
            GetPawn().GetActor().SetInteractPoint(interactPoint);
        }
        
        public override void ResolveAll(LevelContext context)
        {
        }

        public override IEnumerator OnStartLevel()
        {
            m_moveAction.action.performed += OnMove;
            m_moveAction.action.canceled += OnMove;

            m_interactAction.action.performed += OnInteract;
            m_interactAction.action.canceled += OnInteract;
            
            yield break;
        }

        public override void OnDestroyLevel()
        {
            m_moveAction.action.performed -= OnMove;
            m_moveAction.action.canceled -= OnMove;
            
            m_interactAction.action.performed -= OnInteract;
            m_interactAction.action.canceled -= OnInteract;
        }

        private void Update()
        {
            UpdateInteraction();
        }
    }
}