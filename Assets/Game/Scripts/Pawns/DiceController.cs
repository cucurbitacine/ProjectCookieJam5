using System.Collections;
using UnityEngine;

namespace Game.Scripts.Pawns
{
    public class DiceController : MonoBehaviour
    {
        [SerializeField] private PawnController m_pawn;
        
        [Space]
        [SerializeField] private bool m_debugRandomize = true;
        [SerializeField] private float m_debugProcessDuration = 1f;
        [Space]
        [Range(1, 20)] [SerializeField] private int m_debugDiceValueClear = 20;
        [SerializeField] private int m_debugDiceValueResult;
        
        public PawnController GetPawn()
        {
            return m_pawn;
        }
        
        public void ChangePawn(PawnController pawn)
        {
            m_pawn = pawn;
        }
        
        public DiceProcess CreateDiceProcess()
        {
            return new DiceProcess(this, m_debugProcessDuration);
        }

        public DiceResult GetRandomDiceValue()
        {
            if (m_debugRandomize)
            {
                m_debugDiceValueClear = Random.Range(0, 20) + 1;
            }

            m_debugDiceValueResult = m_debugDiceValueClear;

            var pawn = GetPawn();
            if (pawn)
            {
                m_debugDiceValueResult += pawn.GetStats().GetComfort();
            }
            
            return new DiceResult(m_debugDiceValueResult, m_debugDiceValueClear);
        }
    }

    public readonly struct DiceResult
    {
        public readonly int Result;
        public readonly int Clear;

        public DiceResult(int result, int clear)
        {
            Result = result;
            Clear = clear;
        }
        
        public DiceResult(int result) : this(result, result)
        {
        }

        public bool Evaluate(int difficulty)
        {
            switch (Clear)
            {
                case 1:
                    Debug.Log("CRITICAL 1");
                    return false;
                case 20:
                    Debug.Log("CRITICAL 20");
                    return true;
            }
            
            Debug.Log($"{Result} beats {difficulty} :: {Result >= difficulty}");
            
            return Result >= difficulty;
        }
    } 
    
    public class DiceProcess : YieldProcess<DiceResult>
    {
        private readonly DiceController dice;
        private readonly float debugProcessDuration;
        
        public DiceProcess(DiceController dice, float duration = -1f)
        {
            this.dice = dice;
            debugProcessDuration = duration;
        }
        
        protected override IEnumerator OnProcess()
        {
            if (debugProcessDuration > 0f)
            {
                yield return new WaitForSeconds(debugProcessDuration);
            }
        }

        protected override DiceResult GetResult()
        {
            return dice.GetRandomDiceValue();
        }
    }
}