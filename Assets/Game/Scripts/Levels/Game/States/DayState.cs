using System;
using Game.Scripts.Interactions.Impl;
using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Levels.Game.States
{
    [Serializable]
    public class DayState : GameStateBehaviour
    {
        [SerializeField] private PawnStatsData m_statsOnStartDay;
        [SerializeField] private SinkObject m_sink;
        
        private PawnPlayerController m_player;
        
        public override void Enter()
        {
            var playerPawnStats = m_player.GetPawn().GetStats();
            
            playerPawnStats.SetStats(m_statsOnStartDay);
            
            if (m_sink.HasDishFlag())
            {
                playerPawnStats.AddComfort(-m_sink.GetDebuffComfortOnStartDay());
            }
        }

        public override void Exit()
        {
        }
        
        public override void Update(float deltaTime)
        {
        }
        
        protected override void OnInitialize(GameLevelController gameLevel)
        {
            m_player = gameLevel.GetPlayer();
        }
    }
}