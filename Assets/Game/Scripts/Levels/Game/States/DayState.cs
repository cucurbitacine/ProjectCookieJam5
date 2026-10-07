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
        [SerializeField] private PawnStatsData m_statsOnStartDay;
        
        private PawnPlayerController m_player;
        private ActionObject[] m_actionObjects;
        
        public override void Enter()
        {
            var playerPawnStats = m_player.GetPawn().GetStats();
            
            playerPawnStats.SetStats(m_statsOnStartDay);

            foreach (var actionObject in m_actionObjects)
            {
                actionObject.OnDayStart(m_player.GetPawn());
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

            m_actionObjects = Object.FindObjectsByType<ActionObject>();
        }
    }
}