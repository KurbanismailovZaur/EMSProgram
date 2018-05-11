using EMSP.Utility.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.MagneticTension
{
	public class MagneticTensionPoint : MonoBehaviour 
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
            public MagneticTensionPoint Create(MagneticTensionInSpace magneticTensionInSpace, PrimitiveType primitiveType, Transform parent, Material material, float size, float gradientValue, MagneticTensionInfo mtInfo)
            {
                MagneticTensionPoint magneticTensionPoint = GameObject.CreatePrimitive(primitiveType).AddComponent<MagneticTensionPoint>();
                magneticTensionPoint.name = "Point";
                magneticTensionPoint.transform.position = mtInfo.Point;
                magneticTensionPoint.transform.localScale = new Vector3(size, size, size);
                magneticTensionPoint.transform.SetParent(parent, true);

                magneticTensionPoint._magneticTensionInSpace = magneticTensionInSpace;

                Renderer magneticTensionRenderer = magneticTensionPoint.GetComponent<Renderer>();

                magneticTensionPoint._material = new Material(material)
                {
                    color = magneticTensionPoint._magneticTensionInSpace.GetTensionColorFromGradient(gradientValue)
                };

                magneticTensionRenderer.material = magneticTensionPoint._material;

                magneticTensionPoint._precomputedMagneticTension = mtInfo.PrecomputedMagneticTension;
                magneticTensionPoint._calculatedMagneticTensionsInTime = mtInfo.CalculatedMagneticTensionsInTime;

                return magneticTensionPoint;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private MagneticTensionInSpace _magneticTensionInSpace;

        private Material _material;

        private float _precomputedMagneticTension;

        private CalculatedMagneticTensionInTime[] _calculatedMagneticTensionsInTime;

        private int _currentTimeIndex;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public float PrecomputedMagneticTension { get { return _precomputedMagneticTension; } }

        public CalculatedMagneticTensionInTime[] CalculatedMagneticTensionsInTime { get { return _calculatedMagneticTensionsInTime; } }

        public float CurrentCalculatedMagneticTension { get { return _calculatedMagneticTensionsInTime[_currentTimeIndex].CalculatedMagneticTension; } }
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

            if (_magneticTensionInSpace.AmperageMode == AmperageMode.Computational)
            {
                magneticTension = _calculatedMagneticTensionsInTime[_currentTimeIndex].CalculatedMagneticTension;
                concreteMaxMagneticTension = _magneticTensionInSpace.MaxMagneticTensionsInTime.Calculated;
            }
            else
            {
                magneticTension = _precomputedMagneticTension;
                concreteMaxMagneticTension = _magneticTensionInSpace.MaxMagneticTensionsInTime.Precomputed;
            }

            _material.color = _magneticTensionInSpace.GetTensionColorFromGradient(magneticTension.Remap(0f, concreteMaxMagneticTension, 0f, 1f));
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}