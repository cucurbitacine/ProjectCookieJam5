using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    [CreateAssetMenu(menuName = "Create ActionStats", fileName = "ActionStats", order = 0)]
    public class ActionStats : ScriptableObject
    {
        [Header("Settings")]
        [SerializeField] private bool m_oncePerGame = false;
        [SerializeField] private bool m_oncePerDay = false;
        [Range(1, 20)] [SerializeField] private int m_difficulty = 1;
        
        [Header("Cost")]
        [Min(0)] [SerializeField] private int m_actionPointCost = 1;
        [Min(0)] [SerializeField] private int m_willCost = 1;
        
        [Header("Reward")]
        [SerializeField] private int m_willReward = 0;
        [SerializeField] private int m_comfortReward = 0;
        
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
}