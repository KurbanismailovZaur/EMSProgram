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
        private Range _timeRange = new Range(0f, 0.1f);

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
        public Range TimeRange
        {
            get { return _timeRange; }
            set
            {
                if (SetTimeRange(value))
                {
                    CalculateSteps();
                }
            }
        }

        public int StepsCount
        {
            get { return _stepsCount; }
            set
            {
                if (SetStepsCount(value))
                {
                    CalculateSteps();
                }
            }
        }

        public float Delta { get { return _delta; } }

        public ReadOnlyCollection<float> Steps { get { return _steps.AsReadOnly(); } }

        public int TimeIndex
        {
            get { return _timeIndex; }
            set
            {
                int timeIndex = Mathf.Clamp(value, 0, _stepsCount - 1);

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

        private bool SetTimeRange(Range timeRange)
        {
            timeRange = new Range(Mathf.Max(timeRange.Start, 0f), Mathf.Max(timeRange.End, 0f));

            if (timeRange == _timeRange)
            {
                return false;
            }

            _timeRange = timeRange;

            return true;
        }

        private bool SetStepsCount(int stepsCount)
        {
            stepsCount = Mathf.Max(stepsCount, 3);

            if (_stepsCount == stepsCount)
            {
                return false;
            }

            _stepsCount = stepsCount;

            return true;
        }

        public void SetTimeParameters(Range timeRange, int stepsCount)
        {
            int result = 0;

            result |= SetTimeRange(timeRange) ? 1 : 0;
            result |= SetStepsCount(stepsCount) ? 1 : 0;

            if (result == 1)
            {
                CalculateSteps();
            }
        }

        public void SetTimeParameters(float startTime, float endTime, int stepsCount)
        {
            SetTimeParameters(new Range(startTime, endTime), stepsCount);
        }


        private void CalculateSteps()
        {
            float distance = _timeRange.End - _timeRange.Start;
            _delta = distance / (_stepsCount - 1);

            _steps.Clear();
            _steps.Add(_timeRange.Start);

            for (int i = 1; i < _stepsCount - 1; i++)
            {
                _steps.Add(_timeRange.Start + (_delta * i));
            }

            _steps.Add(_timeRange.End);

            TimeParametersChanged.Invoke(this);

            TimeIndex = 0;
        }

        public float[] CalculateSteps(float start, float end, int stepsCount)
        {
            List<float> steps = new List<float>();

            float distance = end - start;
            _delta = distance / (stepsCount - 1);

            steps.Add(start);

            for (int i = 1; i < stepsCount - 1; i++)
            {
                steps.Add(start + (_delta * i));
            }

            steps.Add(end);

            return steps.ToArray();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}