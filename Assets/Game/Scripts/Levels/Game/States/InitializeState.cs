using System;

namespace Game.Scripts.Levels.Game.States
{
    [Serializable]
    public class InitializeState : GameStateController
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
}