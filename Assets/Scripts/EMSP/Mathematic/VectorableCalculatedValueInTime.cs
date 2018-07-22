using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
    [Serializable]
    public struct VectorableCalculatedValueInTime
    {
        private float _time;

        private Dictionary<Wire, float> _calculatedValue;

        private float _summaryCalculatedValue;

        public float Time { get { return _time; } }

        public float SummaryCalculatedValue { get { return _summaryCalculatedValue; } }

        public Dictionary<Wire, float> CalculatedValue { get { return _calculatedValue; } }

        public VectorableCalculatedValueInTime(float time, Dictionary<Wire, float> calculatedValue, float summaryCalculatedValue)
        {
            _time = time;
            _calculatedValue = calculatedValue;
            _summaryCalculatedValue = summaryCalculatedValue;
        }
    }
}