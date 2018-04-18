using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

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
        [Serializable]
        public class TimeParametersChangedEvent : UnityEvent<TimeManager> { }

        [Serializable]
        public class TimeIndexChangedEvent : UnityEvent<TimeManager, int> { }
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

        private int _timeIndex;
        #endregion

        #region Events
        public TimeParametersChangedEvent TimeParametersChanged = new TimeParametersChangedEvent();

        public TimeIndexChangedEvent TimeIndexChanged = new TimeIndexChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public float StartTime
        {
            get { return _startTime; }
            set
            {
                float startTime = Mathf.Max(value, 0f);

                if (_startTime == startTime)
                {
                    return;
                }

                _startTime = startTime;

                CalculateSteps();
            }
        }

        public float EndTime
        {
            get { return _endTime; }
            set
            {
                float endTime = Mathf.Max(value, 0f);

                if (_endTime == endTime)
                {
                    return;
                }

                _endTime = endTime;

                CalculateSteps();
            }
        }

        public int StepsCount
        {
            get { return _stepsCount; }
            set
            {
                int stepsCount = Mathf.Max(value, 3);

                if (_stepsCount == stepsCount)
                {
                    return;
                }

                _stepsCount = stepsCount;

                CalculateSteps();
            }
        }

        public float Delta { get { return _delta; } }

        public ReadOnlyCollection<float> Steps { get { return _steps.AsReadOnly(); } }

        public int TimeIndex
        {
            get { return _timeIndex; }
            set
            {
                int timeIndex = Mathf.Clamp(value, 0, _stepsCount);

                if (_timeIndex == timeIndex)
                {
                    return;
                }

                _timeIndex = timeIndex;

                TimeIndexChanged.Invoke(this, _timeIndex);
            }
        }
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

            for (int i = 0; i < _stepsCount - 1; i++)
            {
                _steps.Add(_startTime + (_delta * (i + 1)));
            }

            _steps.Add(_endTime);

            TimeParametersChanged.Invoke(this);
        }

        public void MoveTimeToNextStep()
        {
            TimeIndex += 1;
        }

        public void MoveTimeToPreviousStep()
        {
            TimeIndex -= 1;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}