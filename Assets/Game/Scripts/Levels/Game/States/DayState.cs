using System;
using Game.Scripts.Interactions.Impl;
using Game.Scripts.Pawns;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Game.Scripts.Levels.Game.States
{
    [Serializable]
    public class DayState : GameStateBehaviour
    {
        [SerializeField] private bool m_keepActionPoints;
        [SerializeField] private bool m_keepWill;
        [SerializeField] private bool m_keepComfort;
        
        [Space]
        [SerializeField] private PawnStatsData m_statsOnStartDay;
        
        [Space]
        [SerializeField] private bool m_debugMode;
        [SerializeField] private PawnStatsData m_debugStatsOnStartDay;
        
        private PawnPlayerController m_player;
        private BedObject m_bedObject;
        private ActionObject[] m_actionObjects;

        private PawnStatsData GetStatsOnStartDay()
        {
            if (m_debugMode) return m_debugStatsOnStartDay;
            return m_statsOnStartDay;
        }
        
        private void OnSleepConfirmed(bool confirm)
        {
            if (!confirm) return;
            
            GameLevel.ChangeGameState(GameState.EndDay);
        }
        
        public override void Enter()
        {
            GameLevel.IncreaseDayCounter();

            var playerPawn = m_player.GetPawn();
            playerPawn.Mute(false);
            
            var playerPawnStats = playerPawn.GetStats();
            var playerPawnStatsData = playerPawnStats.GetStatsData();
            
            var prevActionPoints = playerPawnStatsData.ActionPoints;
            var prevWill = playerPawnStatsData.Will;
            var prevComfort = playerPawnStatsData.Comfort;

            var statsOnStartDay = GetStatsOnStartDay();

            if (m_keepActionPoints) statsOnStartDay.ActionPoints += prevActionPoints;
            if (m_keepWill) statsOnStartDay.Will += prevWill;
            if (m_keepComfort) statsOnStartDay.Comfort += prevComfort;
            
            playerPawnStats.SetStats(statsOnStartDay);
            
            foreach (var actionObject in m_actionObjects)
            {
                actionObject.OnDayStart(m_player.GetPawn());
            }

            m_bedObject.SleepConfirmation.Confirmed += OnSleepConfirmed;
        }

        public override void Exit()
        {
            m_bedObject.SleepConfirmation.Confirmed -= OnSleepConfirmed;
            
            var playerPawn = m_player.GetPawn();
            playerPawn.Mute(true);
        }
        
        public override void Update(float deltaTime)
        {
        }
        
        protected override void OnInitialize(GameLevelController gameLevel)
        {
            m_player = gameLevel.GetPlayer();

            m_bedObject = Object.FindAnyObjectByType<BedObject>();
            m_actionObjects = Object.FindObjectsByType<ActionObject>();
        }
    }
}