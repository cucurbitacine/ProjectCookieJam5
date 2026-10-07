using DG.Tweening;
using UnityEngine;

namespace Game.Scripts.FX
{
    public class DoPunchEffect : DoEffect
    {
        [Header("DoPunch")]
        [SerializeField] private PunchSettingsAsset punchSettings;
        [Space]
        [SerializeField] private PunchType punchType = PunchType.Scale;
        [SerializeField] private bool useWorldSpace;

        public override Tweener CreateTweener(Transform target)
        {
            var punch = punchSettings.punch;

            if (useWorldSpace)
            {
                punch = target.InverseTransformVector(punchSettings.punch);
            }

            return DoPunch(target, punch, punchSettings.duration, punchSettings.vibrato, punchSettings.elasticity);
        }
        
        private Tweener DoPunch(Transform target, Vector3 punch, float duration, int vibrato, float elasticity)
        {
            return DoPunch(target, punchType, punch, duration, vibrato, elasticity);
        }
        
        private static Tweener DoPunch(Transform origin, PunchType punchType, Vector3 punch, float duration, int vibrato, float elasticity)
        {
            switch (punchType)
            {
                case PunchType.Position: return origin.DOPunchPosition(punch, duration, vibrato, elasticity);
                case PunchType.Rotation: return origin.DOPunchRotation(punch, duration, vibrato, elasticity);
                case PunchType.Scale: return origin.DOPunchScale(punch, duration, vibrato, elasticity);
            }

            return null;
        }
    }
}
