using System;
using Game.Scripts.Interactions;
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
        [SerializeField] private InteractableObject m_bedObject;
        
        [Space]
        [SerializeField] private bool m_debugMode;
        [SerializeField] private PawnStatsData m_debugStatsOnStartDay;
        
        private PawnPlayerController m_player;
        private ActionObject[] m_actionObjects;

        private PawnStatsData GetStatsOnStartDay()
        {
            if (m_debugMode) return m_debugStatsOnStartDay;
            return m_statsOnStartDay;
        }
        
        private void OnBedInteracted(GameObject actor, InteractState state, bool value)
        {
            if (state is not InteractState.Interact) return;
            if (!value) return;
            
            GameLevel.ChangeGameState(GameState.EndDay);
        }
        
        public override void Enter()
        {
            GameLevel.IncreaseDayCounter();

            var playerPawn = m_player.GetPawn();
            playerPawn.Mute(false);
            
            var playerPawnStats = playerPawn.GetStats();

            var prevActionPoints = playerPawnStats.GetStatsData().ActionPoints;
            var prevWill = playerPawnStats.GetStatsData().Will;
            var prevComfort = playerPawnStats.GetStatsData().Will;
            
            playerPawnStats.SetStats(GetStatsOnStartDay());

            if (m_keepActionPoints) playerPawnStats.AddActionPoints(prevActionPoints);
            if (m_keepWill) playerPawnStats.AddWill(prevWill);
            if (m_keepComfort) playerPawnStats.AddComfort(prevComfort);
            
            foreach (var actionObject in m_actionObjects)
            {
                actionObject.OnDayStart(m_player.GetPawn());
            }

            m_bedObject.InteractStateChanged += OnBedInteracted;
        }

        public override void Exit()
        {
            m_bedObject.InteractStateChanged -= OnBedInteracted;
            
            var playerPawn = m_player.GetPawn();
            playerPawn.Mute(true);
        }
        
        public override void Update(float deltaTime)
        {
        }
        
        protected override void OnInitialize(GameLevelController gameLevel)
        {
            m_player = gameLevel.GetPlayer();

            m_actionObjects = Object.FindObjectsByType<ActionObject>();
        }
    }
}