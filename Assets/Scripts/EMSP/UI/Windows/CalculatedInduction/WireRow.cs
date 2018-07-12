using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EMSP.UI.Windows.CalculatedInduction
{
    public class WireRow : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes

        public class Factory
        {
            public WireRow Create(WireRow processPanelPrefab, RectTransform parent, string name, float value)
            {
                WireRow wireRow = Instantiate(processPanelPrefab, parent, false);

                wireRow._nameField.text = name;
                wireRow._valueField.text = value.ToString();

                return wireRow;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private Text _nameField;
        [SerializeField]
        private Text _valueField;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties

        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion

        #region Indexers
        #endregion

        #region Events handlers

        #endregion
        #endregion
    }
}
