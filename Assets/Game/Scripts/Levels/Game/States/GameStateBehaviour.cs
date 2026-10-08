using System;

namespace Game.Scripts.Levels.Game.States
{
    [Serializable]
    public abstract class GameStateBehaviour
    {
        public GameLevelController GameLevel { get; private set; }
        
        public void Initialize(GameLevelController gameLevel)
        {
            GameLevel = gameLevel;
            
            OnInitialize(gameLevel);
        }
        
        protected abstract void OnInitialize(GameLevelController gameLevel);
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update(float deltaTime);
    }
}