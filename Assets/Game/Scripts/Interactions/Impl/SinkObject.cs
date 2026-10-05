using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class SinkObject : ActionObject
    {
        [Header("Sink")]
        [SerializeField] private bool m_dishFlag;
        [SerializeField] private int m_debuffComfortOnStartDay;
        
        public bool HasDishFlag()
        {
            return m_dishFlag;
        }
        
        public int GetDebuffComfortOnStartDay()
        {
            return m_debuffComfortOnStartDay;
        }
        
        public void SetDishFlag(bool flag)
        {
            m_dishFlag = flag;
        }
        
        public override bool HasAccess(GameObject actor)
        {
            return HasDishFlag();
        }
        
        protected override void OnSuccess(PawnController pawn)
        {
            SetDishFlag(false);
        }
    }
}