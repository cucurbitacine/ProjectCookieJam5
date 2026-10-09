using UnityEngine;

namespace Game.Scripts.UI.Game
{
    [CreateAssetMenu(menuName = "Create ColorGradientAsset", fileName = "ColorGradientAsset", order = 0)]
    public class ColorGradientAsset : ScriptableObject
    {
        [SerializeField] private Gradient m_gradient;
        
        public Color Evaluate(float t)
        {
            return m_gradient.Evaluate(t);
        }
    }
}