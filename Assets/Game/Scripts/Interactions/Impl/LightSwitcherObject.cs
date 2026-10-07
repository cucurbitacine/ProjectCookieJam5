using System;
using UnityEngine;
using UnityEngine.U2D;

namespace Game.Scripts.Interactions.Impl
{
    public class LightSwitcherObject : InteractableObject
    {
        [Header("Switcher")]
        [SerializeField] private bool m_stateOnStart = true;
        [SerializeField] private Light2DBase m_light;

        public bool HasLightFlag()
        {
            return m_light ? m_light.enabled : false;
        }

        public void SetLightFlag(bool value)
        {
            if (m_light)
            {
                m_light.enabled = value;
            }
        }

        public void SwitchLight()
        {
            SetLightFlag(!HasLightFlag());
        }

        protected override void OnInteract(GameObject actor, bool value)
        {
            if (value) SwitchLight();
        }

        private void Start()
        {
            SetLightFlag(m_stateOnStart);
        }
    }
}