using Numba;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EMSP.Timing
{
    public class TimeManager : MonoSingleton<TimeManager>
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
        [Range(0f, 32f)]
        private float _startTime;

        [SerializeField]
        [Range(0f, 32f)]
        private float _endTime = 0.1f;

        [SerializeField]
        [Range(3, 256)]
        private int _stepsCount = 36;

        private float _delta;
        
        private List<float> _steps = new List<float>();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public float StartTime
        {
            get { return _startTime; }
            set
            {
                _startTime = Mathf.Max(value, 0f);
                CalculateSteps();
            }
        }

        public float EndTime
        {
            get { return _endTime; }
            set
            {
                _endTime = Mathf.Max(value, 0f);
                CalculateSteps();
            }
        }

        public int StepsCount
        {
            get { return _stepsCount; }
            set
            {
                _stepsCount = Mathf.Max(value, 3);
                CalculateSteps();
            }
        }

        public float Delta { get { return _delta; } }

        public ReadOnlyCollection<float> Steps { get { return _steps.AsReadOnly(); } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            CalculateSteps();
        }

        private void CalculateSteps()
        {
            float distance = _endTime - _startTime;
            _delta = distance / _stepsCount;

            _steps.Clear();
            _steps.Add(_startTime);

            for (int i = 0; i < _stepsCount - 2; i++)
            {
                _steps.Add(_startTime + (_delta * (i + 1)));
            }

            _steps.Add(_endTime);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}