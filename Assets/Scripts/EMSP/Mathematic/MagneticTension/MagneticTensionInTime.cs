using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.MagneticTension
{
    [Serializable]
    public struct MagneticTensionInTime
    {
        private float _time;

        private MagneticTensionResult _magneticTensionResult;

        public float Time { get { return _time; } }

        public MagneticTensionResult MagneticTensionResult { get { return _magneticTensionResult; } }

        public MagneticTensionInTime(float time, MagneticTensionResult magneticTensionResult)
        {
            _time = time;
            _magneticTensionResult = magneticTensionResult;
        }
    }
}