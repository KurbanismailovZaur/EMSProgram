﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.Magnetic
{
    [Serializable]
    public struct CalculatedValueInfo
    {
        private Vector3 _point;

        private float _precomputedValue;

        private CalculatedValueInTime[] _calculatedValueInTime;

        public Vector3 Point { get { return _point; } }

        public float PrecomputedValue { get { return _precomputedValue; } }

        public CalculatedValueInTime[] CalculatedValueInTime { get { return _calculatedValueInTime; } }

        public CalculatedValueInfo(Vector3 point, float precomputedValue, CalculatedValueInTime[] calculatedValueInTime)
        {
            _point = point;
            _precomputedValue = precomputedValue;
            _calculatedValueInTime = calculatedValueInTime;
        }
    }
}