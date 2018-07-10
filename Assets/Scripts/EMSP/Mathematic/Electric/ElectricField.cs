using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.Electric
{
	public class ElectricField : PointableMathematicBase 
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
        private ElectricFieldCalculator _electricFieldCalculator = new ElectricFieldCalculator();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        protected override MathematicCalculatorBase Calculator
        {
            get
            {
                return _electricFieldCalculator;
            }
        }

        public override CalculationType Type { get { return CalculationType.ElectricField; } }
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
