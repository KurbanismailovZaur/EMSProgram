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

        private float _magneticTension;

        public float Time { get { return _time; } }

        public float MagneticTension { get { return _magneticTension; } }

        public MagneticTensionInTime(float time, float magneticTension)
        {
            _time = time;
            _magneticTension = magneticTension;
        }
    }
}