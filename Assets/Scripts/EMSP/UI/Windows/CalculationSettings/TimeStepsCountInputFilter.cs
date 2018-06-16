﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Windows.CalculationSettings
{
	public class TimeStepsCountInputFilter : MonoBehaviour 
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
        private InputField _inputField;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public int StepsCount { get { return int.Parse(_inputField.text); } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void SetTimeStepsCountText(int timeStepsCount)
        {
            _inputField.text = timeStepsCount.ToString();
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

            char lastSymbol = data[data.Length - 1];
            if (!char.IsDigit(lastSymbol))
            {
                _inputField.text = data.Substring(0, data.Length - 1);
            }
        }

        public void InputField_OnEndEdit(string data)
        {
            if (data.Length == 0)
            {
                _inputField.text = "3";
                return;
            }

            int timeSteps = Mathf.Max(int.Parse(data), 3);
            _inputField.text = timeSteps.ToString();
        }
        #endregion
        #endregion
    }
}