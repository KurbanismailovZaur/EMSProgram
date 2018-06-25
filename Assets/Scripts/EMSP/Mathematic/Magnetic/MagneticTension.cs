using EMSP.Communication;
using EMSP.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Mathematic.Magnetic
{
    public class MagneticTension : MathematicBase
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
        private MagneticTensionCalculator _calculator = new MagneticTensionCalculator();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        protected override MathematicCalculatorBase Calculator
        {
            get
            {
                return _calculator;
            }
        }

        public override CalculationType Type { get { return CalculationType.MagneticTension; } }
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