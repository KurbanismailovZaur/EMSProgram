using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.MagneticTension
{
    [Serializable]
    public struct MagneticTensionInfo
    {
        private Vector3 _point;

        private MagneticTensionInTime[] _magneticTensionsInTime;

        public Vector3 Point { get { return _point; } }

        public MagneticTensionInTime[] MagneticTensionsInTime { get { return _magneticTensionsInTime; } }

        public MagneticTensionInfo(Vector3 point, MagneticTensionInTime[] magneticTensionsInTime)
        {
            _point = point;
            _magneticTensionsInTime = magneticTensionsInTime;
        }
    }
}