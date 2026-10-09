using System;
using Game.Scripts.Interactions.Impl;
using Game.Scripts.Levels.Game;
using UnityEngine;

namespace Game.Scripts.UI.Game
{
    public class TaskListUI : MonoBehaviour
    {
        [SerializeField] private GameObject m_sinkTask;
        [SerializeField] private GameObject m_vacuumTask;
        [SerializeField] private GameObject m_closetTask;
        [SerializeField] private GameObject m_washingTask;

        private TaskListController m_taskList;
        
        public TaskListController GetTaskList()
        {
            return m_taskList;
        }

        private void Awake()
        {
            m_taskList = FindAnyObjectByType<TaskListController>();
        }

        private void Update()
        {
            m_sinkTask.SetActive(!GetTaskList().IsSinkDone());
            m_vacuumTask.SetActive(!GetTaskList().IsVacuumDone());
            m_closetTask.SetActive(!GetTaskList().IsClosetDone());
            m_washingTask.SetActive(!GetTaskList().IsWashingDone());
        }
    }
}
