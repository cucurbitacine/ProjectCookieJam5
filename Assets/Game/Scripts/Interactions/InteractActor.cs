using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Interactions
{
    public class InteractActor : MonoBehaviour
    {
        [SerializeField] private Vector3 m_interactPoint;
        [Min(0f)]
        [SerializeField] private float m_interactRadius;
        [SerializeField] private ContactFilter2D m_interactFilter = default;
        [SerializeField] private bool m_useWorldSpace;
        [SerializeField] private SelectType m_selectType;

        [Space]
        [SerializeField] private InteractTarget m_selectedTarget;
        [SerializeField] private InteractTarget m_oldestTarget;
        [SerializeField] private InteractTarget m_newestTarget;
        
        private readonly HashSet<InteractTarget> m_checking = new HashSet<InteractTarget>();
        private readonly List<InteractTarget> m_observed = new List<InteractTarget>();
        
        public void SetInteractPoint(Vector2 point)
        {
            m_interactPoint = m_useWorldSpace ? point : transform.InverseTransformPoint(point);
        }
        
        public Vector3 GetInteractPoint()
        {
            return m_useWorldSpace ? m_interactPoint : transform.TransformPoint(m_interactPoint);
        }

        public float GetInteractRadius()
        {
            return m_interactRadius;
        }

        public ContactFilter2D GetInteractFilter()
        {
            return m_interactFilter;
        }

        public void Interact(bool value)
        {
            m_selectedTarget?.Interact(gameObject, value);
        }
        
        private ColliderArray2D OverlapCircle()
        {
            var point = GetInteractPoint();
            var radius = GetInteractRadius();
            var filter = GetInteractFilter();
            
            return  Physics2D.OverlapCircle(point, radius, filter);
        }

        private bool TryGetTarget(Collider2D other, out InteractTarget target)
        {
            if (InteractTrigger.Lookup(other, out var trigger))
            {
                target = trigger.GetTarget();
                return true;
            }

            target = null;
            return false;
        }
        
        private void OnEnter(InteractTarget target)
        {
            target.Hover(gameObject, true);
            
            if (m_observed.Count == 0)
            {
                ChangeOldest(target);
            }
            
            ChangeNewest(target);
            
            m_observed.Add(target);
        }
        
        private void OnExit(int index, InteractTarget target)
        {
            m_observed.RemoveAt(index);

            if (index == 0)
            {
                ChangeOldest(m_observed.Count > 0 ? m_observed[0] : null);
            }
            
            if (index == m_observed.Count)
            {
                ChangeNewest(m_observed.Count > 0 ? m_observed[m_observed.Count - 1] : null);
            }
            
            target.Hover(gameObject, false);
        }

        private void ChangeOldest(InteractTarget target)
        {
            if (m_oldestTarget == target) return;
            
            m_oldestTarget = target;

            if (m_selectType == SelectType.Oldest) ChangeSelected(m_oldestTarget);
        }
        
        private void ChangeNewest(InteractTarget target)
        {
            if (m_newestTarget == target) return;
            
            m_newestTarget = target;
            
            if (m_selectType == SelectType.Newest) ChangeSelected(m_newestTarget);
        }

        private void ChangeSelected(InteractTarget target)
        {
            if (m_selectedTarget == target) return;
            
            m_selectedTarget?.Select(gameObject, false);
            m_selectedTarget = target;
            m_selectedTarget?.Select(gameObject, true);
        }

        private void CheckTargets()
        {
            var result = OverlapCircle();
            
            m_checking.Clear();
            for (var i = 0; i < result.Length; i++)
            {
                if (TryGetTarget(result[i], out var target))
                {
                    m_checking.Add(target);
                }
            }

            for (var i = m_observed.Count - 1; i >= 0; i--)
            {
                var observed = m_observed[i];
                
                if (m_checking.Remove(observed))
                {
                    // Remove from checking target which are observed
                }
                else
                {
                    // Remove from observed target which are lost
                    OnExit(i, observed);
                }
            }
            
            // Checking contains only new targets
            foreach (var check in m_checking)
            {
                OnEnter(check);
            }
        }

        private void OnDisable()
        {
            for (var i = m_observed.Count - 1; i >= 0; i--)
            {
                OnExit(i, m_observed[i]);
            }
        }

        private void FixedUpdate()
        {
            CheckTargets();
        }
        
        private void OnDrawGizmos()
        {
            Gizmos.color = m_selectedTarget ? Color.limeGreen : Color.softRed;
            
            var point = GetInteractPoint();
            var radius = GetInteractRadius();
            if (radius > 0f)
            {
                Gizmos.DrawWireSphere(point, radius);
            }
            else
            {
                Gizmos.DrawWireCube(point, Vector3.one * 0.2f);
            }
        }
    }

    public enum SelectType
    {
        Oldest,
        Newest,
    }
}
