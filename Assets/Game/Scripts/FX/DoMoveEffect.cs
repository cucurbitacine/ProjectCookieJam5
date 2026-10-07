using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.FX
{
    public class DoMoveEffect : DoEffect
    {
        [Header("DoMove")]
        [SerializeField] private MoveSettingsAsset moveSettings;
        [SerializeField] private bool useWorldSpace;
        
        public override Tweener CreateTweener(Transform target)
        {
            var endValue = transform.TransformPoint(moveSettings.endValue);

            if (useWorldSpace)
            {
                endValue = transform.position + moveSettings.endValue;
            }

            return target.DOMove(endValue, moveSettings.duration, moveSettings.snapping);
        }
    }
}