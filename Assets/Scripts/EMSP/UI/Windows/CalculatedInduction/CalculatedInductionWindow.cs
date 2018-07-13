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

        public void DrawCalculated(string wireName, int segmentNumber, Dictionary<string, float> calculated)
        {
            ClearWindow();

            _selectedSegmentNameField.text = string.Format("Провод {0} сегмент {1}", wireName, segmentNumber);

            foreach (var kvp in calculated)
            {
                CreateWireRow(kvp.Key, kvp.Value);
            }
        }

        private void CreateWireRow(string name, float value)
        {
            _wireRowFactory.Create(_wireRowPrefab, _rowsParent, name, value);
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

        #endregion
        #endregion
    }
}
