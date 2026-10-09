using System;
using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Levels.Game.States
{
    [Serializable]
    public class InitializeState : GameStateBehaviour
    {
        public override void Enter()
        {
            GameLevel.ChangeGameState(GameState.StartGame);
        }

        public override void Exit()
        {
        }
        
        public override void Update(float deltaTime)
        {
        }
        
        protected override void OnInitialize(GameLevelController gameLevel)
        {
        }
    }
    
    [Serializable]
    public class StartState : GameStateBehaviour
    {
        [SerializeField] private PawnStatsData m_statsOnStartGame;
        
        public override void Enter()
        {
            GameLevel.GetPlayer().GetPawn().GetStats().SetStats(m_statsOnStartGame);
            
            GameLevel.ChangeGameState(GameState.Day);
        }

        public override void Exit()
        {
        }
        
        public override void Update(float deltaTime)
        {
        }
        
        protected override void OnInitialize(GameLevelController gameLevel)
        {
        }
    }
}