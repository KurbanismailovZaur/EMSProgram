using EMSP.Communication;
using EMSP.Mathematic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
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
        private RectTransform _rowsParent;

        private WireRow.Factory _wireRowFactory = new WireRow.Factory();

        private List<WireRow> _rows = new List<WireRow>(); 
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
            ClearWindow();
            base.ShowModal();
        }

        public void DrawCalculated(VectorableCalculatedValueInfo calculated, AmperageMode mode, int currentTimeIndex)// ToDo
        {
            ClearWindow();

            _selectedSegmentNameField.text = string.Format("Провод {0} сегмент {1}", calculated.Segment.GeneralWire.Name, calculated.Segment.ID);

            foreach(var kvp in calculated.PrecomputedValue)
            {
                _rows.Add(CreateWireRow(kvp, calculated.CalculatedValueInTime, mode, currentTimeIndex));

            }
        }

        private WireRow CreateWireRow(KeyValuePair<Wire, float> precomputed, VectorableCalculatedValueInTime[] calculated, AmperageMode mode, int currentTimeIndex)
        {
            return _wireRowFactory.Create(_wireRowPrefab, _rowsParent, precomputed, calculated, mode, currentTimeIndex);
        }

        private void ClearWindow()
        {
            _selectedSegmentNameField.text = string.Empty;

            for (int aliveRowIndex = 0; aliveRowIndex < _rowsParent.childCount; ++aliveRowIndex)
            {
                Destroy(_rowsParent.GetChild(aliveRowIndex).gameObject);
            }

            _rowsParent.DetachChildren();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void OnTimeStemChanged(int timeIndex)
        {
            foreach(var row in _rows)
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
