using System.Collections;
using CucuTools.LevelSystem;
using Game.Scripts.Levels.Game;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.UI.Game
{
    public class GameUIController : LevelComponent
    {
        [SerializeField] private Button returnMenuButton;
        
        private GameLevelController m_gameLevel;
        
        private void OnReturnMenuButtonClick()
        {
            m_gameLevel.ReturnMenu();
        }
        
        public override void ResolveAll(LevelContext context)
        {
            m_gameLevel = context.Resolve<GameLevelController>();
        }

        public override IEnumerator OnStartLevel()
        {
            returnMenuButton.onClick.AddListener(OnReturnMenuButtonClick);
            
            yield break;
        }

        public override void OnDestroyLevel()
        {
        }
    }
}