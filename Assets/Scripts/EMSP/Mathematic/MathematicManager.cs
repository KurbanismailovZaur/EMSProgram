using EMSP.Communication;
using EMSP.Mathematic.MagneticTension;
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
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #region Common
        [SerializeField]
        [Range(8, 128)]
        private int _rangeLength = 16;

        [SerializeField]
        private AmperageMode _amperageMode;

        [Header("Mathematics")]
        [SerializeField]
        private MagneticTensionInSpace _magneticTensionInSpace;
        #endregion
        #endregion

        #region Events
        [Space(4)]
        public RangeLengthChangedEvent RangeLengthChanged = new RangeLengthChangedEvent();

        public AmperageModeChangedEvent AmperageModeChanged = new AmperageModeChangedEvent();
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
                _magneticTensionInSpace.AmperageMode = _amperageMode;

                AmperageModeChanged.Invoke(this, _amperageMode);
            }
        }

        public MagneticTensionInSpace MagneticTensionInSpace { get { return _magneticTensionInSpace; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void Calculate(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.MagneticTensionInSpace:
                    _magneticTensionInSpace.Calculate(RangeLength, WiringManager.Instance.Wiring, TimeManager.Instance.Steps);
                    break;
            }
        }

        public void Show(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.MagneticTensionInSpace:
                    _magneticTensionInSpace.IsVisible = true;
                    break;
            }
        }

        public void Hide(CalculationType calculationType)
        {
            switch (calculationType)
            {
                case CalculationType.MagneticTensionInSpace:
                    _magneticTensionInSpace.IsVisible = false;
                    break;
            }
        }

        public void DestroyCalculations()
        {
            MagneticTensionInSpace.DestroyMagneticTensions();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}