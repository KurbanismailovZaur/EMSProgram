using EMSP.Communication;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Dialogs.WiringEditor
{
    public class WiringEditorDialog : MonoSingleton<WiringEditorDialog>
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
        private Button _wireButtonPrefab;
        [SerializeField]
        private PointEditPanel _pointEditPanelPrefab;

        [SerializeField]
        private Button _saveButton;
        [SerializeField]
        private Button _cancelButton;
        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private RectTransform _wireButtonContainer;
        [SerializeField]
        private RectTransform _pointsContainer;

        protected Dictionary<int, List<Vector3>> _wiring = new Dictionary<int, List<Vector3>>();
        private CanvasGroup _canvasGroup;

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
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void StartWiringEditing(Wiring wiring, Action<Dictionary<int, List<Vector3>>> onWiringEdited) 
        {
            _saveButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            Show();

            int wireCount = 0;
            foreach(Wire wire in wiring)
            {
                _wiring.Add(wireCount, new List<Vector3>());
                foreach (Vector3 point in wire)
                {
                    _wiring[wireCount].Add(point);
                }

                Button wireButton = Instantiate(_wireButtonPrefab);
                wireButton.transform.parent = _wireButtonContainer;
                wireButton.GetComponentInChildren<Text>().text = string.Format("Wire_{0}", wireCount);

                wireButton.onClick.AddListener(() =>
                {
                    for(int i = 0; i < _pointsContainer.childCount; ++i)
                    {
                        Destroy(_pointsContainer.GetChild(i).gameObject);
                    }

                    int pointCount = 0;
                    foreach (var point in wire)
                    {
                        PointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                        editPanel.transform.parent = _pointsContainer;

                        editPanel.Initialize(wireCount, pointCount);
                        ++pointCount;
                    }
                });

                ++wireCount;
            }

            _saveButton.onClick.AddListener(() =>
            {
                SaveAndClose(onWiringEdited);
            });

            _cancelButton.onClick.AddListener(() =>
            {
                Close();
            });

            _closeButton.onClick.AddListener(() =>
            {
                Close();
            });
        }

        private void Show()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        private void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        private void Close(bool needClearChangedWiring = true)
        {
            for (int i = 0; i < _pointsContainer.childCount; ++i)
            {
                Destroy(_pointsContainer.GetChild(i).gameObject);
            }

            for (int i = 0; i < _wireButtonContainer.childCount; ++i)
            {
                Destroy(_wireButtonContainer.GetChild(i).gameObject);
            }

            if (needClearChangedWiring)
                _wiring.Clear();

            Hide();
        }

        private void SaveAndClose(Action<Dictionary<int, List<Vector3>>> onWiringEdited)
        {
            Close(false);

            if (onWiringEdited != null)
                onWiringEdited(_wiring);

            _wiring.Clear();
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion

    }
}
