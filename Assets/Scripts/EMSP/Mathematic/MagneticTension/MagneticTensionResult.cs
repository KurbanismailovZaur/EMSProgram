using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.MagneticTension
{
    [Serializable]
    public struct MagneticTensionResult
    {
        private float _calculatedAmperageResult;

        private float _precomputedAmperageResult;

        public float CalculatedAmperageResult { get { return _calculatedAmperageResult; } set { _calculatedAmperageResult = value; } }

        public float PrecomputedAmperageResult { get { return _precomputedAmperageResult; } set { _precomputedAmperageResult = value; } }

        public MagneticTensionResult(float calculatedAmperageResult, float precomputedAmperageResult)
        {
            _calculatedAmperageResult = calculatedAmperageResult;
            _precomputedAmperageResult = precomputedAmperageResult;
        }
    }
}