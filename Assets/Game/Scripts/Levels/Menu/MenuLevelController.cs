using System.Collections;
using CucuTools.LevelSystem;
using UnityEngine;

namespace Game.Scripts.Levels.Menu
{
    public class MenuLevelController : LevelController
    {
        [SerializeField] private LevelSceneAsset gameScene;
        
        [ContextMenu(nameof(PlayGame))]
        public void PlayGame()
        {
            LevelManager.Instance.LoadSceneAsync(gameScene);
        }

        public override void BindAll(LevelContext context)
        {
            context.Bind<MenuLevelController>(this);
        }

        protected override IEnumerator StartLevel()
        {
            yield break;
        }

        protected override void DestroyLevel()
        {
        }
    }
}
