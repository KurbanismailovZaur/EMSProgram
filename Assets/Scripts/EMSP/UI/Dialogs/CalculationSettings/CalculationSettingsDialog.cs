using EMSP.Mathematic;
using EMSP.Timing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EMSP.UI.Dialogs.CalculationSettings
{
    public class CalculationSettingsDialog : ModalDialog
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        public struct Settings
        {
            private int _rangeLength;

            private Range _timeRange;

            private int _timeStepsCount;

            public int RangeLength
            {
                get { return _rangeLength; }
                set { _rangeLength = value; }
            }

            public Range TimeRange
            {
                get { return _timeRange; }
                set { _timeRange = value; }
            }

            public int TimeStepsCount
            {
                get { return _timeStepsCount; }
                set { _timeStepsCount = value; }
            }

            public Settings(int rangeLength, Range timeRange, int timeStepsCount)
            {
                _rangeLength = rangeLength;
                _timeRange = timeRange;
                _timeStepsCount = timeStepsCount;
            }
        }
        #endregion

        #region Classes
        [Serializable]
        public class ApplyedEvent : UnityEvent<CalculationSettingsDialog, Settings> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private GeneralPanel _generalPanel;
        #endregion

        #region Events
        public ApplyedEvent Applyed = new ApplyedEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public override void ShowModal()
        {
            base.ShowModal();

            ReadAllParameters();
        }

        private void ReadAllParameters()
        {
            _generalPanel.UpdateValues(MathematicManager.Instance.RangeLength, TimeManager.Instance.TimeRange, TimeManager.Instance.StepsCount);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void ApplyButton_OnClick()
        {
            Applyed.Invoke(this, new Settings(_generalPanel.RangeLength, _generalPanel.TimeRange, _generalPanel.TimeStepsCount));

            Hide();
        }
        #endregion
        #endregion
    }
}