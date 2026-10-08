using System;
using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    [CreateAssetMenu(menuName = "Create ActionObjectAsset", fileName = "ActionObjectAsset", order = 0)]
    public class ActionObjectAsset : InteractableObjectAsset
    {
        [Header("Action Object")]
        [Range(1, 20)] [SerializeField] private int m_difficulty = 1;
        
        [Header("Cost")]
        [SerializeField] private ActionCost m_actionCost = new ActionCost();
        
        [Header("Reward")]
        [SerializeField] private ActionReward m_actionReward = new ActionReward();

        [Header("Settings")]
        [SerializeField] private bool m_oncePerGame = false;
        [SerializeField] private bool m_oncePerDay = false;
        
        [Header("Results")]
        [Multiline] [SerializeField] private string m_resultSuccess;
        [Multiline] [SerializeField] private string m_resultFailure;

        public ActionCost GetCost()
        {
            return m_actionCost;
        }

        public ActionReward GetReward()
        {
            return m_actionReward;
        }
        
        public bool OncePerGame()
        {
            return m_oncePerGame;
        }
        
        public bool OncePerDay()
        {
            return m_oncePerDay;
        }
        
        public int GetDifficulty()
        {
            return m_difficulty;
        }
        
        public string GetActionResult(bool success)
        {
            return success ? m_resultSuccess : m_resultFailure;
        }
        
        public bool CanCharge(PawnController pawn)
        {
            var pawnStats = pawn.GetStats();
            var cost = GetCost();
            return pawnStats.ContainsActionPoints(cost.ActionPoints) &&
                   pawnStats.ContainsWill(cost.Will);
        }
        
        public void Charge(PawnController pawn)
        {
            var pawnStats = pawn.GetStats();
            
            var cost = GetCost();
            pawnStats.AddActionPoints(-cost.ActionPoints);
            pawnStats.AddWill(-cost.Will);
        }
        
        public void Reward(PawnController pawn)
        {
            var pawnStats = pawn.GetStats();
            
            var reward = GetReward();
            pawnStats.AddWill(reward.Will);
            pawnStats.AddComfort(reward.Comfort);

            if (reward.NextDice != 0)
            {
                pawn.GetDice().AddNextDiceBonus(reward.NextDice);
            }
        }
    }

    [Serializable]
    public struct ActionCost
    {
        [Min(0)] public int ActionPoints;
        [Min(0)] public int Will;
    }
    
    [Serializable]
    public struct ActionReward
    {
        [Min(0)] public int Will;
        [Min(0)] public int Comfort;
        public int NextDice;
    }
}