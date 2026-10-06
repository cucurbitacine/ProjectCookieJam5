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
        
        private Vector3 m_teleportBeginPosition;
        private Vector3 m_teleportEndPosition;
        
        private Transform GetPlayerFollow()
        {
            return m_player.GetPawn().transform;
        }
        
        private void OnTeleportStarted(TeleportationProcess process, bool started)
        {
            var target = process.Pawn.transform;
            
            if (started)
            {
                m_teleportBeginPosition = target.position;
            }
            else
            {
                m_teleportEndPosition = target.position;

                var delta = m_teleportEndPosition - m_teleportBeginPosition;
            
                m_playerCamera.OnTargetObjectWarped(target, delta);
            }
        }
        
        public override void ResolveAll(LevelContext context)
        {
            m_level = context.Resolve<GameLevelController>();
            
            m_player = m_level.GetPlayer();
        }
        
        public override IEnumerator OnStartLevel()
        {
            m_playerCamera.Follow = GetPlayerFollow();

            TeleportationProcess.TeleportStarted += OnTeleportStarted;
            
            yield break;
        }

        public override void OnDestroyLevel()
        {
            m_playerCamera.Follow = null;
            
            TeleportationProcess.TeleportStarted -= OnTeleportStarted;
        }
    }
}
