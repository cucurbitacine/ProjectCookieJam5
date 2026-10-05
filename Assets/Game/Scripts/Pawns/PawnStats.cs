using System;
using UnityEngine;

namespace Game.Scripts.Pawns
{
    [DisallowMultipleComponent]
    public class PawnStats : MonoBehaviour
    {
        [SerializeField] private PawnStatsData m_statsData;

        public PawnStatsData GetStats()
        {
            return m_statsData;
        }
        
        public void SetStats(PawnStatsData stats)
        {
            m_statsData = stats;
        }
    }

    [Serializable]
    public struct PawnStatsData
    {
        public int ActionPoints;
        public int Will;
        public int Comfort;
    }
    
    public static class PawnStatsExtensions
    {
        // ActionPoints
        
        public static int GetActionPoints(this PawnStats pawn)
        {
            return pawn.GetStats().ActionPoints;
        }
        
        public static void SetActionPoints(this PawnStats pawn, int amount)
        {
            var stats = pawn.GetStats();
            stats.ActionPoints = amount;
            pawn.SetStats(stats);
        }
        
        public static void AddActionPoints(this PawnStats pawn, int amount)
        {
            pawn.SetActionPoints(pawn.GetActionPoints() + amount);
        }
        
        public static bool ContainsActionPoints(this PawnStats pawn, int amount)
        {
            return pawn.GetActionPoints() >= amount;
        }
        
        // Will
        
        public static int GetWill(this PawnStats pawn)
        {
            return pawn.GetStats().Will;
        }
        
        public static void SetWill(this PawnStats pawn, int amount)
        {
            var stats = pawn.GetStats();
            stats.Will = amount;
            pawn.SetStats(stats);
        }
        
        public static void AddWill(this PawnStats pawn, int amount)
        {
            pawn.SetWill(pawn.GetWill() + amount);
        }
        
        public static bool ContainsWill(this PawnStats pawn, int amount)
        {
            return pawn.GetWill() >= amount;
        }
        
        // Comfort
        
        public static int GetComfort(this PawnStats pawn)
        {
            return pawn.GetStats().Comfort;
        }
        
        public static void SetComfort(this PawnStats pawn, int amount)
        {
            var stats = pawn.GetStats();
            stats.Comfort = amount;
            pawn.SetStats(stats);
        }

        public static void AddComfort(this PawnStats pawn, int amount)
        {
            pawn.SetComfort(pawn.GetComfort() + amount);
        }
        
        public static bool ContainsComfort(this PawnStats pawn, int amount)
        {
            return pawn.GetComfort() >= amount;
        }
    }
}