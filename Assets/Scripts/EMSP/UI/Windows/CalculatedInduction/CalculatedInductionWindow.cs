using EMSP.Communication;
using EMSP.Mathematic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EMSP.UI.Windows.CalculatedInduction
{
    public class CalculatedInductionWindow : ModalWindow
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
        private WireRow _wireRowPrefab;

        [SerializeField]
        private Text _selectedSegmentNameField;

        [SerializeField]
        private Button _clearWindow;

        [SerializeField]
        private RectTransform _rowsParent;

        private WireRow.Factory _wireRowFactory = new WireRow.Factory();

        private List<WireRow> _rows = new List<WireRow>();


        private float _clickDuration = 0.35f;
        private bool _hasMouseDown = false;
        private float _timer = -2;
        private PointerEventData _pointerData;
        private Vector3 _previousMouseScreenPosition;

        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties

        #endregion

        #region Constructors
        #endregion

        #region Methods

        public override void ShowModal()
        {
            _pointerData = new PointerEventData(EventSystem.current);
            _previousMouseScreenPosition = Input.mousePosition;

            ClearWindow();
            base.ShowModal();
        }

        public override void Hide()
        {
            _pointerData = null;
            base.Hide();
        }

        public void DrawCalculated(VectorableCalculatedValueInfo calculated, AmperageMode mode, int currentTimeIndex)// ToDo
        {
            ClearWindow();

            _selectedSegmentNameField.text = string.Format("Провод {0}", calculated.Segment.GeneralWire.Name);
            _clearWindow.interactable = true;

            foreach (var kvp in calculated.PrecomputedValue)
            {
                _rows.Add(CreateWireRow(kvp, calculated.CalculatedValueInTime, mode, currentTimeIndex));

            }
        }

        private WireRow CreateWireRow(KeyValuePair<Wire, float> precomputed, VectorableCalculatedValueInTime[] calculated, AmperageMode mode, int currentTimeIndex)
        {
            return _wireRowFactory.Create(_wireRowPrefab, _rowsParent, precomputed, calculated, mode, currentTimeIndex);
        }

        public void ClearWindow()
        {
            _selectedSegmentNameField.text = string.Empty;

            for (int aliveRowIndex = 0; aliveRowIndex < _rowsParent.childCount; ++aliveRowIndex)
            {
                Destroy(_rowsParent.GetChild(aliveRowIndex).gameObject);
            }

            _rowsParent.DetachChildren();
        }

        private void Update()
        {
            if (IsShowing)
            {
                _pointerData.position = Input.mousePosition;
                _pointerData.delta = Input.mousePosition - _previousMouseScreenPosition;
                _previousMouseScreenPosition = Input.mousePosition;

                if (_timer > -1) _timer -= Time.deltaTime;

                if (_hasMouseDown)
                {
                    if (Input.GetMouseButtonUp(0))
                    {
                        if (_timer >= 0)
                        {
                            var results = new List<RaycastResult>();
                            EventSystem.current.RaycastAll(_pointerData, results);

                            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                            RaycastHit hit;
                            if (!Physics.Raycast(ray, out hit))
                            {
                                if (results.Count == 0 && _pointerData.delta.magnitude < 1)
                                {
                                    _clearWindow.onClick.Invoke();
                                    _clearWindow.interactable = false;
                                }
                            }
                        }

                        _hasMouseDown = false;
                    }
                }
                else
                {
                    if (Input.GetMouseButtonDown(0))
                    {
                        _hasMouseDown = true;
                        _timer = _clickDuration;
                    }
                }
            }
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void OnTimeStemChanged(int timeIndex)
        {
            foreach (var row in _rows)
            {
                row.SetTimeStep(timeIndex);
            }
        }

        public void OnAmperageModeChanged(AmperageMode mode)
        {
            foreach (var row in _rows)
            {
                row.SetAmperageMode(mode);
            }
        }
        #endregion
        #endregion
    }
}
