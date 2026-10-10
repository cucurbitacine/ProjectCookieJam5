using UnityEngine;

namespace Game.Scripts.SoundSystem
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundSource : MonoBehaviour
    {
        [field: SerializeField] public SoundFxPreset SoundFx { get; private set; }

        private AudioSource _audioSource;

        public AudioSource GetAudioSource()
        {
            if (_audioSource == null)
            {
                _audioSource = GetComponent<AudioSource>();
            }
            return _audioSource;
        }
        
        public void Play()
        {
            if (!SoundFx.PlayOneShot) Stop();

            var audioSource = GetAudioSource();
            
            audioSource.volume = SoundFx.Volume;
            audioSource.loop = SoundFx.Looped;
            audioSource.outputAudioMixerGroup = SoundFx.AudioMixerGroup;
            audioSource.spatialBlend = SoundFx.SpatialBlend;
            audioSource.minDistance = SoundFx.MinDistance;
            audioSource.maxDistance = SoundFx.MaxDistance;

            if (SoundFx.PlayOneShot)
            {
                audioSource.PlayOneShot(SoundFx.GetAudioClip(), SoundFx.Volume);
            }
            else
            {
                audioSource.resource = SoundFx.GetAudioClip();
                audioSource.Play();
            }
        }
        
        public void Play(SoundFxPreset soundFx)
        {
            SoundFx = soundFx;
            
            Play();
        }
        
        public void Stop()
        {
            var audioSource = GetAudioSource();
            
            if (audioSource.isPlaying)
            {
                audioSource.Stop();
            }
        }
        
        private void InitAudio()
        {
            var audioSource = GetAudioSource();
            
            audioSource.playOnAwake = false; 
            if (SoundFx && SoundFx.PlayOnEnable)
            {
                audioSource.playOnAwake = false;
                audioSource.clip = SoundFx.GetAudioClip();
            }
        }
        
        private void Awake()
        {
            InitAudio();
        }

        private void OnEnable()
        {
            if (SoundFx && SoundFx.PlayOnEnable) Play();
        }
    }
}