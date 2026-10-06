using UnityEngine;

namespace Game.Scripts.Pawns
{
    public class PawnRotation : MonoBehaviour
    {
        [SerializeField] private float m_direction = 1f;
        [SerializeField] private Transform m_targetRotation;

        public void Mute(bool mute)
        {
            enabled = !mute;
        }
        
        public float GetDirection()
        {
            return m_direction;
        }
        
        public Transform GetTargetRotation()
        {
            return m_targetRotation;
        }

        public void UpdateDirection()
        {
            var target = GetTargetRotation();
            target.localScale = new Vector3(GetDirection(), 1f, 1f);
        }
        
        public void ChangeDirection(float direction)
        {
            if (Mathf.Approximately(direction, 0f)) return;

            if (m_direction * direction > 0f) return;
            m_direction = Mathf.Sign(direction);

            UpdateDirection();
        }
        
        private void SetupTargetRotation()
        {
            if (m_targetRotation == null) m_targetRotation = transform;
        }
        
        private void Awake()
        {
            SetupTargetRotation();
        }
        
        private void Start()
        {
            UpdateDirection();
        }
    }
}