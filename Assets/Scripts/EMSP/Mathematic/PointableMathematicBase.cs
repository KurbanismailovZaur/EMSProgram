using EMSP.Communication;
using EMSP.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Mathematic
{
    public abstract class PointableMathematicBase : MathematicBase, IPointableCalculationMethod
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

            public readonly float PrecomputedValue;

            public readonly PointableCalculatedValueInTime[] CalculatedValuesInTime;

            public PointInfo(Vector3 pos, float precomputedMagneticTension, PointableCalculatedValueInTime[] calculatedValuesInTime)
            {
                Position = pos;
                PrecomputedValue = precomputedMagneticTension;
                CalculatedValuesInTime = calculatedValuesInTime;
            }
        }
        #endregion

        #region Classes
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
        private CalculationPoint.Factory _calculationPointFactory = new CalculationPoint.Factory();

        private AmperageMode _amperageMode;

        private bool _isCalculated;

        private List<CalculationPoint> _calculatedPoints = new List<CalculationPoint>();

        [SerializeField]
        private Material _pointMaterial;

        [SerializeField]
        [Range(0f, 1f)]
        private float _stretchPercent = 0f;

        [SerializeField]
        [Range(0f, 1f)]
        private float _pointSizeStretchPercent = 0f;

        [SerializeField]
        private MeshType _meshType;

        private MaxCalculatedValues _maxCalculatedValues;

        private int _currentTimeIndex;

        [SerializeField]
        private Gradient _valuesGradient;

        private Range _valueFilterRange;

        private Range _currentValueFilterRange;

        private float _pointsSize;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public abstract CalculationType Type { get; }

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

        public ReadOnlyCollection<CalculationPoint> CalculatedPoints { get { return _calculatedPoints.AsReadOnly(); } }

        public MaxCalculatedValues MaxCalculatedValuesInTime { get { return _maxCalculatedValues; } }

        public float CurrentModeMaxCalculatedValue { get { return _amperageMode == AmperageMode.Computational ? _maxCalculatedValues.Calculated : _maxCalculatedValues.Precomputed; } }

        public float MaxCalculatedValue { get { return _maxCalculatedValues.Max; } }

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

        public Range CurrentValueFilterRange
        {
            get { return _currentValueFilterRange; }
            set
            {
                if (_currentValueFilterRange == value)
                {
                    return;
                }

                _currentValueFilterRange = value;

                CurrentValueFilterRangeChanged.Invoke(this, _currentValueFilterRange);
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
            DestroyCalculatedPoints();

            Bounds wiringBounds = wiring.GetBounds();
            float maxSide = wiringBounds.MaxSide();
            float stretchedMaxSide = maxSide + (maxSide * _stretchPercent);

            float step = stretchedMaxSide / (rangeLength - 1);

            List<PointableCalculatedValueInfo> calculatedValueInfos = new List<PointableCalculatedValueInfo>((int)Mathf.Pow(rangeLength, 3));
            //CalculatedValueInfo[] calculatedValuesInfo = new CalculatedValueInfo[(int)Mathf.Pow(rangeLength, 3)];
            //int index = 0;

            for (int i = 0; i < rangeLength; i++)
            {
                for (int j = 0; j < rangeLength; j++)
                {
                    for (int k = 0; k < rangeLength; k++)
                    {
                        Vector3 point = wiringBounds.center - (new Vector3(stretchedMaxSide, stretchedMaxSide, stretchedMaxSide) / 2f) + (new Vector3(i, j, k) * step);

                        if (((PointableMathematicCalculatorBase)Calculator).CheckIntersection(wiring, point)) continue; 

                        //float precomputedValue = Calculator.CalculateWithPrecomputedAmperage(wiring, point);
                        Data precomputedResultData = Calculator.Calculate(new Data()
                        {
                            { "amperageMode", AmperageMode.Precomputed },
                            { "wiring", wiring },
                            { "point", point },
                            { "time", 0f }
                        });

                        float precomputedValue = precomputedResultData.GetValue<float>("result");

                        PointableCalculatedValueInTime[] calculatedValuesInTime = new PointableCalculatedValueInTime[timeSteps.Count()];

                        for (int w = 0; w < timeSteps.Count(); w++)
                        {
                            float time = timeSteps.ElementAt(w);
                            //float calculatedValue = Calculator.CalculateWithComputationalAmperage(wiring, point, time);
                            Data calculatedResultData = Calculator.Calculate(new Data()
                            {
                                { "amperageMode", AmperageMode.Computational },
                                { "wiring", wiring },
                                { "point", point },
                                { "time", time }
                            });

                            float calculatedValue = calculatedResultData.GetValue<float>("result");

                            calculatedValuesInTime[w] = new PointableCalculatedValueInTime(time, calculatedValue);
                        }

                        calculatedValueInfos.Add(new PointableCalculatedValueInfo(point, precomputedValue, calculatedValuesInTime));
                        //calculatedValuesInfo[index++] = new CalculatedValueInfo(point, precomputedValue, calculatedValuesInTime);
                    }
                }
            }

            _maxCalculatedValues = GetMaxCalculatedValues(calculatedValueInfos);

            float maxCalculatedValue = _amperageMode == AmperageMode.Computational ? _maxCalculatedValues.Calculated : _maxCalculatedValues.Precomputed;

            _pointsSize = step + (step * _pointSizeStretchPercent);

            CreatePoints(calculatedValueInfos, maxCalculatedValue);
        }

        public void Restore(PointsInfo pointsInfo)
        {
            DestroyCalculatedPoints();

            if (pointsInfo == null) return;

            int pointsCount = pointsInfo.Infos.Count;

            List<PointableCalculatedValueInfo> calculatedValuesInfo = new List<PointableCalculatedValueInfo>(pointsCount);
            for (int i = 0; i < pointsCount; i++) calculatedValuesInfo.Add(new PointableCalculatedValueInfo());

            for (int i = 0; i < pointsCount; ++i)
            {
                calculatedValuesInfo[i] = new PointableCalculatedValueInfo(pointsInfo.Infos[i].Position, pointsInfo.Infos[i].PrecomputedValue, pointsInfo.Infos[i].CalculatedValuesInTime);
            }

            _maxCalculatedValues = GetMaxCalculatedValues(calculatedValuesInfo);

            float maxCalculatedValue = _amperageMode == AmperageMode.Computational ? _maxCalculatedValues.Calculated : _maxCalculatedValues.Precomputed;

            _pointsSize = pointsInfo.PointsSize;

            CreatePoints(calculatedValuesInfo, maxCalculatedValue);
        }

        private void CreatePoints(List<PointableCalculatedValueInfo> calculatedValuesInfo, float maxCalculatedValue)
        {
            foreach (PointableCalculatedValueInfo mtInfo in calculatedValuesInfo)
            {
                float gradientValue = 0;

                if (maxCalculatedValue != 0f)
                {
                    float calculatedValue = _amperageMode == AmperageMode.Computational ? mtInfo.CalculatedValueInTime[0].CalculatedValue : mtInfo.PrecomputedValue;

                    gradientValue = calculatedValue.Remap(0f, maxCalculatedValue, 0f, 1f);
                }

                CalculationPoint calculatedPoint = _calculationPointFactory.Create(this, (PrimitiveType)(int)_meshType, transform, _pointMaterial, _pointsSize, gradientValue, mtInfo);
                _calculatedPoints.Add(calculatedPoint);
            }

            _isCalculated = true;
            Calculated.Invoke(this);

            _valueFilterRange = new Range(0f, maxCalculatedValue);
            CurrentValueFilterRange = _valueFilterRange;
        }

        private MaxCalculatedValues GetMaxCalculatedValues(List<PointableCalculatedValueInfo> calculatedValuesInfo)
        {
            MaxCalculatedValues maxCalculatedValues = new MaxCalculatedValues(0f, 0f);

            for (int i = 0; i < calculatedValuesInfo.Count; i++)
            {
                maxCalculatedValues.Precomputed = Math.Max(maxCalculatedValues.Precomputed, calculatedValuesInfo[i].PrecomputedValue);

                for (int j = 0; j < calculatedValuesInfo[i].CalculatedValueInTime.Length; j++)
                {
                    maxCalculatedValues.Calculated = Math.Max(maxCalculatedValues.Calculated, calculatedValuesInfo[i].CalculatedValueInTime[j].CalculatedValue);
                }
            }

            return maxCalculatedValues;
        }

        public void FilterPointsByTension(float min, float max)
        {
            FilterPointsByValue(new Range(min, max));
        }

        public void FilterPointsByValue(Range range)
        {
            CurrentValueFilterRange = range.Clamp(_valueFilterRange);

            FilterPointsByTension();
        }

        public void FilterPointsByTension()
        {
            Func<CalculationPoint, float> amperageResultSelector;

            if (_amperageMode == AmperageMode.Computational)
            {
                amperageResultSelector = (CalculationPoint p) => p.CurrentCalculatedMagneticTension;
            }
            else
            {
                amperageResultSelector = (CalculationPoint p) => p.PrecomputedMagneticTension;
            }

            foreach (CalculationPoint point in _calculatedPoints)
            {
                if (amperageResultSelector.Invoke(point).InRange(_currentValueFilterRange))
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
            foreach (CalculationPoint point in _calculatedPoints)
            {
                point.gameObject.SetActive(true);
            }
        }

        public void UpdatePointsMaterial()
        {
            foreach (CalculationPoint point in _calculatedPoints)
            {
                point.UpdatePoint();
            }
        }

        public void SetEntitiesToTime(int timeIndex)
        {
            _currentTimeIndex = timeIndex;

            foreach (CalculationPoint mtPoint in _calculatedPoints)
            {
                mtPoint.SetToTime(_currentTimeIndex);
            }

            FilterPointsByValue(_currentValueFilterRange);
        }

        public Color GetTensionColorFromGradient(float percent)
        {
            return _valuesGradient.Evaluate(percent);
        }

        public void DestroyCalculatedPoints()
        {
            if (!_isCalculated) return;

            foreach (CalculationPoint point in _calculatedPoints)
            {
                Destroy(point.gameObject);
            }

            _calculatedPoints.Clear();

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

            foreach (CalculationPoint point in _calculatedPoints)
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