using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class SinkObject : ActionObject
    {
        [Header("Sink")]
        [SerializeField] private bool m_dishFlag;
        [Min(0)]
        [SerializeField] private int m_debuffComfortOnStartDay;
        
        public bool HasDishFlag()
        {
            return m_dishFlag;
        }
        
        public void SetDishFlag(bool flag)
        {
            m_dishFlag = flag;
        }
        
        public override bool HasAccess(GameObject actor)
        {
            return HasDishFlag() && base.HasAccess(actor);
        }

        public override void OnDayStart(PawnController pawn)
        {
            base.OnDayStart(pawn);

            if (HasDishFlag())
            {
                pawn.GetStats().AddComfort(-m_debuffComfortOnStartDay);
            }
        }
        
        protected override void OnSuccess(PawnController pawn)
        {
            base.OnSuccess(pawn);
            
            SetDishFlag(false);
        }
    }
}