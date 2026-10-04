using System.Collections.Generic;
using UnityEngine;

namespace Game.Scripts.Interactions
{
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Collider2D))]
    public class InteractTrigger : MonoBehaviour
    {
        #region Static

        private static readonly Dictionary<Collider2D, InteractTrigger> Cache =
            new Dictionary<Collider2D, InteractTrigger>();
        
        public static bool Lookup(Collider2D cld2D, out InteractTrigger trigger)
        {
            return Cache.TryGetValue(cld2D, out trigger);
        }

        private static void Register(InteractTrigger trigger)
        {
            Cache[trigger.GetCollider()] = trigger;
        }
        
        private static bool Unregister(InteractTrigger trigger)
        {
            return Cache.Remove(trigger.GetCollider());
        }

        #endregion

        [SerializeField] private InteractTarget m_target;
        
        private Collider2D m_collider2D;
        
        public Collider2D GetCollider()
        {
            return m_collider2D;
        }

        public InteractTarget GetTarget()
        {
            return m_target;
        }
        
        private void Awake()
        {
            m_collider2D = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            Register(this);
        }

        private void OnDisable()
        {
            Unregister(this);
        }
    }
}