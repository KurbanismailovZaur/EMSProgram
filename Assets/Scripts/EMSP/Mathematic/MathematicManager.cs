using EMSP.Communication;
using EMSP.Mathematic.Electric;
using EMSP.Mathematic.Magnetic;
using EMSP.Mathematic.Induction;
using EMSP.Timing;
using EMSP.Utility.Extensions;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Mathematic
{
    public class MathematicManager : MonoSingleton<MathematicManager>
    {
        #region Entities
        #region Enums
        private enum InnerCalculationsType
        {
            None = -1,
            MagneticTension,
            ElectricField,
            Induction
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class RangeLengthChangedEvent : UnityEvent<MathematicManager, int> { }

        [Serializable]
        public class AmperageModeChangedEvent : UnityEvent<MathematicManager, AmperageMode> { }

        [Serializable]
        public class CalculationShowedEvent : UnityEvent<MathematicManager, ICalculationMethod> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        [Range(8, 128)]
        private int _rangeLength = 16;

        [SerializeField]
        private AmperageMode _amperageMode;

        [Header("Mathematics")]
        [SerializeField]
        private MagneticTension _magneticTension;

        [SerializeField]
        private ElectricField _electricField;

        [SerializeField]
        private Induction.Induction _induction;

        private ICalculationMethod _currentCalculationMethod;
        #endregion

        #region Events
        [Space(4)]
        public RangeLengthChangedEvent RangeLengthChanged = new RangeLengthChangedEvent();

        public AmperageModeChangedEvent AmperageModeChanged = new AmperageModeChangedEvent();

        public CalculationShowedEvent CalculationShowed = new CalculationShowedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public int RangeLength
        {
            get { return _rangeLength; }
            set
            {
                if (_rangeLength == value)
                {
                    return;
                }

                _rangeLength = value;

                RangeLengthChanged.Invoke(this, _rangeLength);
            }
        }

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
                if (_currentCalculationMethod != null) _currentCalculationMethod.AmperageMode = _amperageMode;

                AmperageModeChanged.Invoke(this, _amperageMode);
            }
        }

        public MagneticTension MagneticTension { get { return _magneticTension; } }

        public ElectricField ElectricField { get { return _electricField; } }

        public Induction.Induction Induction { get { return _induction; } }

        private InnerCalculationsType CurrentCalculationsType
        {
            get
            {
                if (_currentCalculationMethod == null) return InnerCalculationsType.None;

                return (InnerCalculationsType)_currentCalculationMethod.Type;
            }
        }

        public ICalculationMethod CurrentCalculationMethod { get { return _currentCalculationMethod; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void Calculate(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.MagneticTension:
                    _magneticTension.Calculate(RangeLength, WiringManager.Instance.Wiring, TimeManager.Instance.Steps);
                    break;
                case CalculationType.ElectricField:
                    _electricField.Calculate(RangeLength, WiringManager.Instance.Wiring, TimeManager.Instance.Steps);
                    break;
                case CalculationType.Induction:
                    _induction.Calculate(WiringManager.Instance.Wiring);
                    break;
            }
        }

        public void Show(CalculationType calculationType)
        {
            if ((int)calculationType == (int)CurrentCalculationsType) return;

            HideCurrentCalculations();

            switch (calculationType)
            {
                case CalculationType.MagneticTension:
                    (_currentCalculationMethod = _magneticTension).IsVisible = true;
                    break;
                case CalculationType.ElectricField:
                    (_currentCalculationMethod = _electricField).IsVisible = true;
                    break;
                case CalculationType.Induction:
                    (_currentCalculationMethod = _induction).IsVisible = true;
                    break;
            }

            _currentCalculationMethod.AmperageMode = _amperageMode;
            _currentCalculationMethod.SetEntitiesToTime(TimeManager.Instance.TimeIndex);

            CalculationShowed.Invoke(this, _currentCalculationMethod);
        }

        public void ShowFirstExistCalculation()
        {
            InnerCalculationsType innerCalculationType = _magneticTension.IsCalculated ? InnerCalculationsType.MagneticTension : _electricField.IsCalculated ? InnerCalculationsType.ElectricField : InnerCalculationsType.None;

            if (innerCalculationType == InnerCalculationsType.None) return;

            Show((CalculationType)innerCalculationType);
        }

        public void SwitchVisibility(CalculationType calculationType)
        {
            InnerCalculationsType currentCalculationType = CurrentCalculationsType;

            HideCurrentCalculations();

            if ((int)currentCalculationType == (int)calculationType) return;

            Show(calculationType);
        }

        public void HideCurrentCalculations()
        {
            if (_currentCalculationMethod != null)
            {
                _currentCalculationMethod.IsVisible = false;
                _currentCalculationMethod = null;
            }
        }

        private void HideCalculations(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.MagneticTension:
                    _magneticTension.IsVisible = false;
                    break;
                case CalculationType.ElectricField:
                    _electricField.IsVisible = false;
                    break;
                case CalculationType.Induction:
                    _induction.IsVisible = false;
                    break;
            }

            if ((int)CurrentCalculationsType == (int)calculationType) _currentCalculationMethod = null;
        }

        public void DestroyCalculations(CalculationType calculationType)
        {
            HideCalculations(calculationType);

            if ((int)CurrentCalculationsType == (int)calculationType) _currentCalculationMethod = null;

            switch (calculationType)
            {
                case CalculationType.MagneticTension:
                    _magneticTension.DestroyCalculatedPoints();
                    break;
                case CalculationType.ElectricField:
                    _electricField.DestroyCalculatedPoints();
                    break;
                case CalculationType.Induction:
                    _induction.DestroyCalculated();
                    break;
            }
        }

        public void DestroyAllCalculations()
        {
            _magneticTension.DestroyCalculatedPoints();
            _electricField.DestroyCalculatedPoints();
            _induction.DestroyCalculated();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}