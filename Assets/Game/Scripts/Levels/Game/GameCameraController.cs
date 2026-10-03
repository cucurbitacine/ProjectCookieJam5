using System.Collections;
using CucuTools.LevelSystem;
using Unity.Cinemachine;
using UnityEngine;

namespace Game.Scripts.Levels.Game
{
    public class GameCameraController : LevelComponent
    {
        [SerializeField] private CinemachineCamera m_playerCamera;
        
        private GameLevelController m_level;
        private PawnPlayerController m_player;
        
        public override void ResolveAll(LevelContext context)
        {
            m_level = context.Resolve<GameLevelController>();
            
            m_player = m_level.GetPlayer();
        }

        public override IEnumerator OnStartLevel()
        {
            m_playerCamera.Follow = m_player.GetPawn().transform;   
            yield break;
        }

        public override void OnDestroyLevel()
        {
            m_playerCamera.Follow = null;
        }
    }
}
