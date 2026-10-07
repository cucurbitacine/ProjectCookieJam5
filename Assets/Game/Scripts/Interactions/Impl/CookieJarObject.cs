using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class CookieJarObject : ActionObject
    {
        [SerializeField] private int m_nextDiceBonus = 2;
        
        protected override void OnSuccess(PawnController pawn)
        {
            base.OnSuccess(pawn);
            
            pawn.GetDice().AddNextDiceBonus(m_nextDiceBonus);
        }
    }
}