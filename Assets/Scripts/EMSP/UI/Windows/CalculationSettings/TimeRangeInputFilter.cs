using EMSP.App;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Windows.CalculationSettings
{
    public class TimeRangeInputFilter : MonoBehaviour
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
        public float Time { get { return float.Parse(_inputField.text); } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void SetTimeText(float time)
        {
            _inputField.text = time.ToString();
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
            if ((!char.IsDigit(lastSymbol) && lastSymbol.ToString() != GameSettings.Instance.NumberDecimalSeparator) 
                || (lastSymbol.ToString() == GameSettings.Instance.NumberDecimalSeparator && data.Count(c => c == GameSettings.Instance.NumberDecimalSeparator[0]) > 1))
            {
                _inputField.text = data.Substring(0, data.Length - 1);
            }
        }

        public void InputField_OnEndEdit(string data)
        {
            _inputField.text = data.Length == 0 ? "0" : Mathf.Max(float.Parse(data), 0f).ToString();
        }
        #endregion
        #endregion
    }
}