using EMSP.Processing;
using Numba.Threading;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EMSP.UI.Windows.Processing
{
    public class ProcessPanel : MonoBehaviour
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
        public class CompletedEvent : UnityEvent<ProcessPanel> { }

        public class Factory
        {
            public ProcessPanel Create(ProcessPanel processPanelPrefab, RectTransform parent, IProcessable processable)
            {
                ProcessPanel processPanel = Instantiate(processPanelPrefab, parent, false);
                processPanel._processable = processable;

                processPanel._processText.text = "Подготовка к старту";

                processable.ProgressChanged += processPanel.Processable_ProgressChanged;
                processable.ProgressNameChanged += processPanel.Processable_ProgressNameChanged;
                processable.ProgressCanceled += processPanel.Processable_ProgressCanceled;

                return processPanel;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private Text _processText;

        [SerializeField]
        private RectTransform _progressImage;

        private IProcessable _processable;
        private bool _completed = false;
        #endregion

        #region Events
        public CompletedEvent Completed = new CompletedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public float Progress { get { return _processable.Progress; } }
        public string ProgressName { get { return _processable.ProgressName; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        private void Processable_ProgressChanged(IProcessable processable, float progress)
        {
            ThreadDispatcher.Instance.InvokeFromMainThread(() =>
            {
                _progressImage.anchorMax = new Vector2(progress, 1f);

                if (progress >= 1f && !_completed)
                {
                    _completed = true;
                    Completed.Invoke(this);
                }
            });
        }

        private void Processable_ProgressNameChanged(IProcessable processable, string progressName)
        {
            ThreadDispatcher.Instance.InvokeFromMainThread(() =>
            {
                _processText.text = progressName;
            });
        }

        private void Processable_ProgressCanceled(IProcessable processable)
        {
            ThreadDispatcher.Instance.InvokeFromMainThread(() =>
            {
                if (!_completed)
                {
                    _completed = true;
                    Completed.Invoke(this);
                }
            });
        }

        public void CancelProcessButton_OnClick()
        {
            _processable.Cancel();
        }
        #endregion
        #endregion
    }
}
