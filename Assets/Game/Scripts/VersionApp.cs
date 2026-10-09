using System;
using UnityEngine;
using UnityEngine.Events;

namespace Game.Scripts
{
    public class VersionApp : MonoBehaviour
    {
        [SerializeField] private UnityEvent<string> onVersionChanged = new UnityEvent<string>();

        public string GetVersion()
        {
            return $"{Application.platform}_{Application.unityVersion}_{Application.version}";
        }
        
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        private void Start()
        {
            onVersionChanged.Invoke(GetVersion());
        }
    }
}
