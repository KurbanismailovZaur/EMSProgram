using EMSP.Communication;
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

        public Color TransparentColor;

        [SerializeField]
        private ModalWindow _calculatedWindow;

        private AmperageMode _amperageMode;

        private bool _isCalculated;

        private MaxCalculatedValues _maxCalculatedValues;
        private float _minCalculatedValue;

        private Dictionary<WireSegmentVisual, VectorableCalculatedValueInfo> _calculated = new Dictionary<WireSegmentVisual, VectorableCalculatedValueInfo>();

        private WireSegmentVisual _selectedSegment;

        private int _currentTimeIndex;

        [SerializeField]
        private Gradient _valuesGradient;

        private Range _valueFilterRange;

        private Range _currentValueFilterRange;
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

                ((CalculatedInductionWindow)_calculatedWindow).OnAmperageModeChanged(_amperageMode);

                UpdateSegmentsColor();
                // filter selected segments
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
                {
                    _calculatedWindow.ShowModal();
                    FilterVectorsByValue(_valueFilterRange);
                }
                else
                {
                    SetDefaultColorForAllSegments();
                    _calculatedWindow.Hide();
                }

                VisibilityChanged.Invoke(this, _calculatedWindow.IsShowing);
            }
        }

        public bool IsCalculated { get { return _isCalculated; } }

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

        private Induction.InductionCalculator.InductionResultCalculation[] _serializibleData;

        #endregion

        #region Constructors
        #endregion

        #region Methods
        public Induction.InductionCalculator.InductionResultCalculation[] GetSerializibleData()
        {
            return _serializibleData;
        }

        public void SetEntitiesToTime(int timeIndex)
        {
            if (_selectedSegment == null) return;

            _currentTimeIndex = timeIndex;

            ((CalculatedInductionWindow)_calculatedWindow).OnTimeStemChanged(_currentTimeIndex);

            UpdateSegmentsColor();
        }

        public void FilterVectorsByValue(Range range)
        {
            foreach(var kvp in _calculated)
            {
                var val = kvp.Value.SummaryPrecomputedValue;
                if (val > range.Max || val < range.Min)
                {
                    kvp.Key.SetHighlight(TransparentColor, false, true);
                }
                else
                {
                    kvp.Key.DisableHighlight(false, true);
                }
            }

            CurrentValueFilterRange = range.Clamp(_valueFilterRange);
        }

        public void Calculate(Wiring wiring, IEnumerable<float> timeSteps)
        {
            DestroyCalculated();


            float maxPrecomputedValue = 0;
            float maxCalculatedValue = 0;

            #region old_mockup
            /*
            foreach (var wire in wiring)
            {
                int segmentCount = wire.LocalPoints.Count - 1;


                float maxPrecomutedSummaryValueOfSegments = 0;
                float maxCalculatedSummaryValueOfSegments = 0;

                for (int segmentIndex = 0; segmentIndex < segmentCount; ++segmentIndex)
                {
                    WireSegment key = wire.GetSegment(segmentIndex);

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
            */
            #endregion

            Data ResultData = Calculator.Calculate(new Data()
            {
                { "wiring", wiring },
            });

            Induction.InductionCalculator.InductionResultCalculation[] result = ResultData.GetValue<Induction.InductionCalculator.InductionResultCalculation[]>("result");
            _serializibleData = result;

            // для каждого провода создаём список влияющих на него проводов
            foreach (var wirePair in result)
            {
                Dictionary<Wire, float> dictForWireA = new Dictionary<Wire, float>();
                Dictionary<Wire, float> dictForWireB = new Dictionary<Wire, float>();

                float maxForA = wirePair.Value;
                float maxForB = wirePair.Value;

                dictForWireA.Add(wirePair.WireB, wirePair.Value);
                dictForWireB.Add(wirePair.WireA, wirePair.Value);


                foreach (var wp in result)
                {
                    if (wp == wirePair) continue;

                    if (wirePair.WireA == wp.WireA)
                    {
                        if(!dictForWireA.ContainsKey(wp.WireB))
                            dictForWireA.Add(wp.WireB, wp.Value);

                        if (wp.Value > maxForA) maxForA = wp.Value;
                    }

                    if (wirePair.WireA == wp.WireB)
                    {
                        if (!dictForWireA.ContainsKey(wp.WireA))
                            dictForWireA.Add(wp.WireA, wp.Value);

                        if (wp.Value > maxForA) maxForA = wp.Value;
                    }


                    if (wirePair.WireB == wp.WireA)
                    {
                        if (!dictForWireB.ContainsKey(wp.WireB))
                            dictForWireB.Add(wp.WireB, wp.Value);

                        if (wp.Value > maxForB) maxForB = wp.Value;
                    }

                    if (wirePair.WireB == wp.WireB)
                    {
                        if (!dictForWireB.ContainsKey(wp.WireA))
                            dictForWireB.Add(wp.WireA, wp.Value);

                        if (wp.Value > maxForB) maxForB = wp.Value;
                    }
                }




                // для кадого сегмента провода записываем одинаковые значения - список влияющих проводов
                foreach (var segment in wirePair.WireA.SegmentsVisual)
                {
                    if(!_calculated.ContainsKey(segment))
                        _calculated.Add(segment, new VectorableCalculatedValueInfo(segment, dictForWireA, maxForA, null));
                }

                foreach (var segment in wirePair.WireB.SegmentsVisual)
                {
                    if (!_calculated.ContainsKey(segment))
                        _calculated.Add(segment, new VectorableCalculatedValueInfo(segment, dictForWireB, maxForB, null));
                }
            }


            foreach(var calcInfo in _calculated.Values)
            {
                var val = calcInfo.SummaryPrecomputedValue;
                if (val > maxPrecomputedValue) maxPrecomputedValue = val;
            }
            maxCalculatedValue = maxPrecomputedValue;

            // always need
            _maxCalculatedValues = new MaxCalculatedValues(maxCalculatedValue, maxPrecomputedValue);

            _valueFilterRange = new Range(0f, _maxCalculatedValues.Max);

            CurrentValueFilterRange = _valueFilterRange;

            CalculateAndSetSegmentsColorsByTime();



            FilterVectorsByValue(CurrentValueFilterRange);


            _isCalculated = true;
            Calculated.Invoke(this);
        }

        public void Restore(Induction.InductionCalculator.InductionResultCalculation[] result)
        {
            float maxPrecomputedValue = 0;
            float maxCalculatedValue = 0;

            foreach (var wirePair in result)
            {
                Dictionary<Wire, float> dictForWireA = new Dictionary<Wire, float>();
                Dictionary<Wire, float> dictForWireB = new Dictionary<Wire, float>();

                float maxForA = wirePair.Value;
                float maxForB = wirePair.Value;

                dictForWireA.Add(wirePair.WireB, wirePair.Value);
                dictForWireB.Add(wirePair.WireA, wirePair.Value);


                foreach (var wp in result)
                {
                    if (wp == wirePair) continue;

                    if (wirePair.WireA == wp.WireA)
                    {
                        if (!dictForWireA.ContainsKey(wp.WireB))
                            dictForWireA.Add(wp.WireB, wp.Value);

                        if (wp.Value > maxForA) maxForA = wp.Value;
                    }

                    if (wirePair.WireA == wp.WireB)
                    {
                        if (!dictForWireA.ContainsKey(wp.WireA))
                            dictForWireA.Add(wp.WireA, wp.Value);

                        if (wp.Value > maxForA) maxForA = wp.Value;
                    }


                    if (wirePair.WireB == wp.WireA)
                    {
                        if (!dictForWireB.ContainsKey(wp.WireB))
                            dictForWireB.Add(wp.WireB, wp.Value);

                        if (wp.Value > maxForB) maxForB = wp.Value;
                    }

                    if (wirePair.WireB == wp.WireB)
                    {
                        if (!dictForWireB.ContainsKey(wp.WireA))
                            dictForWireB.Add(wp.WireA, wp.Value);

                        if (wp.Value > maxForB) maxForB = wp.Value;
                    }
                }




                // для кадого сегмента провода записываем одинаковые значения - список влияющих проводов
                foreach (var segment in wirePair.WireA.SegmentsVisual)
                {
                    if (!_calculated.ContainsKey(segment))
                        _calculated.Add(segment, new VectorableCalculatedValueInfo(segment, dictForWireA, maxForA, null));
                }

                foreach (var segment in wirePair.WireB.SegmentsVisual)
                {
                    if (!_calculated.ContainsKey(segment))
                        _calculated.Add(segment, new VectorableCalculatedValueInfo(segment, dictForWireB, maxForB, null));
                }
            }

            foreach (var calcInfo in _calculated.Values)
            {
                var val = calcInfo.SummaryPrecomputedValue;
                if (val > maxPrecomputedValue) maxPrecomputedValue = val;
            }
            maxCalculatedValue = maxPrecomputedValue;

            // always need
            _maxCalculatedValues = new MaxCalculatedValues(maxCalculatedValue, maxPrecomputedValue);

            _valueFilterRange = new Range(0f, _maxCalculatedValues.Max);

            CurrentValueFilterRange = _valueFilterRange;

            CalculateAndSetSegmentsColorsByTime();


            _isCalculated = true;
            _serializibleData = result;
            Calculated.Invoke(this);
        }

        public void ShowCalculatedFor(WireSegmentVisual segment)
        {
            if (!IsCalculated) return;

            switch (Type)
            {
                case CalculationType.Induction:
                    ((CalculatedInductionWindow)_calculatedWindow).DrawCalculated(_calculated[segment], _amperageMode, _currentTimeIndex);
                    break;
                default:
                    Debug.LogError("Unexpected CalculationType = " + Type.ToString());
                    break;
            }

            HighLightSelectedSegment(segment);
        }

        public void DestroyCalculated()
        {
            if (!_isCalculated) return;

            _calculated.Clear();
            _serializibleData = null;

            IsVisible = false;
            _isCalculated = false;

            Destroyed.Invoke(this);
        }

        private void CalculateAndSetSegmentsColorsByTime()
        {
            foreach (var calculatedInfo in _calculated)
            {
                float precomputedGradientValue = calculatedInfo.Value.SummaryPrecomputedValue.Remap(0, _maxCalculatedValues.Max, 0f, 1f);
                //float precomputedGradientValue = calculatedInfo.Value.PrecomputedMaxValue.Remap(_minCalculatedValue, _maxCalculatedValues.Max, 0f, 1f);
                Dictionary<int, float> calculatedGradientValues = new Dictionary<int, float>();

                //int timeIndex = 0;
                //foreach (var valueInTime in calculatedInfo.Value.CalculatedValueInTime)
                //{
                //    calculatedGradientValues.Add(timeIndex, valueInTime.MaxCalculatedValue.Remap(0f, _maxCalculatedValues.Max, 0f, 1f));
                //    ++timeIndex;
                //}

                for (int timeIndex = 0; timeIndex < Timing.TimeManager.Instance.StepsCount; ++timeIndex)
                {
                    calculatedGradientValues.Add(timeIndex, precomputedGradientValue);
                }


                Dictionary<int, Color> colorsByTime = new Dictionary<int, Color>();

                colorsByTime.Add(-1, _valuesGradient.Evaluate(precomputedGradientValue));
                foreach (var gradientInfo in calculatedGradientValues)
                {
                    colorsByTime.Add(gradientInfo.Key, _valuesGradient.Evaluate(gradientInfo.Value));
                }

                calculatedInfo.Key.FillGradientColors(colorsByTime);
            }
        }

        private void HighLightSelectedSegment(WireSegmentVisual segment)
        {
            DisableCurrentSegmentHighlightning(true);
            FilterVectorsByValue(CurrentValueFilterRange);

            _selectedSegment = segment;
            HiglightWireBySelectedSegment(Color.blue);
        }

        private void DisableCurrentSegmentHighlightning(bool fromClick)
        {
            if (_selectedSegment == null) return;
            foreach (var segment in _selectedSegment.GeneralWire.SegmentsVisual)
            {
                segment.DisableHighlight(fromClick, false);
            }
        }

        private void UpdateSegmentsColor()
        {
            if (_selectedSegment == null) return;

            foreach (var segment in _selectedSegment.GeneralWire.SegmentsVisual)
            {
                segment.SetColorByInductionValue(_amperageMode, _currentTimeIndex);
            }
        }

        private void HiglightWireBySelectedSegment(Color color)
        {
            if (_selectedSegment == null) return;

            foreach (var segment in _selectedSegment.GeneralWire.SegmentsVisual)
            {
                segment.SetHighlight(color, true);
            }
        }

        private void SetDefaultColorForAllSegments()
        {
            foreach(var segment in _calculated.Keys)
            {
                segment.DisableHighlight(true, true);
            }
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void InductionCalculatedWindow_OnUnselectCurrentWire()
        {
            DisableCurrentSegmentHighlightning(true);
            ((CalculatedInductionWindow)_calculatedWindow).ClearWindow();
        }
        #endregion
        #endregion
    }
}