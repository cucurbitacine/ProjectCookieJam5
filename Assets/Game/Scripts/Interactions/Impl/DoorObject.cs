using System;
using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    public class DoorObject : TeleportationObject
    {
        [Header("Door")]
        [SerializeField] private Transform m_origin;
        [SerializeField] private DoorObject m_linkedDoor;

        private RoomObject m_room;
        
        public RoomObject GetRoomObject()
        {
            return m_room;
        }
        
        public Transform GetOrigin()
        {
            if (m_origin == null) m_origin = transform;
            return m_origin;
        }

        public DoorObject GetLinkedDoor()
        {
            return m_linkedDoor;
        }
        
        public void LinkDoor(DoorObject door)
        {
            m_linkedDoor = door;
            
            SetDestination(m_linkedDoor.GetOrigin());

            if (m_linkedDoor.GetLinkedDoor() != this)
            {
                m_linkedDoor.LinkDoor(this);
            }
        }

        public override string GetInteractDescription()
        {
            return $"Go to \"{GetLinkedDoor().GetRoomObject().GetRoomPreset().GetRoomName()}\"";
        }

        protected override void Awake()
        {
            base.Awake();

            m_room = GetComponentInParent<RoomObject>();
        }

        private void Start()
        {
            LinkDoor(m_linkedDoor);
        }
    }
}