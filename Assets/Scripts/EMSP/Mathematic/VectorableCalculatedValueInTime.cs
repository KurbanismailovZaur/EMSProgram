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

        private float _maxCalculatedValue;

        public float Time { get { return _time; } }

        public float MaxCalculatedValue { get { return _maxCalculatedValue; } }

        public Dictionary<Wire, float> CalculatedValue { get { return _calculatedValue; } }

        public VectorableCalculatedValueInTime(float time, Dictionary<Wire, float> calculatedValue, float maxCalculatedValue)
        {
            _time = time;
            _calculatedValue = calculatedValue;
            _maxCalculatedValue = maxCalculatedValue;
        }
    }
}