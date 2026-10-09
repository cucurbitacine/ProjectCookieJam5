using System.Linq;
using Game.Scripts.Interactions;
using Game.Scripts.Interactions.Impl;
using Game.Scripts.Pawns;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.Game
{
    public class ActionObjectHint : MonoBehaviour
    {
        [SerializeField] private InteractState m_updateState = InteractState.Select;
        [SerializeField] private ColorGradientAsset m_chanceColor;
        
        [Header("Action Object")]
        [SerializeField] private TMP_Text m_difficulty;
        [SerializeField] private TMP_Text m_chance;
        [SerializeField] private TMP_Text m_cost;
        [SerializeField] private TMP_Text m_reward;

        private ActionObject m_actionObject;
        
        public ActionObject GetActionObject()
        {
            return m_actionObject;
        }
        
        private void OnInteractStateChanged(GameObject actor, InteractState state, bool value)
        {
            var actionObject = GetActionObject();
            
            //Debug.Log($"[{actionObject.name}] {actor.name} / {state} / {value}");
            
            if (state != m_updateState) return;
            if (!value) return;
            if (!actor.TryGetComponent(out PawnController pawn)) return;
            
            
            var actionPreset = actionObject.GetActionPreset();
            if (actionPreset == null) return;
            
            var difficulty = actionPreset.GetDifficulty();
            m_difficulty.text = $"{difficulty}";
            
            var chance = DiceResult.EvaluateChance(difficulty, pawn.GetDice().GetBonuses().Sum(b => b.Value));
            m_chance.text = $"{(chance * 100):F0}%";
            if (m_chanceColor) m_chance.color = m_chanceColor.Evaluate(chance);
            
            var cost = actionPreset.GetCost();
            var costText = $"";
            if (cost.ActionPoints != 0) costText += $"-{cost.ActionPoints} AP\n";
            if (cost.Will != 0) costText += $"-{cost.Will} Will\n";
            m_cost.text = costText;
            
            var reward = actionPreset.GetReward();
            var rewardText = $"";
            if (reward.Will != 0) rewardText += $"+{reward.Will} Will\n";
            if (reward.Comfort != 0) rewardText += $"+{reward.Comfort} Comfort\n";
            m_reward.text = rewardText;
        }

        private void Awake()
        {
            m_actionObject = GetComponentInParent<ActionObject>();
        }
        
        private void OnEnable()
        {
            if (m_actionObject)
            {
                m_actionObject.InteractStateChanged += OnInteractStateChanged;
            }
        }

        private void OnDisable()
        {
            if (m_actionObject)
            {
                m_actionObject.InteractStateChanged -= OnInteractStateChanged;
            }
        }

        private void Start()
        {
            m_difficulty.text = string.Empty;
            m_chance.text = string.Empty;
            m_cost.text = string.Empty;
            m_reward.text = string.Empty;
        }
    }
}
