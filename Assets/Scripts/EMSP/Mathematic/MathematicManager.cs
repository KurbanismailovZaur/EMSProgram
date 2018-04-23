using EMSP.Communication;
using EMSP.Mathematic.MagneticTension;
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
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #region Common
        [SerializeField]
        [Range(8, 128)]
        private int _rangeLength = 16;

        [Header("Mathematics")]
        [SerializeField]
        private MagneticTensionInSpace _magneticTensionInSpace;
        #endregion

        #endregion

        #region Events
        public RangeLengthChangedEvent RangeLengthChanged = new RangeLengthChangedEvent();
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
                    _magneticTensionInSpace.Calculate();
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