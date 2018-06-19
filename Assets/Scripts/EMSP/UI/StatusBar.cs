using EMSP.UI.Windows.Processing;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI
{
    public class StatusBar : MonoBehaviour
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
        [SerializeField] private ProcessWindow _processWindow;
        [SerializeField] private Text _processStatusField;
        [SerializeField] private RectTransform _progressImageTransform;

        private RectTransform _rectTransform;
        private bool _hasAliveProcess = false;
        private int _processesCount = 0;
        private bool _lastProcessHandling = false;
        private int _comletedProcessesCount = 0;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties

        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Start()
        {
            _rectTransform = GetComponent<RectTransform>();
            _processWindow.ProcessPanelCreated.AddListener(ProcessPanelCreatedHandler);
        }


        private void Update()
        {
            _hasAliveProcess = _processesCount > 0;
            if (!_hasAliveProcess) return;

            if (_processesCount == 1)
            {
                if (!_lastProcessHandling && _comletedProcessesCount > 0)
                {
                    _lastProcessHandling = true;
                }

                if (!_lastProcessHandling)
                {
                    _processStatusField.text = _processWindow.ProcessPanels[0].ProgressName;
                    _progressImageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _rectTransform.rect.width * _processWindow.ProcessPanels[0].Progress);

                    return;
                }
            }

            _processStatusField.text = string.Format("Осталось задач: {0} из {1}", _processesCount, _processesCount + _comletedProcessesCount);

            float allProgress = 0;
            for (int i = 0; i < _processesCount; ++i)
            {
                allProgress += _processWindow.ProcessPanels[i].Progress;
            }
            allProgress += _comletedProcessesCount;

            _progressImageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _rectTransform.rect.width * (allProgress / (_processesCount + _comletedProcessesCount)));
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        private void ProcessCompletedHandler(ProcessPanel processPanel)
        {
            if (_lastProcessHandling || _processesCount == 1)
            {
                _processStatusField.text = string.Empty;
                _progressImageTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, 0);

                _comletedProcessesCount = 0;
                _lastProcessHandling = false;
            }
            else
            {
                ++_comletedProcessesCount;
            }

            --_processesCount;
        }

        private void ProcessPanelCreatedHandler(ProcessPanel processPanel)
        {
            processPanel.Completed.AddListener(ProcessCompletedHandler);
            ++_processesCount;
        }

        #endregion
        #endregion
    }
}
