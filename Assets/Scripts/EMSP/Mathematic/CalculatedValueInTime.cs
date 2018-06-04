using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
    [Serializable]
    public struct CalculatedValueInTime
    {
        private float _time;

        private float _calculatedMagneticTension;

        public float Time { get { return _time; } }

        public float CalculatedMagneticTension { get { return _calculatedMagneticTension; } }

        public CalculatedValueInTime(float time, float calculatedMagneticTension)
        {
            _time = time;
            _calculatedMagneticTension = calculatedMagneticTension;
        }
    }
}