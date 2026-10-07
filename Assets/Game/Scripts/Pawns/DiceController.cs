using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        [SerializeField] private int m_onlyNextDiceBonus;
        [SerializeField] private int m_debugDiceValueResult;
        
        public float EvaluateChance(int difficulty)
        {
            var bonus = 0;
            var pawn = GetPawn();
            if (pawn) bonus += pawn.GetStats().GetComfort();
            bonus += m_onlyNextDiceBonus;

            var minValue = 2 + bonus;
            var maxValue = 19 + bonus;
            var rangeValue = maxValue - minValue;

            var chanceCrit = 1f / 20f;
            var chanceNormal = difficulty <= minValue ? 1f :
                difficulty > maxValue ? 0f :
                (((float)maxValue - difficulty) / rangeValue);

            return 18f / 20f * chanceNormal + chanceCrit;
        }
        
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

        public void AddNextDiceBonus(int amount)
        {
            m_onlyNextDiceBonus = amount;
        }
        
        public DiceResult GetRandomDiceValue()
        {
            var diceResult = new DiceResult(GetRandomValue());
            
            var pawn = GetPawn();
            if (pawn)
            {
                diceResult.AddBonus(DiceBonus.Comfort, pawn.GetStats().GetComfort());
            }

            if (m_onlyNextDiceBonus != 0)
            {
                diceResult.AddBonus(DiceBonus.NextDiceBonus, m_onlyNextDiceBonus);
                m_onlyNextDiceBonus = 0;
            }
            
            m_debugDiceValueResult = diceResult.Result;
            
            return diceResult;
        }

        private int GetRandomValue()
        {
            if (m_debugRandomize)
            {
                m_debugDiceValueClear = Random.Range(0, 20) + 1;
            }

            return m_debugDiceValueClear;
        }
    }

    public enum DiceBonus
    {
        NextDiceBonus,
        Comfort,
    }
    
    public readonly struct DiceResult
    {
        public readonly int Clear;
        public readonly Dictionary<DiceBonus, int> Bonus;

        public int Result => Clear + Bonus.Sum(b => b.Value);
        
        public DiceResult(int clear)
        {
            Clear = clear;
            Bonus = new Dictionary<DiceBonus, int>();
        }

        public void AddBonus(DiceBonus bonus, int amount)
        {
            Bonus[bonus] = amount;
        }
        
        public int RemoveBonus(DiceBonus bonus)
        {
            return Bonus.Remove(bonus, out var amount) ? amount : 0;
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

            var success = Result >= difficulty;
            
            if (Bonus.Count > 0)
            {
                var bonus = string.Join(" + ", Bonus.Where(b => b.Value != 0).Select(b => $"{b.Key}({b.Value})"));
                Debug.Log($"[{Clear} + {bonus} = {Result}] >= {difficulty} [{(success ? "Success" : "Failure")}]");
            }
            else
            {
                Debug.Log($"[{Clear}] >= {difficulty} [{(success ? "Success" : "Failure")}]");
            }
            
            return success;
        }
    } 
    
    public class DiceProcess : YieldProcess<DiceResult>
    {
        public readonly DiceController Dice;
        private readonly float debugProcessDuration;
        
        public DiceProcess(DiceController dice, float duration = -1f)
        {
            Dice = dice;
            debugProcessDuration = duration;
        }
        
        protected override IEnumerator OnProcess()
        {
            if (debugProcessDuration > 0f)
            {
                Dice.GetPawn().Mute(true);
                yield return new WaitForSeconds(debugProcessDuration);
                Dice.GetPawn().Mute(false);
            }
        }

        protected override DiceResult GetResult()
        {
            return Dice.GetRandomDiceValue();
        }
    }
}