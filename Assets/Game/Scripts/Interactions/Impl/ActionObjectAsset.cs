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
        [Min(0)] [SerializeField] private int m_actionPointCost = 1;
        [Min(0)] [SerializeField] private int m_willCost = 1;
        
        [Header("Reward")]
        [SerializeField] private int m_willReward = 0;
        [SerializeField] private int m_comfortReward = 0;

        [Header("Settings")]
        [SerializeField] private bool m_oncePerGame = false;
        [SerializeField] private bool m_oncePerDay = false;
        
        public ActionCost GetCost()
        {
            return new ActionCost()
            {
                ActionPoints = m_actionPointCost,
                Will = m_willCost,
            };
        }

        public ActionReward GetReward()
        {
            return new ActionReward()
            {
                Will = m_willReward,
                Comfort = m_comfortReward,
            };
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
        
        public bool CanCharge(PawnStats pawnStats)
        {
            return pawnStats.ContainsActionPoints(m_actionPointCost) &&
                   pawnStats.ContainsWill(m_willCost);
        }
        
        public void Charge(PawnStats pawnStats)
        {
            pawnStats.AddActionPoints(-m_actionPointCost);
            pawnStats.AddWill(-m_willCost);
        }
        
        public void Reward(PawnStats pawnStats)
        {
            pawnStats.AddWill(m_willReward);
            pawnStats.AddComfort(m_comfortReward);
        }
    }

    public struct ActionCost
    {
        public int ActionPoints;
        public int Will;
    }
    
    public struct ActionReward
    {
        public int Will;
        public int Comfort;
    }
}