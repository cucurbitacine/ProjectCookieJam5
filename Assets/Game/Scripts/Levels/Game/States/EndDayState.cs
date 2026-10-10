using System;
using UnityEngine;
using UnityEngine.Events;

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
            if (GameLevel.GetCurrentDay() >= m_lastDay || GameLevel.GetTaskList().IsAllDone())
            {
                GameLevel.ChangeGameState(GameState.EndGame);
            }
            else
            {
                GameLevel.ChangeGameState(GameState.Day);
            }
        }
    }
    
    [Serializable]
    public class EndGameState : GameStateBehaviour
    {
        [SerializeField] private UnityEvent m_onWin = new UnityEvent();
        [SerializeField] private UnityEvent m_onLoose = new UnityEvent();
        
        public bool IsWin()
        {
            return GameLevel.GetTaskList().IsAllDone();
        }
        
        protected override void OnInitialize(GameLevelController gameLevel)
        {
        }
        
        public override void Enter()
        {
            if (IsWin())
            {
                m_onWin.Invoke();
            }
            else
            {
                m_onLoose.Invoke();
            }
        }

        public override void Exit()
        {
        }

        public override void Update(float deltaTime)
        {
        }
    }
}