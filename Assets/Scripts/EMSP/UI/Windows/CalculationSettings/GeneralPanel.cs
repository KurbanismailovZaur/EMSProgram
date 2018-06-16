using EMSP.App;
using EMSP.Mathematic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EMSP.UI.Windows.CalculationSettings
{
	public class GeneralPanel : Panel 
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
        private RangeLengthInputFilter _rangeLengthInputFilter;

        [SerializeField]
        private TimeRangeInputFilter _startTimeRangeInputFilter;

        [SerializeField]
        private TimeRangeInputFilter _endTimeRangeInputFilter;

        [SerializeField]
        private TimeStepsCountInputFilter _timeStepsCountInputFilter;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public int RangeLength { get { return _rangeLengthInputFilter.RangeLength; } }

        public Range TimeRange { get { return new Range(_startTimeRangeInputFilter.Time, _endTimeRangeInputFilter.Time); } }

        public int TimeStepsCount { get { return _timeStepsCountInputFilter.StepsCount; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void UpdateValues(int rangeLength, Range timeRange, int stepsCount)
        {
            _rangeLengthInputFilter.SetRangeLengthText(rangeLength);
            _startTimeRangeInputFilter.SetTimeText(timeRange.Start);
            _endTimeRangeInputFilter.SetTimeText(timeRange.End);
            _timeStepsCountInputFilter.SetTimeStepsCountText(stepsCount);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}