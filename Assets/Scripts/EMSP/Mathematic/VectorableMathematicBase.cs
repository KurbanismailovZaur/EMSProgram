using EMSP.Communication;
using EMSP.UI.Windows;
using EMSP.UI.Windows.CalculatedInduction;
using EMSP.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Mathematic
{
    public abstract class VectorableMathematicBase : MathematicBase, IVectorableCalculationMethod
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
        private ModalWindow _calculatedWindow;

        private AmperageMode _amperageMode;

        private bool _isCalculated;

        private MaxCalculatedValues _maxCalculatedValues;

        private Dictionary<string, Dictionary<string, float>> _calculated = new Dictionary<string, Dictionary<string, float>>();

        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public abstract CalculationType Type { get; }

        public AmperageMode AmperageMode
        {
            get { return _amperageMode; }
            set
            {
                if (_amperageMode == value)
                {
                    return;
                }

                _amperageMode = value;
            }
        }

        public float CurrentModeMaxCalculatedValue { get { return _amperageMode == AmperageMode.Computational ? _maxCalculatedValues.Calculated : _maxCalculatedValues.Precomputed; } }

        public bool IsVisible
        {
            get { return _isCalculated && _calculatedWindow.IsShowing; }
            set
            {
                if (_calculatedWindow.IsShowing == value || !_isCalculated)
                {
                    return;
                }

                if (value)
                    _calculatedWindow.ShowModal();
                else
                    _calculatedWindow.Hide();

                VisibilityChanged.Invoke(this, gameObject.activeSelf);
            }
        }

        public bool IsCalculated { get { return _isCalculated; } }

        #endregion

        #region Constructors
        #endregion

        #region Methods

        public void SetEntitiesToTime(int timeIndex)
        {

        }

        public void Calculate(Wiring wiring)
        {
            foreach (var wire in wiring)
            {
                int segmentCount = wire.LocalPoints.Count - 1;

                for (int segmentIndex = 0; segmentIndex < segmentCount; ++segmentIndex)
                {
                    string key = string.Format("{0}_{1}", wire.Name, segmentIndex);

                    Data calculatedData = Calculator.Calculate(new Data()
                    {
                        { "name", wire.Name },
                        { "segment", segmentIndex },
                        { "wiring", wiring },
                    });

                    _calculated.Add(key, calculatedData.GetValue<Dictionary<string, float>>("result"));
                }
            }

            _isCalculated = true;

            Calculated.Invoke(this);
        }

        public void ShowCalculatedFor(string wireName, int segmentNumber)
        {
            if (!IsCalculated) return;

            string key = string.Format("{0}_{1}", wireName, segmentNumber);

            switch(Type)
            {
                case CalculationType.Induction:
                    ((CalculatedInductionWindow)_calculatedWindow).DrawCalculated(_calculated[key]);
                    break;
                default:
                    Debug.LogError("Unexpeted CalculationType = " + Type.ToString());
                    break;
            }
        }

        public void DestroyCalculated()
        {
            if (!_isCalculated) return;

            _calculated.Clear();

            IsVisible = false;
            _isCalculated = false;

            Destroyed.Invoke(this);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}