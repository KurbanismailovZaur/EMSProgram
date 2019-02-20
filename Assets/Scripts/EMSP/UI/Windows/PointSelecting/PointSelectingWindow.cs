using EMSP.Communication;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Windows.PointSelecting
{
    public class PointSelectingWindow : ModalWindow
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
        private Button _addPointButtonPrefab;

        [SerializeField]
        private PSPointEditPanel _pointEditPanelPrefab;

        [SerializeField]
        private Button _calculateButton;

        [SerializeField]
        private Button _cancelButton;

        [SerializeField]
        private Button _closeButton;

        [SerializeField]
        private RectTransform _pointsContainer;

        #endregion

        #region Events
        public event Action OnDialogActivated = () => { };

        public event Action OnDialogDeactivated = () => { };
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        [ContextMenu("startSelecting")]
        public void StartSelectingPoint()
        {
            _calculateButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            ShowModal();

            Button addPointButton = Instantiate(_addPointButtonPrefab);
            addPointButton.transform.SetParent(_pointsContainer, false);
            addPointButton.onClick.AddListener(() =>
            {
                PSPointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                editPanel.transform.SetParent(_pointsContainer, false);

                editPanel.Initialize(this);
                editPanel.DeletePointButton.onClick.AddListener(() =>
                {
                    Vector3 _point = editPanel.CurrentValue;

                    StartCoroutine(WaitAndUpdatePointsNumber());
                    Destroy(editPanel.gameObject);
                });
                addPointButton.transform.SetAsLastSibling();

                StartCoroutine(WaitAndMovePointsContainer(editPanel.GetComponent<RectTransform>().rect.height));
            });


            _calculateButton.onClick.AddListener(() =>
            {
                Calculate();
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

        private IEnumerator WaitAndMovePointsContainer(float offset)
        {
            yield return null;

            var pcParentHeight = _pointsContainer.parent.GetComponent<RectTransform>().rect.height;
            var pcHeight = _pointsContainer.rect.height;

            if (pcHeight > pcParentHeight)
            {
                _pointsContainer.anchoredPosition3D = new Vector3(
                       0,
                       _pointsContainer.anchoredPosition3D.y + offset + 5,
                       0);
            }
        }

        private IEnumerator WaitAndUpdatePointsNumber()
        {
            yield return null;

            foreach (var editPointPanel in _pointsContainer.GetComponentsInChildren<PSPointEditPanel>())
            {
                editPointPanel.UpdatePointNumberAndSelectableImmediate();
            }
        }

        public override void ShowModal()
        {
            base.ShowModal();


            OnDialogActivated();
        }

        public override void Hide()
        {
            base.Hide();
        }

        private void Close()
        {
            for (int i = 0; i < _pointsContainer.childCount; ++i)
            {
                Destroy(_pointsContainer.GetChild(i).gameObject);
            }

            OnDialogDeactivated();

            OnDialogActivated = () => { };
            OnDialogDeactivated = () => { };

            Hide();
        }

        private void Calculate()
        {

            List<Vector3> _points = new List<Vector3>();
            foreach (var editPointPanel in _pointsContainer.GetComponentsInChildren<PSPointEditPanel>())
            {
                _points.Add(editPointPanel.CurrentValue);
            }

            Close();
            App.GameFacade.Instance.CalculateElectricFieldByConeretePoints(_points);
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
