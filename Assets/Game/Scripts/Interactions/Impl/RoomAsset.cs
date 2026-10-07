using UnityEngine;

namespace Game.Scripts.Interactions.Impl
{
    [CreateAssetMenu(menuName = "Create RoomAsset", fileName = "RoomAsset", order = 0)]
    public class RoomAsset : ScriptableObject
    {
        [SerializeField] private string m_roomName;

        public string GetRoomName()
        {
            return m_roomName;
        }
    }
}