using System;

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
        public override void Enter()
        {
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