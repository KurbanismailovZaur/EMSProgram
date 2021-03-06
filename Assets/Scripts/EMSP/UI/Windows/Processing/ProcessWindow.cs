﻿using EMSP.Processing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Windows.Processing
{
    public class ProcessWindow : ModalWindow
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class ProcessPanelCreatedEvent : UnityEvent<ProcessPanel> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private ProcessPanel _processPanelPrefab;

        [SerializeField]
        private GameObject _deviderPrefab;

        private ProcessPanel.Factory _processPanelFactory = new ProcessPanel.Factory();

        [SerializeField]
        private RectTransform _content;

        private List<ProcessPanel> _processPanels = new List<ProcessPanel>();
        #endregion

        #region Events
        public ProcessPanelCreatedEvent ProcessPanelCreated = new ProcessPanelCreatedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public List<ProcessPanel> ProcessPanels
        {
            get { return _processPanels; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void CreateProcessPanel(IProcessable processable)
        {
            ProcessPanel processPanel = _processPanelFactory.Create(_processPanelPrefab, _content, processable);
            processPanel.Completed.AddListener(ProcessPanel_Completed);

            _processPanels.Add(processPanel);

            ProcessPanelCreated.Invoke(processPanel);
        }

        private void RemoveProcessPanel(ProcessPanel processPanel)
        {
            if (_processPanels.Remove(processPanel))
            {
                Destroy(processPanel.gameObject);
            }
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void ProcessManager_ProcessCreated(ProcessManager processManager, IProcessable processable)
        {
            CreateProcessPanel(processable);
        }

        private void ProcessPanel_Completed(ProcessPanel processPanel)
        {
            RemoveProcessPanel(processPanel);
        }
        #endregion
        #endregion
    }
}
