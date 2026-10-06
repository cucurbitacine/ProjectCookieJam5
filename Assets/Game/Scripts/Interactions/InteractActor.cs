using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Interactions
{
    public class InteractActor : MonoBehaviour
    {
        [SerializeField] private GameObject m_actor;
        
        [Space]
        [SerializeField] private InteractType m_interactType;
        [SerializeField] private Vector3 m_interactPoint;
        [Min(0f)]
        [SerializeField] private float m_interactRadius = 0.5f;
        [SerializeField] private Vector2 m_interactSize = Vector2.one;
        [SerializeField] private ContactFilter2D m_interactFilter = default;
        [SerializeField] private bool m_useWorldSpace;
        
        [Space]
        [SerializeField] private SelectType m_selectType;
        [SerializeField] private InteractableObject m_selectedTarget;
        private InteractableObject m_oldestTarget;
        private InteractableObject m_newestTarget;
        
        private readonly HashSet<InteractableObject> m_checking = new HashSet<InteractableObject>();
        private readonly List<InteractableObject> m_observed = new List<InteractableObject>();

        public void Mute(bool mute)
        {
            enabled = !mute;
        } 
        
        public InteractType GetInteractType()
        {
            return m_interactType;
        }
        
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

        public Vector2 GetInteractSize()
        {
            return m_interactSize;
        }
        
        public ContactFilter2D GetInteractFilter()
        {
            return m_interactFilter;
        }

        public void Interact(bool value)
        {
            m_selectedTarget?.Interact(GetActor(), value);
        }

        public GameObject GetActor()
        {
            return m_actor == null ? (m_actor = gameObject) : m_actor;
        }

        private ColliderArray2D Overlap()
        {
            switch (GetInteractType())
            {
                case InteractType.Circle: return OverlapCircle();
                case InteractType.Box: return OverlapBox();
                default: return default;
            }
        }
        
        private ColliderArray2D OverlapCircle()
        {
            var point = GetInteractPoint();
            var radius = GetInteractRadius();
            var filter = GetInteractFilter();
            
            return Physics2D.OverlapCircle(point, radius, filter);
        }

        private ColliderArray2D OverlapBox()
        {
            var point = GetInteractPoint();
            var size = GetInteractSize();
            var filter = GetInteractFilter();
            
            return  Physics2D.OverlapBox(point, size, 0f, filter);
        }
        
        private bool TryGetTarget(Collider2D other, out InteractableObject target)
        {
            if (InteractTrigger.Lookup(other, out var trigger))
            {
                target = trigger.GetTarget();
                return target ? target.HasAccess(GetActor()) : false;
            }

            target = null;
            return false;
        }
        
        private void OnEnter(InteractableObject target)
        {
            target.Hover(GetActor(), true);
            
            if (m_observed.Count == 0)
            {
                ChangeOldest(target);
            }
            
            ChangeNewest(target);
            
            m_observed.Add(target);
        }
        
        private void OnExit(int index, InteractableObject target)
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
            
            target.Hover(GetActor(), false);
        }

        private void ChangeOldest(InteractableObject target)
        {
            if (m_oldestTarget == target) return;
            
            m_oldestTarget = target;

            if (m_selectType == SelectType.Oldest) ChangeSelected(m_oldestTarget);
        }
        
        private void ChangeNewest(InteractableObject target)
        {
            if (m_newestTarget == target) return;
            
            m_newestTarget = target;
            
            if (m_selectType == SelectType.Newest) ChangeSelected(m_newestTarget);
        }

        private void ChangeSelected(InteractableObject target)
        {
            if (m_selectedTarget == target) return;
            
            m_selectedTarget?.Select(GetActor(), false);
            m_selectedTarget = target;
            m_selectedTarget?.Select(GetActor(), true);
        }

        private void CheckTargets()
        {
            var result = Overlap();
            
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
            if (GetInteractType() is InteractType.Circle)
            {
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
            else if (GetInteractType() is InteractType.Box)
            {
                var size = GetInteractSize();
                Gizmos.DrawWireCube(point, size);
            }
        }
    }

    public enum InteractType
    {
        Circle,
        Box,
    }
    
    public enum SelectType
    {
        Oldest,
        Newest,
    }
}
