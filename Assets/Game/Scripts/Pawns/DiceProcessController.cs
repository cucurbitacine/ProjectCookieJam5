using System;
using System.Collections;
using Game.Scripts.Interactions.Impl;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Game.Scripts.Pawns
{
    public class DiceProcessController : MonoBehaviour
    {
        [SerializeField] private DiceProcessState m_processState;
        [SerializeField] private bool m_done;
        [SerializeField] private bool m_canceled;

        [Space]
        [SerializeField] private float m_rollingDuration = 2f;
        [SerializeField] private float m_bonusDuration = 2f;
        [SerializeField] private float m_resultDuration = 60f;
        
        [Space]
        [SerializeField] private InputActionReference m_confirmAction;
        [SerializeField] private InputActionReference m_cancelAction;
        
        private DiceController m_dice;
        private ActionObjectAsset m_action;
        
        private float m_timerRolling = 0f;
        private float m_timerBonus = 0f;
        private float m_timerResult = 0f;
        private DiceResult m_diceResult;

        public Action<DiceProcessState, DiceProcessState> ProcessStateChanged;
        
        public DiceProcessState GetProcessState()
        {
            return m_processState;
        }
        
        public void ChangeProcessState(DiceProcessState nextState)
        {
            if (m_processState == nextState) return;
            var previousState = m_processState;
            m_processState = nextState;
            
            ProcessStateChanged?.Invoke(previousState, nextState);

            OnProcessChanged(previousState, nextState);
        }

        public bool IsDone()
        {
            return m_done;
        }
        
        public bool IsCanceled()
        {
            return m_canceled;
        }
        
        public bool InProgress()
        {
            return !IsDone();
        }

        public DiceController GetDice()
        {
            return m_dice;
        }
        
        public ActionObjectAsset GetAction()
        {
            return m_action;
        }
        
        public DiceResult GetDiceResult()
        {
            return m_diceResult;
        }
        
        public DiceProcess LaunchProcess(DiceController dice, ActionObjectAsset action)
        {
            m_dice = dice;
            m_action = action;

            return new DiceProcess(this);
        }

        public void Roll()
        {
            if (GetProcessState() is DiceProcessState.Ready)
            {
                ChangeProcessState(DiceProcessState.Rolling);
            }
        }

        public void Close()
        {
            if (GetProcessState() is DiceProcessState.Ready)
            {
                SetCancel();
                
                ChangeProcessState(DiceProcessState.Exit);
            }
            else if (GetProcessState() is DiceProcessState.Result)
            {
                ChangeProcessState(DiceProcessState.Exit);
            }
        }
        
        public void Bonuses()
        {
            m_diceResult = GetDice().GenerateDiceResult();
            
            ChangeProcessState(DiceProcessState.Bonuses);
        }

        public void Result()
        {
            ChangeProcessState(DiceProcessState.Result);
        }

        private void SetDone()
        {
            m_done = true;
        }
        
        private void SetCancel()
        {
            m_canceled = true;
        }
        
        private void OnProcessChanged(DiceProcessState exit, DiceProcessState enter)
        {
            if (enter is DiceProcessState.Ready)
            {
                GetDice().GetPawn().Mute(true);
            }
            else if (enter is DiceProcessState.Exit)
            {
                GetDice().GetPawn().Mute(false);

                SetDone();
                
                Destroy(gameObject);
            }
        }
        
        private void UpdateProcessState(DiceProcessState state, float deltaTime)
        {
            if (state is DiceProcessState.Rolling)
            {
                if (m_timerRolling >= m_rollingDuration)
                {
                    Bonuses();
                }
                else
                {
                    m_timerRolling += deltaTime;
                }
            }
            else if (state is DiceProcessState.Bonuses)
            {
                if (m_timerBonus >= m_bonusDuration)
                {
                    Result();
                }
                else
                {
                    m_timerBonus += deltaTime;
                }
            }
            else if (state is DiceProcessState.Result)
            {
                if (m_timerResult >= m_resultDuration)
                {
                    Close();
                }
                else
                {
                    m_timerResult += deltaTime;
                }
            }
        }
        
        private void OnConfirmInput(InputAction.CallbackContext context)
        {
            if (!context.ReadValueAsButton()) return;
            
            if (GetProcessState() is DiceProcessState.Ready)
            {
                Roll();
            }
            else if (GetProcessState() is DiceProcessState.Rolling)
            {
                Bonuses();
                //Result();
            }
            else if (GetProcessState() is DiceProcessState.Bonuses)
            {
                Result();
            }
            else if (GetProcessState() is DiceProcessState.Result)
            {
                Close();
            }
        }

        private void OnCancelInput(InputAction.CallbackContext context)
        {
            if (!context.ReadValueAsButton()) return;
            
            if (GetProcessState() is DiceProcessState.Ready)
            {
                Close();
            }
            else if (GetProcessState() is DiceProcessState.Result)
            {
                Close();
            }
        }
        
        private void OnEnable()
        {
            m_confirmAction.action.started += OnConfirmInput;
            m_cancelAction.action.started += OnCancelInput;
        }

        private void OnDisable()
        {
            m_confirmAction.action.started -= OnConfirmInput;
            m_cancelAction.action.started -= OnCancelInput;
        }
        
        private void Start()
        {
            ChangeProcessState(DiceProcessState.Ready);
        }

        private void Update()
        {
            UpdateProcessState(GetProcessState(), Time.deltaTime);
        }
    }

    public enum DiceProcessState
    {
        Unknown,
        Ready,
        Rolling,
        Bonuses,
        Result,
        Exit,
    }
    
    public class DiceProcess : YieldProcess<DiceResult>
    {
        public readonly DiceProcessController Dice;
        private DiceResult m_diceResult;
        
        public DiceProcess(DiceProcessController dice)
        {
            Dice = dice;
        }
        
        protected override IEnumerator OnProcessWithResult()
        {
            while (Dice.InProgress())
            {
                yield return null;
            }

            if (Dice.IsCanceled())
            {
                Cancel();
            }
            else
            {
                m_diceResult = Dice.GetDiceResult(); 
            }
        }

        protected override DiceResult GetResult()
        {
            return m_diceResult;
        }
    }
}