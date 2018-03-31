using EMSP.Communication;
using EMSP.Utility.Extensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Mathematic.MagneticTension
{
    public class MagneticTensionInSpace : MonoBehaviour
    {
        #region Entities
        #region Enums
        public enum MeshType
        {
            Sphere = 0,
            Cube = 3
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        public struct MagneticTensionsInfo
        {
            private Vector3 _point;

            private float _magneticTension;

            public Vector3 Point { get { return _point; } }

            public float MagneticTension { get { return _magneticTension; } }

            public MagneticTensionsInfo(Vector3 point, float magneticTension)
            {
                _point = point;
                _magneticTension = magneticTension;
            }
        }
        #endregion

        #region Classes
        [Serializable]
        public class CalculatedEvent : UnityEvent<MagneticTensionInSpace> { }

        [Serializable]
        public class DestroyedEvent : UnityEvent<MagneticTensionInSpace> { }

        [Serializable]
        public class VisibilityChangedEvent : UnityEvent<MagneticTensionInSpace, bool> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private MagneticTensionCalculator _mtCalculator = new MagneticTensionCalculator();

        private MagneticTensionPoint.Factory _magneticTensionPointFactory = new MagneticTensionPoint.Factory();

        private bool _isCalculated;

        private List<MagneticTensionPoint> _mtPoints = new List<MagneticTensionPoint>();

        [SerializeField]
        private Material _magneticTensionPointMaterial;

        [SerializeField]
        [Range(0f, 1f)]
        private float _stretchPercent = 0f;

        [SerializeField]
        [Range(0f, 1f)]
        private float _pointSizeStretchPercent = 0f;

        [SerializeField]
        private MeshType _meshType;
        #endregion

        #region Events
        public CalculatedEvent Calculated = new CalculatedEvent();

        public DestroyedEvent Destroyed = new DestroyedEvent();

        public VisibilityChangedEvent VisibilityChanged = new VisibilityChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public ReadOnlyCollection<MagneticTensionPoint> MTPoints { get { return _mtPoints.AsReadOnly(); } }

        public bool IsVisible
        {
            get { return _isCalculated && gameObject.activeSelf; }
            set
            {
                if (gameObject.activeSelf == value || !_isCalculated)
                {
                    return;
                }

                gameObject.SetActive(value);

                VisibilityChanged.Invoke(this, gameObject.activeSelf);
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void Calculate()
        {
            if (_isCalculated)
            {
                DestroyMagneticTensions();
            }

            gameObject.SetActive(true);

            float rangeLength = MathematicManager.Instance.RangeLength;
            float amperage = MathematicManager.Instance.Amperage;

            Bounds wiringBounds = WiringManager.Instance.Wiring.GetBounds();
            float maxSide = wiringBounds.MaxSide();
            float stretchedMaxSide = maxSide + (maxSide * _stretchPercent);

            float step = stretchedMaxSide / (rangeLength - 1);

            MagneticTensionsInfo[] magneticTensionsInfo = new MagneticTensionsInfo[(int)Mathf.Pow(rangeLength, 3)];
            int index = 0;

            for (int i = 0; i < rangeLength; i++)
            {
                for (int j = 0; j < rangeLength; j++)
                {
                    for (int k = 0; k < rangeLength; k++)
                    {
                        Vector3 point = wiringBounds.center - (new Vector3(stretchedMaxSide, stretchedMaxSide, stretchedMaxSide) / 2f) + (new Vector3(i, j, k) * step);
                        float magneticTension = _mtCalculator.Calculate(WiringManager.Instance.Wiring, point, amperage);

                        magneticTensionsInfo[index++] = new MagneticTensionsInfo(point, magneticTension);
                    }
                }
            }

            float maxMagneticTension = magneticTensionsInfo.Max(x => x.MagneticTension);

            foreach (MagneticTensionsInfo mtInfo in magneticTensionsInfo)
            {
                float alpha = Remap(mtInfo.MagneticTension, 0f, maxMagneticTension, 0f, 1f);

                MagneticTensionPoint magneticTensionPoint = _magneticTensionPointFactory.Create((PrimitiveType)(int)_meshType, transform, _magneticTensionPointMaterial, step + (step * _pointSizeStretchPercent), alpha, mtInfo);
                _mtPoints.Add(magneticTensionPoint);
            }

            _isCalculated = true;

            Calculated.Invoke(this);

            //FilterPointsByAlpha(0.01f);
        }

        public void FilterPointsByAlpha(float alphaThresold)
        {
            foreach (MagneticTensionPoint point in _mtPoints)
            {
                if (point.Alpha < alphaThresold)
                {
                    point.gameObject.SetActive(false);
                }
                else
                {
                    point.gameObject.SetActive(true);
                }
            }
        }

        private float Remap(float value, float iMin, float iMax, float oMin, float oMax)
        {
            return oMin + (value - iMin) * (oMax - oMin) / (iMax - iMin);
        }

        public void DestroyMagneticTensions()
        {
            if (!_isCalculated)
            {
                return;
            }

            foreach (MagneticTensionPoint point in _mtPoints)
            {
                Destroy(point.gameObject);
            }

            _mtPoints.Clear();

            _isCalculated = false;

            Destroyed.Invoke(this);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}