using System.Collections;
using CucuTools.LevelSystem;
using Game.Scripts.Levels.Menu;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Menu
{
    public class MenuUIController : LevelComponent
    {
        [SerializeField] private Button playGameButton;

        private MenuLevelController m_menuLevel;

        public override int Order => StartOrder.UI;

        public override void ResolveAll(LevelContext context)
        {
            m_menuLevel = context.Resolve<MenuLevelController>();
        }

        public override IEnumerator OnStartLevel()
        {
            playGameButton.onClick.AddListener(OnPlayGameButtonClick);
            
            yield break;
        }

        public override void OnDestroyLevel()
        {
            playGameButton.onClick.AddListener(OnPlayGameButtonClick);
        }
        
        private void OnPlayGameButtonClick()
        {
            m_menuLevel.PlayGame();
        }
    }
}
