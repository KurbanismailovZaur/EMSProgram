﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Utility.Extensions
{
    public static class FloatExtensions
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public static float Remap(this float value, float iMin, float iMax, float oMin, float oMax)
        {
            return oMin + (value - iMin) * (oMax - oMin) / (iMax - iMin);
        }

        public static bool InRange(this float value, float min, float max)
        {
            return value.InRange(new Range(min, max));
        }

        public static bool InRange(this float value, Range range)
        {
            return value >= range.Min && value <= range.Max;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}