using System.Collections;
using CucuTools.LevelSystem;
using Game.Scripts.Levels;
using Game.Scripts.Levels.Menu;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

namespace Game.Scripts.UI.Menu
{
    public class MenuUIController : LevelComponent
    {
        [SerializeField] private Button playGameButton;
        [SerializeField] private InputActionReference m_playAction;

        private MenuLevelController m_menuLevel;

        public override int Order => StartOrder.UI;
        
        private void OnPlayInput(InputAction.CallbackContext context)
        {
            if (context.ReadValueAsButton())
            {
                OnPlayGameButtonClick();
            }
        }
        
        public override void ResolveAll(LevelContext context)
        {
            m_menuLevel = context.Resolve<MenuLevelController>();
        }

        public override IEnumerator OnStartLevel()
        {
            playGameButton.onClick.AddListener(OnPlayGameButtonClick);

            m_playAction.action.started += OnPlayInput;
            
            yield break;
        }
        
        public override void OnDestroyLevel()
        {
            playGameButton.onClick.AddListener(OnPlayGameButtonClick);
            
            m_playAction.action.started -= OnPlayInput;
        }
        
        private void OnPlayGameButtonClick()
        {
            m_menuLevel.PlayGame();
        }
    }
}
