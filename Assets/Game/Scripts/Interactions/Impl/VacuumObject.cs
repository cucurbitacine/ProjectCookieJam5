using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class VacuumObject : ActionObject
    {
        [Header("Vacuum")]
        [Min(0)]
        [SerializeField] private int m_debuffComfortOnStartDay;
        
        public override void OnDayStart(PawnController pawn)
        {
            base.OnDayStart(pawn);

            if (!HasDonePerGameFlag())
            {
                pawn.GetStats().AddComfort(-m_debuffComfortOnStartDay);
            }
        }
    }
}