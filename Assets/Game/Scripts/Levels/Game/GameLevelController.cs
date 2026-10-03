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

        [Header("States")]
        [SerializeField] private GameState m_gameState;
        [SerializeField] private InitializeState m_initializeState = new InitializeState();

        [Header("Settings")]
        [SerializeField] private LevelSceneAsset menuScene;
        
        [Header("Debug")]
        [SerializeField] private bool m_debugLog;

        #endregion

        #region Private members

        private GameState m_previousGameState;
        private readonly Dictionary<GameState, GameStateController> m_states = new Dictionary<GameState, GameStateController>();

        #endregion

        #region Public API

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
            
            OnGameStateChanged(m_previousGameState, m_gameState);
            
            GameStateChanged?.Invoke(m_previousGameState, m_gameState);
        }

        [ContextMenu(nameof(ReturnMenu))]
        public void ReturnMenu()
        {
            LevelManager.Instance.LoadSceneAsync(menuScene);
        }
        
        #endregion

        #region Private API

        private void AddState(GameState gameState, GameStateController state)
        {
            m_states[gameState] = state;
            state.Initialize(this);
        }
        
        private bool TryGetState(GameState gameState, out GameStateController state)
        {
            return m_states.TryGetValue(gameState, out state);
        }
        
        private bool TryGetActualState(out GameStateController actualState)
        {
            return TryGetState(GetGameState(), out actualState);
        }
        
        private void OnGameStateChanged(GameState previous, GameState next)
        {
            if (m_debugLog) Debug.Log($"[GameLevel] :: [{previous}] >>> [{next}]");
            
            if (TryGetState(previous, out var exitState))
            {
                exitState.Exit();
            }
            
            if (TryGetState(next, out var enterState))
            {
                enterState.Enter();
            }
        }

        #endregion

        #region LevelController

        public override void BindAll(LevelContext context)
        {
            context.Bind<GameLevelController>(this);

            AddState(GameState.Initialize, m_initializeState);
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