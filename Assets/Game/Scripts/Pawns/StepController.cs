using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts.Pawns
{
    public class StepController : MonoBehaviour
    {
        [SerializeField] private PawnController pawn;
        [Space]
        [SerializeField] private float stepDelay = 0.5f;
        [Space]
        [SerializeField] private UnityEvent onStep = new UnityEvent();

        private float stepTimer;
        private bool wasMoving;
        private bool started;
        private bool canceled;
        
        private void UpdateStep(bool isMoving, float deltaTime)
        {
            started = !wasMoving && isMoving;
            canceled = wasMoving && !isMoving;

            wasMoving = isMoving;

            if (started)
            {
                stepTimer = 0f;
                onStep?.Invoke();
            }
            else if (isMoving)
            {
                if (stepTimer >= stepDelay)
                {
                    stepTimer = 0f;
                    onStep?.Invoke();
                }
                else
                {
                    stepTimer += deltaTime;
                }
            }
            else if (canceled)
            {
                stepTimer = 0f;
            }
        }
        
        private void Update()
        {
            if (pawn)
            {
                UpdateStep(pawn.IsMoving(), Time.deltaTime);
            }
        }
    }

    public static partial class Extensions
    {
        public static bool IsMoving(this PawnMovement movement)
        {
            return movement.enabled && !Mathf.Approximately(movement.GetMove(), 0f);
        }
        
        public static bool IsMoving(this PawnController pawn)
        {
            return pawn.enabled && pawn.GetMovement().IsMoving();
        }
    }
}