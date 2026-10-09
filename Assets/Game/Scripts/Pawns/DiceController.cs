using System.Collections.Generic;
using System.Linq;
using Game.Scripts.Interactions.Impl;
using UnityEngine;

namespace Game.Scripts.Pawns
{
    public class DiceController : MonoBehaviour
    {
        [SerializeField] private PawnController m_pawn;
        
        [Space]
        [SerializeField] private bool m_debugRandomize = true;
        [Space]
        [Range(1, 20)] [SerializeField] private int m_debugDiceValueClear = 20;
        [SerializeField] private int m_onlyNextDiceBonus;

        [Space]
        [SerializeField] private DiceProcessController m_diceProcessPrefab;
        
        public PawnController GetPawn()
        {
            return m_pawn;
        }
        
        public void ChangePawn(PawnController pawn)
        {
            m_pawn = pawn;
        }
        
        public DiceProcess CreateDiceProcess(ActionObjectAsset action)
        {
            return Instantiate(m_diceProcessPrefab).LaunchProcess(this, action);
        }

        public void AddNextDiceBonus(int amount)
        {
            m_onlyNextDiceBonus = amount;
        }

        public Dictionary<DiceBonus, int> GetBonuses()
        {
            var bonuses = new Dictionary<DiceBonus, int>();

            var pawn = GetPawn();
            if (pawn)
            {
                var comfortBonus = pawn.GetStats().GetComfort();
                if (comfortBonus != 0)
                {
                    bonuses[DiceBonus.Comfort] = comfortBonus;
                }
            }

            if (m_onlyNextDiceBonus != 0)
            {
                bonuses[DiceBonus.NextDiceBonus] = m_onlyNextDiceBonus;
            }

            return bonuses;
        }
        
        public DiceResult GenerateDiceResult()
        {
            var diceResult = new DiceResult(GetRandomValue());

            var bonuses = GetBonuses();
            foreach (var bonus in bonuses)
            {
                diceResult.AddBonus(bonus.Key, bonus.Value);
            }

            if (bonuses.ContainsKey(DiceBonus.NextDiceBonus))
            {
                m_onlyNextDiceBonus = 0;
            }
            
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

            if (Bonus.Count(b => b.Value != 0) > 0)
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
        
        public static float EvaluateChance(int difficulty, int bonus)
        {
            /*
            var bonus = 0;
            var pawn = GetPawn();
            if (pawn) bonus += pawn.GetStats().GetComfort();
            bonus += m_onlyNextDiceBonus;
            */
            //var bonus = GetBonuses().Sum(b => b.Value);
            
            var minValue = 2 + bonus;
            var maxValue = 19 + bonus;
            var rangeValue = maxValue - minValue;

            var chanceCrit = 1f / 20f;
            var chanceNormal = difficulty <= minValue ? 1f :
                difficulty > maxValue ? 0f :
                (((float)maxValue - difficulty) / rangeValue);

            return 18f / 20f * chanceNormal + chanceCrit;
        }
    } 
}