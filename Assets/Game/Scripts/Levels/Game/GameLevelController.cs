using System;
using System.Collections;
using System.Collections.Generic;
using CucuTools.LevelSystem;
using Game.Scripts.Levels.Game.States;
using UnityEngine;

namespace Game.Scripts.Levels.Game
{
    public class GameLevelController : LevelController
    {
        #region SerializeField

        [SerializeField] private int m_currentDay = 0;
        
        [Header("States")]
        [SerializeField] private GameState m_gameState;
        [SerializeField] private InitializeState m_initializeState = new InitializeState();
        [SerializeField] private StartState m_startState = new StartState();
        [SerializeField] private DayState m_dayState = new DayState();
        [SerializeField] private EndDayState m_endDayState = new EndDayState();
        
        [Header("Settings")]
        [SerializeField] private LevelSceneAsset menuScene;
        
        [Header("Debug")]
        [SerializeField] private bool m_debugLog;

        #endregion

        #region Private members

        private PawnPlayerController m_pawnPlayer;
        private GameState m_previousGameState;
        private readonly Dictionary<GameState, GameStateBehaviour> m_states = new Dictionary<GameState, GameStateBehaviour>();

        #endregion

        #region Public API

        public event Action<int, int> DayChanged;
        public event Action<GameState, GameState> GameStateChanged;
        
        public GameState GetGameState()
        {
            return m_gameState;
        }

        public GameState GetPreviousGameState()
        {
            return m_previousGameState;
        }
        
        public void ChangeGameState(GameState nextGameState)
        {
            if (m_gameState == nextGameState) return;
            m_previousGameState = m_gameState;
            m_gameState = nextGameState;
            
            GameStateChanged?.Invoke(m_previousGameState, m_gameState);
            
            OnGameStateChanged(m_previousGameState, m_gameState);
        }

        [ContextMenu(nameof(ReturnMenu))]
        public void ReturnMenu()
        {
            LevelManager.Instance.LoadSceneAsync(menuScene);
        }
        
        public PawnPlayerController GetPlayer()
        {
            return m_pawnPlayer;
        }
        
        public void IncreaseDayCounter()
        {
            var prevDay = m_currentDay;
            m_currentDay++;
            
            DayChanged?.Invoke(prevDay, m_currentDay);
        }
        
        public int GetCurrentDay()
        {
            return m_currentDay;
        }
        
        #endregion

        #region Private API

        private void AddState(GameState gameState, GameStateBehaviour state)
        {
            m_states[gameState] = state;
            state.Initialize(this);
        }
        
        private bool TryGetState(GameState gameState, out GameStateBehaviour state)
        {
            return m_states.TryGetValue(gameState, out state);
        }
        
        private bool TryGetActualState(out GameStateBehaviour actualState)
        {
            return TryGetState(GetGameState(), out actualState);
        }
        
        private void OnGameStateChanged(GameState previous, GameState next)
        {
            if (m_debugLog) Debug.Log($"[GameLevel] :: [{previous}] >>> [{next}]");
            
            if (TryGetState(previous, out var exitState))
            {
                if (m_debugLog) Debug.Log($"[GameLevel] :: [Exit] [{exitState.GetType().Name}]");
                
                exitState.Exit();
            }
            
            if (TryGetState(next, out var enterState))
            {
                if (m_debugLog) Debug.Log($"[GameLevel] :: [Enter] [{enterState.GetType().Name}]");
                
                enterState.Enter();
            }
        }

        #endregion

        #region LevelController

        protected override void OnAwake()
        {
            m_pawnPlayer = FindAnyObjectByType<PawnPlayerController>();
        }

        public override void BindAll(LevelContext context)
        {
            context.Bind<GameLevelController>(this);

            AddState(GameState.Initialize, m_initializeState);
            AddState(GameState.StartGame, m_startState);
            AddState(GameState.Day, m_dayState);
            AddState(GameState.EndDay, m_endDayState);
        }

        protected override IEnumerator StartLevel()
        {
            ChangeGameState(GameState.Initialize);

            yield break;
        }

        protected override void DestroyLevel()
        {
        }

        #endregion

        private void Update()
        {
            if (TryGetActualState(out var actualState))
            {
                actualState.Update(Time.deltaTime);
            }
        }
    }
}