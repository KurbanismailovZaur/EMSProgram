using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Dialogs.WiringEditor
{
    public class PointEditPanel : MonoBehaviour
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

        public Text PointNumberField;
        public InputField X;
        public InputField Y;
        public InputField Z;

        private Vector3 _currentValue;

        #endregion

        #region Events
        #endregion

        #region Behaviour

        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        public void Initialize(int wireNumber, int pointNumber)
        {
            X.contentType = InputField.ContentType.DecimalNumber;
            Y.contentType = InputField.ContentType.DecimalNumber;
            Z.contentType = InputField.ContentType.DecimalNumber;

            Vector3 point = WiringEditorDialog.Instance.Wiring[wireNumber][pointNumber];
            _currentValue = point;
            PointNumberField.text = pointNumber.ToString();
            X.text = point.x.ToString();
            Y.text = point.y.ToString();
            Z.text = point.z.ToString();

            X.onValueChanged.AddListener((str) =>
            {
                float newX;
                if (float.TryParse(str, out newX))
                {
                    _currentValue.x = newX;
                    WiringEditorDialog.Instance.Wiring[wireNumber][pointNumber] = _currentValue;
                }
            });

            X.onEndEdit.AddListener((str) =>
            {
                if(string.IsNullOrEmpty(str) || str == "-")
                {
                    _currentValue.x = 0;
                    WiringEditorDialog.Instance.Wiring[wireNumber][pointNumber] = _currentValue;
                    X.text = 0.ToString();
                }
            });

            Y.onValueChanged.AddListener((str) =>
            {
                float newY;
                if (float.TryParse(str, out newY))
                {
                    _currentValue.y = newY;
                    WiringEditorDialog.Instance.Wiring[wireNumber][pointNumber] = _currentValue;
                }
            });

            Y.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    _currentValue.y = 0;
                    WiringEditorDialog.Instance.Wiring[wireNumber][pointNumber] = _currentValue;
                    Y.text = 0.ToString();
                }
            });

            Z.onValueChanged.AddListener((str) =>
            {
                float newZ;
                if (float.TryParse(str, out newZ))
                {
                    _currentValue.z = newZ;
                    WiringEditorDialog.Instance.Wiring[wireNumber][pointNumber] = _currentValue;
                }
            });


            Z.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    _currentValue.z = 0;
                    WiringEditorDialog.Instance.Wiring[wireNumber][pointNumber] = _currentValue;
                    Z.text = 0.ToString();
                }
            });

        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
