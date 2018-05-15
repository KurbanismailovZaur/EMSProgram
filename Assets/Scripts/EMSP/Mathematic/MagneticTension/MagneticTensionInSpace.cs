using EMSP.Communication;
using EMSP.Data.Serialization;
using EMSP.Timing;
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
        public struct PointInfo
        {
            public readonly Vector3 Position;

            public readonly float PrecomputedMagneticTension;

            public readonly CalculatedMagneticTensionInTime[] CalculatedMagneticTensionsInTime;

            public PointInfo(Vector3 pos, float precomputedMagneticTension, CalculatedMagneticTensionInTime[] calculatedMagneticTensionsInTime)
            {
                Position = pos;
                PrecomputedMagneticTension = precomputedMagneticTension;
                CalculatedMagneticTensionsInTime = calculatedMagneticTensionsInTime;
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

        [Serializable]
        public class CurrentTensionFilterRangeChangedEvent : UnityEvent<MagneticTensionInSpace, Range> { }

        public class PointsInfo
        {
            public readonly float PointsSize;

            public readonly List<PointInfo> Infos;

            public PointsInfo(float pointSize, List<PointInfo> pointsInfo)
            {
                PointsSize = pointSize;
                Infos = pointsInfo;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private MagneticTensionCalculator _mtCalculator = new MagneticTensionCalculator();

        private MagneticTensionPoint.Factory _magneticTensionPointFactory = new MagneticTensionPoint.Factory();

        private AmperageMode _amperageMode;

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

        private MaxMagneticTensions _maxMagneticTensions;

        private int _currentTimeIndex;

        [SerializeField]
        private Gradient _tensionsGradient;

        private Range _tensionFilterRange;

        private Range _currentTensionFilterRange;

        private float _pointsSize;
        #endregion

        #region Events
        public CalculatedEvent Calculated = new CalculatedEvent();

        public DestroyedEvent Destroyed = new DestroyedEvent();

        public VisibilityChangedEvent VisibilityChanged = new VisibilityChangedEvent();

        public CurrentTensionFilterRangeChangedEvent CurrentTensionFilterRangeChanged = new CurrentTensionFilterRangeChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
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

                FilterPointsByTension();
                UpdatePointsMaterial();
            }
        }

        public ReadOnlyCollection<MagneticTensionPoint> MTPoints { get { return _mtPoints.AsReadOnly(); } }

        public MaxMagneticTensions MaxMagneticTensionsInTime { get { return _maxMagneticTensions; } }

        public float CurrentMaxMagneticTension { get { return _amperageMode == AmperageMode.Computational ? _maxMagneticTensions.Calculated : _maxMagneticTensions.Precomputed; } }

        public float MaxMagneticTension { get { return _maxMagneticTensions.Max; } }

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

        public Range CurrentTensionFilterRange
        {
            get { return _currentTensionFilterRange; }
            set
            {
                if (_currentTensionFilterRange == value)
                {
                    return;
                }

                _currentTensionFilterRange = value;

                CurrentTensionFilterRangeChanged.Invoke(this, _currentTensionFilterRange);
            }
        }

        public bool IsCalculated { get { return _isCalculated; } }

        public float PointsSize { get { return _isCalculated ? _pointsSize : 0f; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void Calculate(int rangeLength, Wiring wiring, IEnumerable<float> timeSteps)
        {
            DestroyMagneticTensions();

            Bounds wiringBounds = wiring.GetBounds();
            float maxSide = wiringBounds.MaxSide();
            float stretchedMaxSide = maxSide + (maxSide * _stretchPercent);

            float step = stretchedMaxSide / (rangeLength - 1);

            MagneticTensionInfo[] magneticTensionsInfo = new MagneticTensionInfo[(int)Mathf.Pow(rangeLength, 3)];
            int index = 0;

            for (int i = 0; i < rangeLength; i++)
            {
                for (int j = 0; j < rangeLength; j++)
                {
                    for (int k = 0; k < rangeLength; k++)
                    {
                        Vector3 point = wiringBounds.center - (new Vector3(stretchedMaxSide, stretchedMaxSide, stretchedMaxSide) / 2f) + (new Vector3(i, j, k) * step);

                        float precomputedMagneticTension = _mtCalculator.CalculateWithPrecomputedAmperage(wiring, point);

                        CalculatedMagneticTensionInTime[] magneticTensionsInTime = new CalculatedMagneticTensionInTime[timeSteps.Count()];

                        for (int w = 0; w < timeSteps.Count(); w++)
                        {
                            float time = timeSteps.ElementAt(w);
                            float calculatedMagneticTension = _mtCalculator.CalculateWithComputationalAmperage(wiring, point, time);

                            magneticTensionsInTime[w] = new CalculatedMagneticTensionInTime(time, calculatedMagneticTension);
                        }

                        magneticTensionsInfo[index++] = new MagneticTensionInfo(point, precomputedMagneticTension, magneticTensionsInTime);
                    }
                }
            }

            _maxMagneticTensions = GetMaxMagneticTensions(magneticTensionsInfo);
            
            float maxMagneticTension = _amperageMode == AmperageMode.Computational ? _maxMagneticTensions.Calculated : _maxMagneticTensions.Precomputed;

            _pointsSize = step + (step * _pointSizeStretchPercent);

            CreatePoints(magneticTensionsInfo, maxMagneticTension);
        }

        public void Restore(PointsInfo pointsInfo)
        {
            DestroyMagneticTensions();

            int pointsCount = pointsInfo.Infos.Count;

            MagneticTensionInfo[] magneticTensionsInfo = new MagneticTensionInfo[pointsCount];

            for (int i = 0; i < pointsCount; ++i)
            {
                magneticTensionsInfo[i] = new MagneticTensionInfo(pointsInfo.Infos[i].Position, pointsInfo.Infos[i].PrecomputedMagneticTension, pointsInfo.Infos[i].CalculatedMagneticTensionsInTime);
            }

            _maxMagneticTensions = GetMaxMagneticTensions(magneticTensionsInfo);

            float maxMagneticTension = _amperageMode == AmperageMode.Computational ? _maxMagneticTensions.Calculated : _maxMagneticTensions.Precomputed;

            _pointsSize = pointsInfo.PointsSize;

            CreatePoints(magneticTensionsInfo, maxMagneticTension);
        }

        private void CreatePoints(MagneticTensionInfo[] magneticTensionsInfo, float maxMagneticTension)
        {
            foreach (MagneticTensionInfo mtInfo in magneticTensionsInfo)
            {
                float gradientValue = 0;

                if (maxMagneticTension != 0f)
                {
                    float magneticTension = _amperageMode == AmperageMode.Computational ? mtInfo.CalculatedMagneticTensionsInTime[0].CalculatedMagneticTension : mtInfo.PrecomputedMagneticTension;

                    gradientValue = magneticTension.Remap(0f, maxMagneticTension, 0f, 1f);
                }

                MagneticTensionPoint magneticTensionPoint = _magneticTensionPointFactory.Create(this, (PrimitiveType)(int)_meshType, transform, _magneticTensionPointMaterial, _pointsSize, gradientValue, mtInfo);
                _mtPoints.Add(magneticTensionPoint);
            }

            _isCalculated = true;
            Calculated.Invoke(this);

            _tensionFilterRange = new Range(0f, maxMagneticTension);
            CurrentTensionFilterRange = _tensionFilterRange;
        }

        private MaxMagneticTensions GetMaxMagneticTensions(MagneticTensionInfo[] magneticTensionsInfo)
        {
            MaxMagneticTensions maxMagneticTensions = new MaxMagneticTensions(0f, 0f);

            for (int i = 0; i < magneticTensionsInfo.Length; i++)
            {
                maxMagneticTensions.Precomputed = Math.Max(maxMagneticTensions.Precomputed, magneticTensionsInfo[i].PrecomputedMagneticTension);

                for (int j = 0; j < magneticTensionsInfo[i].CalculatedMagneticTensionsInTime.Length; j++)
                {
                    maxMagneticTensions.Calculated = Math.Max(maxMagneticTensions.Calculated, magneticTensionsInfo[i].CalculatedMagneticTensionsInTime[j].CalculatedMagneticTension);
                }
            }

            return maxMagneticTensions;
        }

        public void FilterPointsByTension(float min, float max)
        {
            FilterPointsByTension(new Range(min, max));
        }

        public void FilterPointsByTension(Range range)
        {
            CurrentTensionFilterRange = range.Clamp(_tensionFilterRange);

            FilterPointsByTension();
        }

        public void FilterPointsByTension()
        {
            Func<MagneticTensionPoint, float> amperageResultSelector;

            if (_amperageMode == AmperageMode.Computational)
            {
                amperageResultSelector = (MagneticTensionPoint p) => p.CurrentCalculatedMagneticTension;
            }
            else
            {
                amperageResultSelector = (MagneticTensionPoint p) => p.PrecomputedMagneticTension;
            }

            foreach (MagneticTensionPoint point in _mtPoints)
            {
                if (amperageResultSelector.Invoke(point).InRange(_currentTensionFilterRange))
                {
                    point.gameObject.SetActive(true);
                }
                else
                {
                    point.gameObject.SetActive(false);
                }
            }
        }

        public void ShowAllPoints()
        {
            foreach (MagneticTensionPoint point in _mtPoints)
            {
                point.gameObject.SetActive(true);
            }
        }

        public void UpdatePointsMaterial()
        {
            foreach (MagneticTensionPoint point in _mtPoints)
            {
                point.UpdatePoint();
            }
        }

        public void SetPointsToTime(int timeIndex)
        {
            _currentTimeIndex = timeIndex;

            foreach (MagneticTensionPoint mtPoint in _mtPoints)
            {
                mtPoint.SetToTime(_currentTimeIndex);
            }

            FilterPointsByTension(_currentTensionFilterRange);
        }

        public Color GetTensionColorFromGradient(float percent)
        {
            return _tensionsGradient.Evaluate(percent);
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

            IsVisible = false;
            _isCalculated = false;

            Destroyed.Invoke(this);
        }

        public PointsInfo GetPointsInfo()
        {
            if (!_isCalculated)
            {
                return null;
            }

            List<PointInfo> pointInfos = new List<PointInfo>();

            foreach (MagneticTensionPoint point in _mtPoints)
            {
                pointInfos.Add(new PointInfo(point.transform.position, point.PrecomputedMagneticTension, point.CalculatedMagneticTensionsInTime));
            }

            return new PointsInfo(_pointsSize, pointInfos);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}