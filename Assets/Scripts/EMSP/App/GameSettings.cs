using Numba;
using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App
{
	public class GameSettings : MonoSingleton<GameSettings> 
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
        private readonly ExtensionFilter[] _modelExtensionFilter = new ExtensionFilter[] { new ExtensionFilter("3D Model", "obj") };

        private readonly ExtensionFilter[] _wiringExtensionFilter = new ExtensionFilter[] { new ExtensionFilter("Excel Worksheets 2003", "xls") };

        private readonly ExtensionFilter[] _projectExtensionFilter = new ExtensionFilter[] { new ExtensionFilter("EMSP", "emsp") };

        private readonly string _projectDefaultName = "Untitled";

        private readonly Vector3 _orbitControllerDefaultTargetVector = Vector3.up;

        private readonly float _orbitControllerDefaultTargetUpAngle = 0f;

        private readonly float _orbitControllerDefaultDistance = 8f;

        private readonly string _magneticTensionInSpaceDefaultName = "MagneticTensionInSpace";

        private readonly int _calculationDefaultMinRangeLength = 8;

        private readonly string _numberDecimalSeparator = ".";

        private readonly Range _defaultTimeRange = new Range(0f, 0.1f);

        private readonly int _defaultTimeStepsCount = 36;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public ExtensionFilter[] ModelExtensionFilter { get { return _modelExtensionFilter; } }

        public ExtensionFilter[] WiringExtensionFilter { get { return _wiringExtensionFilter; } }

        public ExtensionFilter[] ProjectExtensionFilter { get { return _projectExtensionFilter; } }

        public string ProjectDefaultName { get { return _projectDefaultName; } }

        public Vector3 OrbitControllerDefaultTargetVector { get { return _orbitControllerDefaultTargetVector; } }

        public float OrbitControllerDefaultTargetUpAngle { get { return _orbitControllerDefaultTargetUpAngle; } }

        public float OrbitControllerDefaultDistance { get { return _orbitControllerDefaultDistance; } }

        public string MagneticTensionInSpaceDefaultName { get { return _magneticTensionInSpaceDefaultName; } }

        public int CalculationDefaultMinRangeLength { get { return _calculationDefaultMinRangeLength; } }

        public string NumberDecimalSeparator { get { return _numberDecimalSeparator; } }

        public Range DefaultTimeRange { get { return _defaultTimeRange; } }

        public int DefaultTimeStepsCount { get { return _defaultTimeStepsCount; } }
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