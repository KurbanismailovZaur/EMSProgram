using EMSP.Processing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void CreateProcessPanel(IProcessable processable)
        {
            ProcessPanel processPanel = _processPanelFactory.Create(_processPanelPrefab, _content, processable);
            processPanel.Completed.AddListener(ProcessPanel_Completed);

            _processPanels.Add(processPanel);
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
