using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class FridgeObject : ActionObject
    {
        [Header("Fridge")]
        [SerializeField] private bool m_foodFlag;
        
        private SinkObject m_sink;
        
        public bool HasFoodFlag()
        {
            return m_foodFlag;
        }
        
        public void SetFoodFlag(bool flag)
        {
            m_foodFlag = flag;
        }

        public SinkObject GetSink()
        {
            return m_sink;
        }
        
        public override bool HasAccess(GameObject actor)
        {
            var sink = GetSink();
            if (sink && sink.HasDishFlag()) return false;
            
            return HasFoodFlag() && base.HasAccess(actor);
        }
        
        protected override void OnAttempt(PawnController pawn)
        {
            base.OnAttempt(pawn);
            
            SetFoodFlag(false);
            
            GetSink()?.SetDishFlag(true);
        }
        
        private void Awake()
        {
            m_sink = FindAnyObjectByType<SinkObject>();
        }
    }
}