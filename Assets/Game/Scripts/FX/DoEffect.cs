using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.FX
{
    public abstract class DoEffect : MonoBehaviour
    {
        [SerializeField] private Transform _targetObject;
        [SerializeField] private EffectEvent effectEvent = EffectEvent.Manual;
        
        private Tweener _tweener;
        
        public abstract Tweener CreateTweener(Transform targetObject);
        
        [ContextMenu(nameof(LaunchEffect))]
        public void LaunchEffect()
        {
            KillTweener();
            
            _tweener = CreateTweener(GetTargetObject());
        }
        
        public void KillTweener()
        {
            _tweener?.Kill(true);
        }

        public Transform GetTargetObject()
        {
            return _targetObject != null ? _targetObject : (_targetObject = transform);
        }
        
        protected virtual void OnEnable()
        {
            if (effectEvent is EffectEvent.OnEnable) LaunchEffect();
        }
        
        protected virtual void Start()
        {
            if (effectEvent is EffectEvent.Start) LaunchEffect();
        }
        
        protected virtual void OnDisable()
        {
            KillTweener();
        }
    }
    
    public enum EffectEvent
    {
        Manual,
        OnEnable,
        Start,
    }
}