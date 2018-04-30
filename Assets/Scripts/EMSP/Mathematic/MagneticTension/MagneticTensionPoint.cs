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

                magneticTensionPoint._magneticTensionsInTime = mtInfo.MagneticTensionsInTime;

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

        private MagneticTensionInTime[] _magneticTensionsInTime;

        private int _currentTimeIndex;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public MagneticTensionInTime[] MagneticTensionsInTime { get { return _magneticTensionsInTime; } }

        public float Alpha { get { return _material.color.a; } }

        public MagneticTensionResult CurrentMagneticTension { get { return _magneticTensionsInTime[_currentTimeIndex].MagneticTensionResult; } }
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
                magneticTension = _magneticTensionsInTime[_currentTimeIndex].MagneticTensionResult.CalculatedAmperageResult;
                concreteMaxMagneticTension = _magneticTensionInSpace.MaxMagneticTensionsInTime.Calculated;
            }
            else
            {
                magneticTension = _magneticTensionsInTime[_currentTimeIndex].MagneticTensionResult.PrecomputedAmperageResult;
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