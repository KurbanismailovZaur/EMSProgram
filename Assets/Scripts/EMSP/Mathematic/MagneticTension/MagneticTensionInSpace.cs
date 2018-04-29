﻿using EMSP.Communication;
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
        #endregion

        #region Events
        public CalculatedEvent Calculated = new CalculatedEvent();

        public DestroyedEvent Destroyed = new DestroyedEvent();

        public VisibilityChangedEvent VisibilityChanged = new VisibilityChangedEvent();
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
            }
        }

        public ReadOnlyCollection<MagneticTensionPoint> MTPoints { get { return _mtPoints.AsReadOnly(); } }

        public MaxMagneticTensions MaxMagneticTensionsInTime { get { return _maxMagneticTensions; } }

        public float CurrentMaxMagneticTension { get { return _amperageMode == AmperageMode.Computational ? _maxMagneticTensions.Calculated : _maxMagneticTensions.Precomputed; } }

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
        public void Calculate(int rangeLength, Wiring wiring, IEnumerable<float> timeSteps)
        {
            if (_isCalculated)
            {
                DestroyMagneticTensions();
            }

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

                        MagneticTensionInTime[] magneticTensionsInTime = new MagneticTensionInTime[timeSteps.Count()];

                        for (int w = 0; w < timeSteps.Count(); w++)
                        {
                            magneticTensionsInTime[w] = new MagneticTensionInTime(timeSteps.ElementAt(w), _mtCalculator.Calculate(wiring, point, timeSteps.ElementAt(w)));
                        }

                        magneticTensionsInfo[index++] = new MagneticTensionInfo(point, magneticTensionsInTime);
                    }
                }
            }

            _maxMagneticTensions = new MaxMagneticTensions
            {
                Calculated = magneticTensionsInfo.Max(x => x.MagneticTensionsInTime.Max(y => y.MagneticTensionResult.CalculatedAmperageResult)),
                Precomputed = magneticTensionsInfo.Max(x => x.MagneticTensionsInTime.Max(y => y.MagneticTensionResult.PrecomputedAmperageResult))
            };

            float _maxMagneticTensionInTime = _amperageMode == AmperageMode.Computational ? _maxMagneticTensions.Calculated : _maxMagneticTensions.Precomputed;

            foreach (MagneticTensionInfo mtInfo in magneticTensionsInfo)
            {
                float gradientValue = 0;

                if (_maxMagneticTensionInTime != 0f)
                {
                    MagneticTensionResult magneticTensionResult = mtInfo.MagneticTensionsInTime[0].MagneticTensionResult;
                    float magneticTensionInTime = _amperageMode == AmperageMode.Computational ? magneticTensionResult.CalculatedAmperageResult : magneticTensionResult.PrecomputedAmperageResult;

                    gradientValue = magneticTensionInTime.Remap(0f, _maxMagneticTensionInTime, 0f, 1f);
                }

                MagneticTensionPoint magneticTensionPoint = _magneticTensionPointFactory.Create(this, (PrimitiveType)(int)_meshType, transform, _magneticTensionPointMaterial, step + (step * _pointSizeStretchPercent), gradientValue, mtInfo);
                _mtPoints.Add(magneticTensionPoint);
            }

            _isCalculated = true;

            Calculated.Invoke(this);

            _tensionFilterRange = new Range(0f, _maxMagneticTensionInTime);
            _currentTensionFilterRange = _tensionFilterRange;
        }

        public void FilterPointsByTension(float min, float max)
        {
            FilterPointsByTension(new Range(min, max));
        }

        public void FilterPointsByTension(Range range)
        {
            _currentTensionFilterRange = range.Clamp(_tensionFilterRange);

            Func<MagneticTensionPoint, float> amperageResultSelector;
            if (_amperageMode == AmperageMode.Computational)
            {
                amperageResultSelector = (MagneticTensionPoint p) => p.CurrentMagneticTension.CalculatedAmperageResult;
            }
            else
            {
                amperageResultSelector = (MagneticTensionPoint p) => p.CurrentMagneticTension.PrecomputedAmperageResult;
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
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}