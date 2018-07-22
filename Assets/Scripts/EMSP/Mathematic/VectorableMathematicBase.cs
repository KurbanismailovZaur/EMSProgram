﻿using EMSP.Communication;
using EMSP.UI.Windows;
using EMSP.UI.Windows.CalculatedInduction;
using EMSP.Utility.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Mathematic
{
    public abstract class VectorableMathematicBase : MathematicBase, IVectorableCalculationMethod
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

        [SerializeField]
        private ModalWindow _calculatedWindow;

        [SerializeField]
        private Material _highlightMaterial;

        private AmperageMode _amperageMode;

        private bool _isCalculated;

        private MaxCalculatedValues _maxCalculatedValues;

        // new 

        private Dictionary<string, VectorableCalculatedValueInfo> _calculated = new Dictionary<string, VectorableCalculatedValueInfo>();


        // old
        //private Dictionary<string, Dictionary<Wire, float>> _calculated = new Dictionary<string, Dictionary<Wire, float>>();

        //private GameObject _currentSegmentHighlightningGameObject;

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
            }
        }

        public float CurrentModeMaxCalculatedValue { get { return _amperageMode == AmperageMode.Computational ? _maxCalculatedValues.Calculated : _maxCalculatedValues.Precomputed; } }

        public bool IsVisible
        {
            get { return _isCalculated && _calculatedWindow.IsShowing; }
            set
            {
                if (_calculatedWindow.IsShowing == value || !_isCalculated)
                {
                    return;
                }

                if (value)
                    _calculatedWindow.ShowModal();
                else
                {
                    DisableCurrentSegmentHighlightning();
                    _calculatedWindow.Hide();
                }

                VisibilityChanged.Invoke(this, _calculatedWindow.IsShowing);
            }
        }

        public bool IsCalculated { get { return _isCalculated; } }

        #endregion

        #region Constructors
        #endregion

        #region Methods

        public void SetEntitiesToTime(int timeIndex)
        {

        }

        public void Calculate(Wiring wiring, IEnumerable<float> timeSteps)
        {
            DestroyCalculated();


            float maxPrecomputedValue = 0;
            float maxCalculatedValue = 0;
            foreach (var wire in wiring)
            {
                int segmentCount = wire.LocalPoints.Count - 1;


                float maxPrecomutedSummaryValueOfSegments = 0;
                float maxCalculatedSummaryValueOfSegments = 0;

                for (int segmentIndex = 0; segmentIndex < segmentCount; ++segmentIndex)
                {
                    string key = string.Format("{0}_{1}", wire.Name, segmentIndex);


                    Data precomputedResultData = Calculator.Calculate(new Data()
                    {
                        { "amperageMode", AmperageMode.Precomputed },
                        { "name", wire.Name },
                        { "segment", segmentIndex },
                        { "wiring", wiring },
                        { "time", 0f }
                    });

                    Dictionary<Wire, float> precomputedValue = precomputedResultData.GetValue<Dictionary<Wire, float>>("result");
                    float precomputedSummaryValue = precomputedResultData.GetValue<float>("summary");
                    if (precomputedSummaryValue > maxPrecomutedSummaryValueOfSegments) maxPrecomutedSummaryValueOfSegments = precomputedSummaryValue;

                    VectorableCalculatedValueInTime[] calculatedValuesInTime = new VectorableCalculatedValueInTime[timeSteps.Count()];

                    float maxSummaryCalculatedValue = 0;
                    for (int w = 0; w < timeSteps.Count(); w++)
                    {
                        float time = timeSteps.ElementAt(w);

                        Data calculatedResultData = Calculator.Calculate(new Data()
                        {
                            { "amperageMode", AmperageMode.Computational },
                            { "name", wire.Name },
                            { "segment", segmentIndex },
                            { "wiring", wiring },
                            { "time", time }
                        });

                        Dictionary<Wire, float> calculatedValue = calculatedResultData.GetValue<Dictionary<Wire, float>>("result");
                        float calculatedSummaryValue = calculatedResultData.GetValue<float>("summary");
                        if (calculatedSummaryValue > maxSummaryCalculatedValue) maxSummaryCalculatedValue = calculatedSummaryValue;

                        calculatedValuesInTime[w] = new VectorableCalculatedValueInTime(time, calculatedValue, calculatedSummaryValue);
                    }
                    if (maxSummaryCalculatedValue > maxCalculatedSummaryValueOfSegments) maxCalculatedSummaryValueOfSegments = maxSummaryCalculatedValue;

                    VectorableCalculatedValueInfo segmentValuesInfo = new VectorableCalculatedValueInfo(key, precomputedValue, precomputedSummaryValue, calculatedValuesInTime);

                    _calculated.Add(key, segmentValuesInfo);
                }

                if (maxPrecomutedSummaryValueOfSegments > maxPrecomputedValue) maxPrecomputedValue = maxPrecomutedSummaryValueOfSegments;
                if (maxCalculatedSummaryValueOfSegments > maxCalculatedValue) maxCalculatedValue = maxCalculatedSummaryValueOfSegments;
            }

            _maxCalculatedValues = new MaxCalculatedValues(maxCalculatedValue, maxPrecomputedValue);

            _isCalculated = true;

            Calculated.Invoke(this);
        }

        public void ShowCalculatedFor(string wireName, int segmentNumber)
        {
            if (!IsCalculated) return;

            string key = string.Format("{0}_{1}", wireName, segmentNumber);

            switch (Type)
            {
                case CalculationType.Induction:
                    ((CalculatedInductionWindow)_calculatedWindow).DrawCalculated(wireName, segmentNumber, _calculated[key]);
                    break;
                default:
                    Debug.LogError("Unexpeted CalculationType = " + Type.ToString());
                    break;
            }

            HighLightSelectedSegment(wireName, segmentNumber);
        }

        public void DestroyCalculated()
        {
            if (!_isCalculated) return;

            _calculated.Clear();

            IsVisible = false;
            _isCalculated = false;

            Destroyed.Invoke(this);
        }

        private void HighLightSelectedSegment(string wireName, int segmentNumber)
        {
            DisableCurrentSegmentHighlightning();

            Wire selectedWire = WiringManager.Instance.Wiring.GetWireByName(wireName);

            Vector3 startSegmentPoint;
            Vector3 endSegmentPoint;

            selectedWire.GetSegment(segmentNumber, out startSegmentPoint, out endSegmentPoint);

            // create highlighter
            float halfMagnitude = (endSegmentPoint - startSegmentPoint).magnitude / 2;

            Vector3 pos = startSegmentPoint + (endSegmentPoint - startSegmentPoint).normalized * halfMagnitude;
            Quaternion rot = Quaternion.FromToRotation(Vector3.up, endSegmentPoint - startSegmentPoint);
            Vector3 scl = new Vector3(0.05f, halfMagnitude, 0.05f);

            _currentSegmentHighlightningGameObject = GameObject.CreatePrimitive(PrimitiveType.Capsule);
            _currentSegmentHighlightningGameObject.transform.position = pos;
            _currentSegmentHighlightningGameObject.transform.rotation = rot;
            _currentSegmentHighlightningGameObject.transform.localScale = scl;

            _currentSegmentHighlightningGameObject.GetComponent<MeshRenderer>().material = _highlightMaterial;

            Destroy(_currentSegmentHighlightningGameObject.GetComponent<Collider>());
        }

        private void DisableCurrentSegmentHighlightning()
        {
            if (_currentSegmentHighlightningGameObject != null)
                Destroy(_currentSegmentHighlightningGameObject);

            _currentSegmentHighlightningGameObject = null;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}