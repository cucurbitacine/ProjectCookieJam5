using System;
using UnityEngine;

namespace Game.Scripts.Pawns
{
    [DisallowMultipleComponent]
    public class PawnStats : MonoBehaviour
    {
        [SerializeField] private PawnStatsData m_statsData;

        public Action<StatsType, int, int> StatsChanged;
        
        public PawnStatsData GetStatsData()
        {
            return m_statsData;
        }
        
        public void SetStats(PawnStatsData stats)
        {
            var prevData = m_statsData;
            m_statsData = stats;

            CheckChange(StatsType.ActionPoints, prevData.ActionPoints, stats.ActionPoints);
            CheckChange(StatsType.Will, prevData.Will, stats.Will);
            CheckChange(StatsType.Comfort, prevData.Comfort, stats.Comfort);
        }

        private void CheckChange(StatsType type, int prev, int next)
        {
            if (prev != next)
            {
                StatsChanged?.Invoke(type, prev, next);
            }
        }
    }

    [Serializable]
    public struct PawnStatsData
    {
        public int ActionPoints;
        public int Will;
        public int Comfort;
    }

    public enum StatsType
    {
        ActionPoints,
        Will,
        Comfort,
    }
    
    public static class PawnStatsExtensions
    {
        // ActionPoints
        
        public static int GetActionPoints(this PawnStats pawn)
        {
            return pawn.GetStatsData().ActionPoints;
        }
        
        public static void SetActionPoints(this PawnStats pawn, int amount)
        {
            var stats = pawn.GetStatsData();
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
            return pawn.GetStatsData().Will;
        }
        
        public static void SetWill(this PawnStats pawn, int amount)
        {
            var stats = pawn.GetStatsData();
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
            return pawn.GetStatsData().Comfort;
        }
        
        public static void SetComfort(this PawnStats pawn, int amount)
        {
            var stats = pawn.GetStatsData();
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