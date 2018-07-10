using EMSP.Utility.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
	public class CalculationPoint : MonoBehaviour 
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
            public CalculationPoint Create(PointableMathematicBase mathematicBase, PrimitiveType primitiveType, Transform parent, Material material, float size, float gradientValue, CalculatedValueInfo mtInfo)
            {
                CalculationPoint magneticTensionPoint = GameObject.CreatePrimitive(primitiveType).AddComponent<CalculationPoint>();
                magneticTensionPoint.name = "Point";
                magneticTensionPoint.transform.position = mtInfo.Point;
                magneticTensionPoint.transform.localScale = new Vector3(size, size, size);
                magneticTensionPoint.transform.SetParent(parent, true);

                magneticTensionPoint._mathematicBase = mathematicBase;

                Renderer magneticTensionRenderer = magneticTensionPoint.GetComponent<Renderer>();

                magneticTensionPoint._material = new Material(material)
                {
                    color = magneticTensionPoint._mathematicBase.GetTensionColorFromGradient(gradientValue)
                };

                magneticTensionRenderer.material = magneticTensionPoint._material;

                magneticTensionPoint._precomputedMagneticTension = mtInfo.PrecomputedValue;
                magneticTensionPoint._calculatedMagneticTensionsInTime = mtInfo.CalculatedValueInTime;

                return magneticTensionPoint;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private PointableMathematicBase _mathematicBase;

        private Material _material;

        private float _precomputedMagneticTension;

        private CalculatedValueInTime[] _calculatedMagneticTensionsInTime;

        private int _currentTimeIndex;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public float PrecomputedMagneticTension { get { return _precomputedMagneticTension; } }

        public CalculatedValueInTime[] CalculatedMagneticTensionsInTime { get { return _calculatedMagneticTensionsInTime; } }

        public float CurrentCalculatedMagneticTension { get { return _calculatedMagneticTensionsInTime[_currentTimeIndex].CalculatedValue; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void SetToTime(int timeIndex)
        {
            _currentTimeIndex = timeIndex;

            UpdatePoint();
        }

        public void UpdatePoint()
        {
            float magneticTension = 0f;
            float concreteMaxMagneticTension = 0f;

            if (_mathematicBase.AmperageMode == AmperageMode.Computational)
            {
                magneticTension = _calculatedMagneticTensionsInTime[_currentTimeIndex].CalculatedValue;
                concreteMaxMagneticTension = _mathematicBase.MaxCalculatedValuesInTime.Calculated;
            }
            else
            {
                magneticTension = _precomputedMagneticTension;
                concreteMaxMagneticTension = _mathematicBase.MaxCalculatedValuesInTime.Precomputed;
            }

            _material.color = _mathematicBase.GetTensionColorFromGradient(magneticTension.Remap(0f, concreteMaxMagneticTension, 0f, 1f));
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}