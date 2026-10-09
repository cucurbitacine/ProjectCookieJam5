using System.Collections;
using System.Collections.Generic;
using CucuTools.LevelSystem;
using UnityEngine;

namespace Game.Scripts.Levels.Tutorial
{
    public class TutorialLevelController : LevelController
    {
        [SerializeField] private LevelSceneAsset gameScene;

        [SerializeField] private int m_tutorialStage = 0;
        [SerializeField] private List<GameObject> m_tutorials = new List<GameObject>();
        
        [ContextMenu(nameof(PlayGame))]
        public void PlayGame()
        {
            LevelManager.Instance.LoadSceneAsync(gameScene);
        }

        [ContextMenu(nameof(Next))]
        public void Next()
        {
            m_tutorialStage++;

            if (m_tutorialStage >= 0 && m_tutorialStage < m_tutorials.Count)
            {
                m_tutorials[m_tutorialStage].SetActive(true);
            }
            else
            {
                PlayGame();
            }
        }
        
        public override void BindAll(LevelContext context)
        {
            context.Bind<TutorialLevelController>(this);
        }

        protected override void OnAwake()
        {
            base.OnAwake();
            
            foreach (var tutorial in m_tutorials)
            {
                tutorial.gameObject.SetActive(false);
            }
        }

        protected override IEnumerator StartLevel()
        {
            m_tutorialStage = -1;
            
            Next();
            
            yield break;
        }

        protected override void DestroyLevel()
        {
        }
    }
}