using UnityEngine;

namespace Game.Scripts.FX
{
    [CreateAssetMenu(menuName = "Create OutlineSpriteAsset", fileName = "OutlineSpriteAsset", order = 0)]
    public class OutlineSpriteAsset : ScriptableObject
    {
        [SerializeField] private Color m_outlineColor = Color.darkOrange;
        [SerializeField] private float m_outlineThickness = 1f;

        public Color GetColor()
        {
            return m_outlineColor;
        }
        
        public float GetThickness()
        {
            return m_outlineThickness;
        }
    }
}