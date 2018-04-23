using EMSP.App;
using EMSP.Mathematic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EMSP.UI.Dialogs.CalculationSettings
{
    public class InputFilter : MonoBehaviour
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
        public class RangeLengthCalculatedEvent : UnityEvent<InputFilter, int> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private InputField _inputField;
        #endregion

        #region Events
        public RangeLengthCalculatedEvent RangeLengthCalculated = new RangeLengthCalculatedEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void SetRangeLengthText(int rangeLength)
        {
            _inputField.text = Mathf.Pow(rangeLength, 3f).ToString();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void InputField_OnValueChanged(string data)
        {
            if (data.Length == 0)
            {
                return;
            }

            if (!char.IsDigit(data[data.Length - 1]))
            {
                _inputField.text = data.Substring(0, data.Length - 1);
            }
        }

        public void InputField_OnEndEdit(string data)
        {
            int pointsCount = 0;

            if (data.Length == 0)
            {
                pointsCount = (int)Mathf.Pow(GameSettings.Instance.CalculationMinRange, 3);
            }
            else
            {
                pointsCount = int.Parse(data);
            }

            if (pointsCount < GameSettings.Instance.CalculationMinRange)
            {
                pointsCount = (int)Mathf.Pow(GameSettings.Instance.CalculationMinRange, 3);
            }

            float baseValue = Mathf.Pow(pointsCount, 1f / 3f);

            int baseMin = (int)baseValue;
            int baseMax = (int)baseValue + 1;

            int minPointsCount = (int)Mathf.Pow(baseMin, 3f);
            int maxPointsCount = (int)Mathf.Pow(baseMax, 3f);

            int minDiff = pointsCount - minPointsCount;
            int maxDiff = maxPointsCount - pointsCount;

            int resultRangeLength = minDiff <= maxDiff ? baseMin : baseMax;

            if (resultRangeLength != MathematicManager.Instance.RangeLength)
            {
                RangeLengthCalculated.Invoke(this, resultRangeLength);
            }
            else
            {
                SetRangeLengthText(resultRangeLength);
            }
        }
        #endregion
        #endregion
    }
}