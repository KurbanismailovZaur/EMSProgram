﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.MagneticTension
{
    public struct MagneticTensionDirectionResult
    {
        private Vector3 _calculatedAmperageResult;

        private Vector3 _precomputedAmperageResult;

        public Vector3 CalculatedAmperageResult { get { return _calculatedAmperageResult; } set { _calculatedAmperageResult = value; } }

        public Vector3 PrecomputedAmperageResult { get { return _precomputedAmperageResult; } set { _precomputedAmperageResult = value; } }

        public MagneticTensionDirectionResult(Vector3 calculatedAmperageResult, Vector3 precomputedAmperageResult)
        {
            _calculatedAmperageResult = calculatedAmperageResult;
            _precomputedAmperageResult = precomputedAmperageResult;
        }

        public static MagneticTensionDirectionResult operator +(MagneticTensionDirectionResult obj1, MagneticTensionDirectionResult obj2)
        {
            return new MagneticTensionDirectionResult(obj1.CalculatedAmperageResult + obj2.CalculatedAmperageResult, obj1.PrecomputedAmperageResult + obj2.PrecomputedAmperageResult);
        }
    }
}