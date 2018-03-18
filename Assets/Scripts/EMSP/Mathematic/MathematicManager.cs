using EMSP.Communication;
using EMSP.Mathematic.MagneticTension;
using EMSP.Utility.Extensions;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

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
        private float _amperage = 2f;

        [Header("Mathematics")]
        [SerializeField]
        private MagneticTensionInSpace _magneticTensionInSpace;
        #endregion

        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public int RangeLength { get { return _rangeLength; } }

        public float Amperage { get { return _amperage; } }

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
                    _magneticTensionInSpace.Show();
                    break;
            }
        }

        public void DestroyCalculations()
        {
            MagneticTensionInSpace.DestroyPoints();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}