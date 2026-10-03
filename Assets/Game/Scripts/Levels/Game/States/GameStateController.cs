namespace Game.Scripts.Levels.Game.States
{
    public abstract class GameStateController
    {
        public GameLevelController GameLevel { get; private set; }
        
        public void Initialize(GameLevelController gameLevel)
        {
            GameLevel = gameLevel;
            
            OnInitialize(gameLevel);
        }
        
        public abstract void Enter();
        public abstract void Exit();
        public abstract void Update(float deltaTime);
        protected abstract void OnInitialize(GameLevelController gameLevel);
    }
}