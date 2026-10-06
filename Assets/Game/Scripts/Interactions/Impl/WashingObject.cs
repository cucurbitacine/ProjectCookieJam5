
using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class WashingObject : DiceActionObject
    {
        [Header("Washing")]
        [Min(0)]
        [SerializeField] private int m_debuffComfortOnStartDay;
        
        public override void OnDayStart(PawnController pawn)
        {
            base.OnDayStart(pawn);

            if (!HasDoneFlag())
            {
                pawn.GetStats().AddComfort(-m_debuffComfortOnStartDay);
            }
        }
    }
}