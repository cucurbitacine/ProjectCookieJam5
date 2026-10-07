using Game.Scripts.Interactions;
using TMPro;
using UnityEngine;

namespace Game.Scripts.UI.Game
{
    public class InteractableObjectHint : MonoBehaviour
    {
        [SerializeField] private InteractState m_updateState = InteractState.Select;
        
        [Header("Interactable Object")]
        [SerializeField] private TMP_Text m_interactName;
        [SerializeField] private TMP_Text m_interactDescription;
        
        private InteractableObject m_interactableObject;

        public InteractableObject GetInteractableObject()
        {
            return m_interactableObject;
        }

        private void OnInteractStateChanged(GameObject actor, InteractState state, bool value)
        {
            if (state != m_updateState) return;
            if (!value) return;

            m_interactName.text = GetInteractableObject().GetInteractName();
            m_interactDescription.text = GetInteractableObject().GetInteractDescription();
        }
        
        private void Awake()
        {
            m_interactableObject = GetComponentInParent<InteractableObject>();
        }

        private void OnEnable()
        {
            if (m_interactableObject)
            {
                m_interactableObject.InteractStateChanged += OnInteractStateChanged;
            }
        }

        private void OnDisable()
        {
            if (m_interactableObject)
            {
                m_interactableObject.InteractStateChanged -= OnInteractStateChanged;
            }
        }
        
        private void Start()
        {
            m_interactName.text = string.Empty;
            m_interactDescription.text = string.Empty;
        }
    }
}