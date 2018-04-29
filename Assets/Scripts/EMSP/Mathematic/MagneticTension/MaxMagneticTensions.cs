using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.MagneticTension
{
    [Serializable]
    public struct MaxMagneticTensions
    {
        private float _calculated;

        private float _precomputed;

        public float Calculated { get { return _calculated; } set { _calculated = value; } }

        public float Precomputed { get { return _precomputed; } set { _precomputed = value; } }

        public MaxMagneticTensions(float calculated, float precomputed)
        {
            _calculated = calculated;
            _precomputed = precomputed;
        }
    }
}