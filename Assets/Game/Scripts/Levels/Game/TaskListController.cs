using Game.Scripts.Interactions.Impl;
using UnityEngine;

namespace Game.Scripts.Levels.Game
{
    public class TaskListController : MonoBehaviour
    {
        private SinkObject m_sink;
        private VacuumObject m_vacuum;
        private ClosetObject m_closet; 
        private WashingObject m_washing;

        public bool IsAllDone()
        {
            return IsSinkDone() && IsVacuumDone() && IsClosetDone() && IsWashingDone();
        }
        
        public bool IsSinkDone()
        {
            return !m_sink.HasDishFlag();
        }
        
        public bool IsVacuumDone()
        {
            return m_vacuum.HasDonePerGameFlag();
        }
        
        public bool IsClosetDone()
        {
            return m_closet.HasDonePerGameFlag();
        }
        
        public bool IsWashingDone()
        {
            return m_washing.HasDonePerGameFlag();
        }
        
        private void Awake()
        {
            m_sink = FindAnyObjectByType<SinkObject>();
            m_vacuum = FindAnyObjectByType<VacuumObject>();
            m_closet = FindAnyObjectByType<ClosetObject>();
            m_washing = FindAnyObjectByType<WashingObject>();
        }
    }
}