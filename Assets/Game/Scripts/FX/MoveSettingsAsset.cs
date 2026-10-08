using UnityEngine;

namespace Game.Scripts.FX
{
    [CreateAssetMenu(menuName = "Create MoveSettingsAsset", fileName = "MoveSettingsAsset", order = 0)]
    public class MoveSettingsAsset : ScriptableObject
    {
        [SerializeField] private Vector3 m_vector = Vector3.up;
        [SerializeField] private Vector3 m_vectorThreshold = Vector3.zero;
        [SerializeField] private float m_radiusThreshold = 0f;
        
        [Space]
        [SerializeField] private float m_durationMin = 1f;
        [SerializeField] private float m_durationThreshold = 0f;
        
        [Space]
        public bool snapping = false;
        
        public Vector3 endValue => Vector3.Lerp(m_vector, m_vector + m_vectorThreshold, Random.value) +
                                   Random.onUnitSphere * m_radiusThreshold;
        public float duration => m_durationMin + Random.Range(0f, m_durationThreshold);
    }
}