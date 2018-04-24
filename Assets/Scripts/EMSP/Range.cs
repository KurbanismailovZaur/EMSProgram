using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP
{
    [Serializable]
    public struct Range
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
        [SerializeField]
        private float _start;

        [SerializeField]
        private float _end;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public float Start
        {
            get { return _start; }
            set { _start = value; }
        }

        public float End
        {
            get { return _end; }
            set { _end = value; }
        }

        public float Middle { get { return _start + ((_end - _start) / 2f); } }

        public float Min
        {
            get { return _start <= _end ? _start : _end; }
        }

        public float Max
        {
            get { return _end >= _start ? _end : _start; }
        }
        #endregion

        #region Constructors
        public Range(float start, float end)
        {
            _start = start;
            _end = end;
        }
        #endregion

        #region Methods
        public static bool operator ==(Range obj1, Range obj2)
        {
            return obj1.Start == obj2.Start && obj1.End == obj2.End;
        }

        public static bool operator !=(Range obj1, Range obj2)
        {
            return !(obj1 == obj2);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}