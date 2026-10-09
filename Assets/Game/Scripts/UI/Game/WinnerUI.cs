using System;
using Game.Scripts.Levels.Game;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.Game
{
    public class WinnerUI : MonoBehaviour
    {
        [SerializeField] private TMP_Text m_text;

        private GameLevelController m_gameLevel;
        
        private void Awake()
        {
            m_gameLevel = FindAnyObjectByType<GameLevelController>();
        }

        private void Update()
        {
            m_text.text = $"WON in {m_gameLevel.GetCurrentDay()} days";
        }
    }
}
