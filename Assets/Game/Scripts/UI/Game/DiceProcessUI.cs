using System;
using System.Linq;
using Game.Scripts.FX;
using Game.Scripts.Pawns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace Game.Scripts.UI.Game
{
    public class DiceProcessUI : MonoBehaviour
    {
        [SerializeField] private DiceProcessController m_diceProcess;

        [Header("Action")]
        [SerializeField] private TMP_Text m_textActionName;
        [SerializeField] private TMP_Text m_textActionDifficulty;
        [SerializeField] private TMP_Text m_textActionChance;
        [SerializeField] private TMP_Text m_textActionResult;
        
        [Header("Dice")]
        [SerializeField] private TMP_Text m_textDiceNumber;
        [SerializeField] private TMP_Text m_textDiceBonus;
        [SerializeField] private TMP_Text m_textDiceResult;
        [SerializeField] private Image m_imageDice;
        
        [Header("Buttons")]
        [SerializeField] private Button m_buttonRoll;
        [SerializeField] private Button m_buttonOk;
        [SerializeField] private Button m_buttonClose;

        [Header("Settings")]
        [SerializeField] private float m_fullRotationPerSecond = 8f;
        [SerializeField] private float m_changeNumberPerSecond = 8f;

        private DoPunchEffect m_diceEffect;
        private DoPunchEffect m_diceBonusEffect;
        private DoPunchEffect m_diceResultEffect;
        
        private float m_fakeNumberTimer;
        
        public DiceProcessController GetProcess()
        {
            return m_diceProcess;
        }

        private void OnButtonRollClick()
        {
            GetProcess().Roll();
        }
        
        private void OnButtonCloseClick()
        {
            GetProcess().Close();
        }
        
        private void OnButtonOkClick()
        {
            GetProcess().Close();
        }
        
        private void OnReady()
        {
            var process = GetProcess();
            var action = process.GetAction();
            m_textActionName.text = action.GetInteractName();

            var difficulty = action.GetDifficulty();
            m_textActionDifficulty.text = $"{difficulty}";

            var bonus = process.GetDice().GetBonuses().Sum(b => b.Value);
            m_textDiceBonus.enabled = bonus != 0;
            m_textDiceBonus.text = $"{(bonus > 0 ? "+" : "")}{bonus}";
                
            var chance = DiceResult.EvaluateChance(difficulty, bonus);
            m_textActionChance.text = $"{(chance * 100):F0}%";
            
            m_textDiceResult.enabled = false;
            m_textActionResult.enabled = false;
            
            m_buttonOk.gameObject.SetActive(false);
            
            m_buttonRoll.gameObject.SetActive(true);
            m_buttonRoll.onClick.AddListener(OnButtonRollClick);
            
            m_buttonClose.gameObject.SetActive(true);
            m_buttonClose.onClick.AddListener(OnButtonCloseClick);
        }
        
        private void OnRolling()
        {
            m_buttonRoll.gameObject.SetActive(false);
            m_buttonClose.gameObject.SetActive(false);
            
            m_buttonRoll.onClick.RemoveListener(OnButtonRollClick);
            m_buttonClose.onClick.RemoveListener(OnButtonCloseClick);

            m_fakeNumberTimer = 0f;

            // Start Animation
        }
        
        private void OnBonuses()
        {
            var diceResult = GetProcess().GetDiceResult(); // TODO Handle Crit
            m_textDiceNumber.text = $"{diceResult.Clear}";

            m_imageDice.transform.rotation = Quaternion.identity;

            m_diceEffect.LaunchEffect();
            m_diceBonusEffect.LaunchEffect();
            
            // Start Bonus Animation
        }
        
        private void OnResult()
        {
            var diceResult = GetProcess().GetDiceResult();
            var crit = diceResult.Clear is 1 or 20;
            m_textDiceNumber.text = $"{(crit ? diceResult.Clear: diceResult.Result)}";

            var action = GetProcess().GetAction();
            var difficulty = action.GetDifficulty();
            var success = diceResult.Evaluate(difficulty);

            m_textDiceResult.enabled = true;
            m_textDiceResult.text = $"{(crit ? "CRITICAL " : "")}{(success ? "Success" : "Failure")}";
            
            m_textDiceBonus.enabled = false;
            
            m_textActionResult.enabled = true;
            m_textActionResult.text = action.GetActionResult(success);
            
            m_buttonOk.gameObject.SetActive(true);
            m_buttonOk.onClick.AddListener(OnButtonOkClick);
            
            m_buttonClose.gameObject.SetActive(true);
            m_buttonClose.onClick.AddListener(OnButtonCloseClick);
            
            m_diceEffect.LaunchEffect();
            m_diceResultEffect.LaunchEffect();
        }

        private void OnExit()
        {
            m_buttonOk.gameObject.SetActive(false);
            m_buttonOk.onClick.RemoveListener(OnButtonOkClick);
            
            m_buttonClose.gameObject.SetActive(false);
            m_buttonClose.onClick.RemoveListener(OnButtonCloseClick);
        }
        
        private void OnDiceProcessChanged(DiceProcessState exit, DiceProcessState enter)
        {
            if (enter is DiceProcessState.Ready)
            {
                OnReady();
            }
            else if (enter is DiceProcessState.Rolling)
            {
                OnRolling();
            }
            else if (enter is DiceProcessState.Bonuses)
            {
                OnBonuses();
            }
            else if (enter is DiceProcessState.Result)
            {
                OnResult();
            }
            else if (enter is DiceProcessState.Exit)
            {
                OnExit();
            }
        }
        
        private void UpdateDiceProcess(DiceProcessState state, float deltaTime)
        {
            if (state is DiceProcessState.Rolling)
            {
                var rotationSpeed = 360f * m_fullRotationPerSecond;
                var deltaRotation = rotationSpeed * deltaTime;
                m_imageDice.transform.Rotate(Vector3.forward, deltaRotation);

                m_fakeNumberTimer -= deltaTime;
                if (m_fakeNumberTimer <= 0f)
                {
                    m_fakeNumberTimer = m_changeNumberPerSecond > 0f ? 1f / m_changeNumberPerSecond : 0f;

                    var intFakeNumber = Random.Range(1, 21);
                    m_textDiceNumber.text = $"{intFakeNumber}";
                }

            }
        }
        
        private void Awake()
        {
            m_imageDice.TryGetComponent(out m_diceEffect);
            m_textDiceBonus.TryGetComponent(out m_diceBonusEffect);
            m_textDiceResult.TryGetComponent(out m_diceResultEffect);
        }

        private void OnEnable()
        {
            GetProcess().ProcessStateChanged += OnDiceProcessChanged;
        }

        private void OnDisable()
        {
            GetProcess().ProcessStateChanged -= OnDiceProcessChanged;
        }

        private void Update()
        {
            UpdateDiceProcess(GetProcess().GetProcessState(), Time.deltaTime);
        }
    }
}