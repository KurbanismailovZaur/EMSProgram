using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.Magnetic
{
    [Serializable]
    public struct MagneticTensionInfo
    {
        private Vector3 _point;

        private float _precomputedMagneticTension;

        private CalculatedMagneticTensionInTime[] _calculatedMagneticTensionsInTime;

        public Vector3 Point { get { return _point; } }

        public float PrecomputedMagneticTension { get { return _precomputedMagneticTension; } }

        public CalculatedMagneticTensionInTime[] CalculatedMagneticTensionsInTime { get { return _calculatedMagneticTensionsInTime; } }

        public MagneticTensionInfo(Vector3 point, float precomputedMagneticTension, CalculatedMagneticTensionInTime[] magneticTensionsInTime)
        {
            _point = point;
            _precomputedMagneticTension = precomputedMagneticTension;
            _calculatedMagneticTensionsInTime = magneticTensionsInTime;
        }
    }
}