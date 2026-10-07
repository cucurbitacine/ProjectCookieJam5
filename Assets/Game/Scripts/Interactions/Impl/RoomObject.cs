using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    [DisallowMultipleComponent]
    public class RoomObject : MonoBehaviour
    {
        [SerializeField] private RoomAsset m_roomPreset;

        public RoomAsset GetRoomPreset()
        {
            return m_roomPreset;
        }
    }
}