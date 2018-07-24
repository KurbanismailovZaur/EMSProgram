using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
    [Serializable]
    public struct VectorableCalculatedValueInfo
    {
        private WireSegment _segment;

        private Dictionary<Wire, float> _precomputedValue;

        private float _precomputedSummaryValue;

        private VectorableCalculatedValueInTime[] _calculatedValueInTime;


        public WireSegment Segment { get { return _segment; } }

        public Dictionary<Wire, float> PrecomputedValue { get { return _precomputedValue; } }

        public float PrecomputedSummaryValue { get { return _precomputedSummaryValue; } }

        public VectorableCalculatedValueInTime[] CalculatedValueInTime { get { return _calculatedValueInTime; } }

        public VectorableCalculatedValueInfo(WireSegment segmentKey, Dictionary<Wire, float> precomputedValue, float precomputedSummaryValue, VectorableCalculatedValueInTime[] calculatedValueInTime)
        {
            _segment = segmentKey;
            _precomputedValue = precomputedValue;
            _precomputedSummaryValue = precomputedSummaryValue;
            _calculatedValueInTime = calculatedValueInTime;
        }
    }
}