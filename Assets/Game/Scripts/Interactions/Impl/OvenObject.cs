using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class OvenObject : ActionObject
    {
        private FridgeObject m_fridge;
        private SinkObject m_sink;

        public FridgeObject GetFridge()
        {
            return m_fridge;
        }
        
        public SinkObject GetSink()
        {
            return m_sink;
        }
        
        public override bool HasAccess(GameObject actor)
        {
            var fridge = GetFridge();
            if (fridge && fridge.HasFoodFlag()) return false;
            
            var sink = GetSink();
            if (sink && sink.HasDishFlag()) return false;
            
            return base.HasAccess(actor);
        }

        protected override void OnSuccess(PawnController pawn)
        {
            base.OnSuccess(pawn);
            
            GetFridge()?.SetFoodFlag(true);
        }

        protected override void OnFailure(PawnController pawn)
        {
            base.OnFailure(pawn);
            
            GetSink()?.SetDishFlag(true);
        }

        private void Awake()
        {
            m_fridge = FindAnyObjectByType<FridgeObject>();
            m_sink = FindAnyObjectByType<SinkObject>();
        }
    }
}