using EMSP.App;
using EMSP.Mathematic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EMSP.UI.Dialogs.CalculationSettings
{
	public class GeneralPanel : Panel 
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
        public class RangeLengthCalculatedEvent : UnityEvent<GeneralPanel, int> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private InputFilter _inputFilter;
        #endregion

        #region Events
        public RangeLengthCalculatedEvent RangeLengthCalculated = new RangeLengthCalculatedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public InputFilter InputFilter { get { return _inputFilter; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Start()
        {
            _inputFilter.SetRangeLengthText(MathematicManager.Instance.RangeLength);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void InputFilter_RangeLengthCalculated(InputFilter inputFilter, int rangeLength)
        {
            RangeLengthCalculated.Invoke(this, rangeLength);
        }
        #endregion
        #endregion
    }
}