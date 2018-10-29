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
        private WireSegmentVisual _segment;

        private Dictionary<Wire, float> _precomputedValue;

        private float _precomputedMaxValue;

        private VectorableCalculatedValueInTime[] _calculatedValueInTime;


        public WireSegmentVisual Segment { get { return _segment; } }

        public Dictionary<Wire, float> PrecomputedValue { get { return _precomputedValue; } }

        public float PrecomputedMaxValue { get { return _precomputedMaxValue; } }

        public VectorableCalculatedValueInTime[] CalculatedValueInTime { get { return _calculatedValueInTime; } }

        public VectorableCalculatedValueInfo(WireSegmentVisual segmentKey, Dictionary<Wire, float> precomputedValue, float precomputedMaxValue, VectorableCalculatedValueInTime[] calculatedValueInTime)
        {
            _segment = segmentKey;
            _precomputedValue = precomputedValue;
            _precomputedMaxValue = precomputedMaxValue;
            _calculatedValueInTime = calculatedValueInTime;
        }
    }
}