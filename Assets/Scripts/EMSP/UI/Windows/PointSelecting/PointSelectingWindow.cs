using EMSP.Communication;
using EMSP.UI.Toggle;
using NPOI.HSSF.UserModel;
using NPOI.SS.UserModel;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
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
        private Button _importButton;

        [SerializeField]
        private RectTransform _pointsContainer;

        [SerializeField]
        private ToggleBehaviour _toggleAutogeneration;

        [SerializeField]
        private ToggleBehaviour _toggleByHand;


        private Button _addPointButton;

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
            _importButton.onClick.RemoveAllListeners();
            _cancelButton.onClick.RemoveAllListeners();
            _closeButton.onClick.RemoveAllListeners();
            ShowModal();

            _addPointButton = Instantiate(_addPointButtonPrefab);
            _addPointButton.transform.SetParent(_pointsContainer, false);
            _addPointButton.onClick.AddListener(() =>
            {
                PSPointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                editPanel.transform.SetParent(_pointsContainer, false);

                editPanel.Initialize(this, Vector3.zero);
                editPanel.DeletePointButton.onClick.AddListener(() =>
                {
                    Vector3 _point = editPanel.CurrentValue;

                    StartCoroutine(WaitAndUpdatePointsNumber());
                    Destroy(editPanel.gameObject);
                });
                _addPointButton.transform.SetAsLastSibling();

                StartCoroutine(WaitAndMovePointsContainer(editPanel.GetComponent<RectTransform>().rect.height));
            });


            _calculateButton.onClick.AddListener(() =>
            {
                Calculate();
            });

            _importButton.onClick.AddListener(() =>
            {
                Import();
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
            if (_toggleAutogeneration.State == true)
            {
                Close();
                App.GameFacade.Instance.Calculate(Mathematic.CalculationType.ElectricField);
            }
            else if (_toggleByHand.State == true)
            {

                List<Vector3> _points = new List<Vector3>();
                foreach (var editPointPanel in _pointsContainer.GetComponentsInChildren<PSPointEditPanel>())
                {
                    _points.Add(editPointPanel.CurrentValue);
                }

                Close();
                App.GameFacade.Instance.CalculateElectricFieldByConeretePoints(_points);
            }
            else
                throw new Exception();

        }

        private void Import()
        {
            string[] results = SFB.StandaloneFileBrowser.OpenFilePanel("Открыть Проводку", Application.dataPath, "xls", false);

            if (results.Length == 0)
            {
                return;
            }

            List<Vector3> _points = new List<Vector3>();
            HSSFWorkbook workbook;

            using (FileStream stream = new FileStream(results[0], FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            {
                workbook = new HSSFWorkbook(stream);
            }

            ISheet sheet = workbook.GetSheetAt(0);

            for (int i = 2; i <= sheet.LastRowNum; i++)
            {
                IRow row = sheet.GetRow(i);

                if (!IsCorrectNodeRow(row))
                {
                    break;
                }

                _points.Add(ReadNode(row));
            }


            foreach(var p in _points)
            {
                PSPointEditPanel editPanel = Instantiate(_pointEditPanelPrefab);
                editPanel.transform.SetParent(_pointsContainer, false);

                editPanel.Initialize(this, p);
                editPanel.DeletePointButton.onClick.AddListener(() =>
                {
                    Vector3 _point = editPanel.CurrentValue;

                    StartCoroutine(WaitAndUpdatePointsNumber());
                    Destroy(editPanel.gameObject);
                });
                _addPointButton.transform.SetAsLastSibling();

                StartCoroutine(WaitAndMovePointsContainer(editPanel.GetComponent<RectTransform>().rect.height));
            }
        }

        private Vector3 ReadNode(IRow row)
        {
            Vector3 node;

            node.x = (float)row.GetCell(1).NumericCellValue;
            node.y = (float)row.GetCell(2).NumericCellValue;
            node.z = (float)row.GetCell(3).NumericCellValue;

            return node;
        }

        private bool IsCorrectNodeRow(IRow row)
        {
            for (int i = 1; i < 4; i++)
            {
                ICell cell = row.GetCell(i);

                if (cell.CellType != CellType.Numeric)
                {
                    return false;
                }
            }

            return true;
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
