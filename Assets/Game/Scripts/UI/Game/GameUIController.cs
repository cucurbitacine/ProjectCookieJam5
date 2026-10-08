using System;
using System.Collections;
using CucuTools.LevelSystem;
using CucuTools.Utilities;
using Game.Scripts.FX;
using Game.Scripts.Levels;
using Game.Scripts.Levels.Game;
using Game.Scripts.Pawns;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Game
{
    public class GameUIController : LevelComponent
    {
        [SerializeField] private Button returnMenuButton;
        [SerializeField] private Image m_blackout;
        [Space]
        [SerializeField] private TMP_Text m_textStatsActionPoints;
        [SerializeField] private TMP_Text m_textStatsWill;
        [SerializeField] private TMP_Text m_textStatsComfort;
        [Space]
        [SerializeField] private TMP_Text m_textDay;
        
        private DoPunchEffect m_effectStatsActionPoints;
        private DoPunchEffect m_effectStatsWill;
        private DoPunchEffect m_effectStatsComfort;
        private DoPunchEffect m_effectDay;
        
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

        private void UpdatePawnStats(PawnStatsData pawnStatsData)
        {
            m_textStatsActionPoints.text = $"{pawnStatsData.ActionPoints}";
            m_textStatsWill.text = $"{pawnStatsData.Will}";
            m_textStatsComfort.text = $"{pawnStatsData.Comfort}";
            
            m_effectStatsActionPoints?.LaunchEffect();
            m_effectStatsWill?.LaunchEffect();
            m_effectStatsComfort?.LaunchEffect();
        }
        
        private void OnTeleportStarted(TeleportationProcess teleport, bool started)
        {
            Blackout(started, started ? teleport.TimeIn : teleport.TimeOut);
        }
        
        private void OnPawnStatsChanged(StatsType type, int prev, int next)
        {
            switch (type)
            {
                case StatsType.ActionPoints:
                    m_textStatsActionPoints.text = $"{next}";
                    m_effectStatsActionPoints?.LaunchEffect();
                    break;
                case StatsType.Will:
                    m_textStatsWill.text = $"{next}";
                    m_effectStatsWill?.LaunchEffect();
                    break;
                case StatsType.Comfort:
                    m_textStatsComfort.text = $"{next}";
                    m_effectStatsComfort?.LaunchEffect();
                    break;
            }
        }
        
        private void OnDayChanged(int prev, int next)
        {
            m_textDay.text = $"{next}";
            
            m_effectDay?.LaunchEffect();
        }
        
        public override void ResolveAll(LevelContext context)
        {
            m_gameLevel = context.Resolve<GameLevelController>();
        }

        public override IEnumerator OnStartLevel()
        {
            returnMenuButton.onClick.AddListener(OnReturnMenuButtonClick);

            TeleportationProcess.TeleportStarted += OnTeleportStarted;

            m_gameLevel.GetPlayer().GetPawn().GetStats().StatsChanged += OnPawnStatsChanged;
            UpdatePawnStats(m_gameLevel.GetPlayer().GetPawn().GetStats().GetStatsData());

            m_gameLevel.DayChanged += OnDayChanged;
            OnDayChanged(0, 0);
                
            yield return StartCoroutine(BlackoutProcess(false, 1f));
        }

        public override void OnDestroyLevel()
        {
            TeleportationProcess.TeleportStarted -= OnTeleportStarted;
            
            m_gameLevel.GetPlayer().GetPawn().GetStats().StatsChanged -= OnPawnStatsChanged;
            
            m_gameLevel.DayChanged -= OnDayChanged;
        }

        private void Awake()
        {
            Blackout(true);

            m_textStatsActionPoints.TryGetComponent(out m_effectStatsActionPoints);
            m_textStatsWill.TryGetComponent(out m_effectStatsWill);
            m_textStatsComfort.TryGetComponent(out m_effectStatsComfort);
            m_textDay.TryGetComponent(out m_effectDay);
        }
    }
}