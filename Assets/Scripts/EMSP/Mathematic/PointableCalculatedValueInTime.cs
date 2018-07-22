using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
    [Serializable]
    public struct PointableCalculatedValueInTime
    {
        private float _time;

        private float _calculatedValue;

        public float Time { get { return _time; } }

        public float CalculatedValue { get { return _calculatedValue; } }

        public PointableCalculatedValueInTime(float time, float calculatedValue)
        {
            _time = time;
            _calculatedValue = calculatedValue;
        }
    }
}