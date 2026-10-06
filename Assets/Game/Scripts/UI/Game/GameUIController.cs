using System;
using System.Collections;
using CucuTools.LevelSystem;
using CucuTools.Utilities;
using Game.Scripts.Levels;
using Game.Scripts.Levels.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Game
{
    public class GameUIController : LevelComponent
    {
        [SerializeField] private Button returnMenuButton;
        [SerializeField] private Image m_blackout;
        
        private GameLevelController m_gameLevel;

        private Coroutine m_blackoutProcess;
        
        private void OnReturnMenuButtonClick()
        {
            m_gameLevel.ReturnMenu();
        }

        private void Blackout(bool isOn, float fadeDuration = 0f)
        {
            if (fadeDuration > 0f)
            {
                if (m_blackoutProcess != null) StopCoroutine(m_blackoutProcess);
                m_blackoutProcess = StartCoroutine(BlackoutProcess(isOn, fadeDuration));
            }
            else
            {
                m_blackout.color = m_blackout.color.Alpha(isOn ? 1f : 0f);
                m_blackout.enabled = isOn;
            }
        }

        private IEnumerator BlackoutProcess(bool isOn, float fadeDuration)
        {
            var alphaOrigin = m_blackout.color.a;
            var alphaTarget = isOn ? 1f : 0f;

            m_blackout.enabled = true;
            
            var fadeTime = 0f;
            while (fadeTime < fadeDuration)
            {
                var t = fadeTime / fadeDuration;
                t = Mathf.SmoothStep(0f, 1f, t);
                var alpha = Mathf.Lerp(alphaOrigin, alphaTarget, t);
                
                m_blackout.color = m_blackout.color.Alpha(alpha);
                
                fadeTime += Time.deltaTime;
                yield return null;
            }

            m_blackout.color = m_blackout.color.Alpha(alphaTarget);
            m_blackout.enabled = isOn;
        }

        private void OnTeleportStarted(TeleportationProcess teleport, bool started)
        {
            Blackout(started, started ? teleport.TimeIn : teleport.TimeOut);
        }
        
        public override void ResolveAll(LevelContext context)
        {
            m_gameLevel = context.Resolve<GameLevelController>();
        }

        public override IEnumerator OnStartLevel()
        {
            returnMenuButton.onClick.AddListener(OnReturnMenuButtonClick);

            TeleportationProcess.TeleportStarted += OnTeleportStarted;
            
            yield return StartCoroutine(BlackoutProcess(false, 1f));
        }

        public override void OnDestroyLevel()
        {
            TeleportationProcess.TeleportStarted -= OnTeleportStarted;
        }

        private void Awake()
        {
            Blackout(true);
        }
    }
}