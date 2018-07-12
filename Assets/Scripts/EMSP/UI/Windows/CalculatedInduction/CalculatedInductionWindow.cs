using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

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
        public void DrawCalculated(Dictionary<string, float> calculated)
        {
            for(int aliveRowIndex = 0; aliveRowIndex < _rowsParent.childCount; ++aliveRowIndex)
            {
                Destroy(_rowsParent.GetChild(aliveRowIndex).gameObject);
            }

            _rowsParent.DetachChildren();

            foreach (var kvp in calculated)
            {
                CreateWireRow(kvp.Key, kvp.Value);
            }
        }

        private void CreateWireRow(string name, float value)
        {
            _wireRowFactory.Create(_wireRowPrefab, _rowsParent, name, value);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers

        #endregion
        #endregion
    }
}
