using System;
using System.Collections;
using Game.Scripts.Pawns;
using UnityEngine;

namespace Game.Scripts.Levels
{
    public class TeleportationManager : MonoBehaviour
    {
        [Min(0f)] [SerializeField] private float m_durationIn = 0.5f;
        [Min(0f)] [SerializeField] private float m_durationOut = 0.5f;

        public float GetDurationIn()
        {
            return m_durationIn;
        }
        
        public float GetDurationOut()
        {
            return m_durationOut;
        }
        
        public TeleportationProcess CreateTeleportProcess(PawnController pawn, Transform destination)
        {
            return new TeleportationProcess(pawn, destination, GetDurationIn(), GetDurationOut());
        }
    }

    public class TeleportationProcess : YieldProcess
    {
        public readonly PawnController Pawn;
        public readonly Transform Destination;
        public readonly float TimeIn;
        public readonly float TimeOut;

        public static Action<TeleportationProcess, bool> TeleportStarted;
        
        public TeleportationProcess(PawnController pawn, Transform destination, float timeIn, float timeOut)
        {
            Pawn = pawn;
            Destination = destination;
            TimeIn = Mathf.Max(0f, timeIn);
            TimeOut = Mathf.Max(0f, timeOut);
        }
        
        public override IEnumerator Process()
        {
            Pawn.Mute(true);

            try
            {
                TeleportStarted?.Invoke(this, true);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
            yield return new WaitForSeconds(TimeIn);

            Teleport();
            
            try
            {
                TeleportStarted?.Invoke(this, false);
            }
            catch (Exception e)
            {
                Debug.LogError(e);
            }
            
            yield return new WaitForSeconds(TimeOut);
            
            Pawn.Mute(false);
        }

        private void Teleport()
        {
            Pawn.transform.position = Destination.position;
        }
    }
}