using System;
using UnityEngine;

namespace Game.Scripts.Levels.Game.States
{
    [Serializable]
    public class EndDayState : GameStateBehaviour
    {
        [SerializeField] private int m_lastDay = 7;
        
        protected override void OnInitialize(GameLevelController gameLevel)
        {
        }
        
        public override void Enter()
        {
        }

        public override void Exit()
        {
        }

        public override void Update(float deltaTime)
        {
            if (GameLevel.GetCurrentDay() >= m_lastDay)
            {
                GameLevel.ChangeGameState(GameState.EndGame);
            }
            else
            {
                GameLevel.ChangeGameState(GameState.Day);
            }
        }
    }
}