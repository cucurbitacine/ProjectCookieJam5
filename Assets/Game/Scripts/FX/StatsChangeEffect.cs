using System.Collections.Generic;
using Game.Scripts.Pawns;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Game.Scripts.FX
{
    public class StatsChangeEffect : MonoBehaviour
    {
        [SerializeField] private Vector2 m_offset = Vector2.up;
        [SerializeField] private float m_radius = 1f;
        [SerializeField]
        private Dictionary<StatsType, TextPopupAsset> m_popups = new Dictionary<StatsType, TextPopupAsset>();
        
        [Header("Colors")]
        [SerializeField] private Color m_defaultColor = Color.softYellow;
        [SerializeField]
        private Dictionary<StatsType, Color> m_colors = new Dictionary<StatsType, Color>();

        [SerializeField] private Color m_minusColor = Color.softRed;
        [SerializeField] private Color m_plusColor = Color.softGreen;
        
        private PawnController m_pawn;
        
        private void OnPawnStatsChanged(StatsType type, int prev, int next)
        {
            if (!m_popups.TryGetValue(type, out var popup)) return;
            
            var delta = next - prev;
            if (delta == 0) return;

            var numberColor = delta > 0 ? m_plusColor : m_minusColor;
            var typeColor = m_colors.GetValueOrDefault(type, m_defaultColor);

            var htmlNumberColor = ColorUtility.ToHtmlStringRGB(numberColor);
            var htmlTypeColor = ColorUtility.ToHtmlStringRGB(typeColor);
            
            var numberText = $"{delta}";
            if (delta > 0) numberText = $"+{numberText}";
            numberText = $"<color=#{htmlNumberColor}>{numberText}</color>";
            
            var typeText = $"{type}";
            typeText = $"<color=#{htmlTypeColor}>{typeText}</color>";

            var text = $"{numberText} {typeText}";
            
            var point = (Vector2)m_pawn.transform.TransformPoint(m_offset);
            point += Random.insideUnitCircle.normalized * (Random.value * m_radius);
            
            var textPopup = popup.Popup(text, point);
        }
        
        private void Awake()
        {
            m_pawn = GetComponentInParent<PawnController>();
        }

        private void Start()
        {
            m_pawn.GetStats().StatsChanged += OnPawnStatsChanged;
        }

        private void OnDestroy()
        {
            m_pawn.GetStats().StatsChanged -= OnPawnStatsChanged;
        }
    }
}