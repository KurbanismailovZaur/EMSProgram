using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Dialogs.WiringEditor
{
    public class PointEditPanel : WiringEditorDialog
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
        [ContextMenu("Test")]
        void Test()
        {
            _wiring.Add(0, new List<Vector3>());
            _wiring[0].Add(new Vector3(0.14262f, 45.4377f, 3.4543f));

            Initialize(0, 0);
        }

        public void Initialize(int wireNumber, int pointNumber)
        {
            Vector3 point = _wiring[wireNumber][pointNumber];
            _currentValue = point;
            PointNumberField.text = pointNumber.ToString();
            X.text = point.x.ToString();
            Y.text = point.y.ToString();
            Z.text = point.z.ToString();

            X.onValueChanged.AddListener((c) =>
            {
                _currentValue.x = float.Parse(c);
                _wiring[wireNumber][pointNumber] = _currentValue;
            });

            Y.onValueChanged.AddListener((c) =>
            {
                _currentValue.y = float.Parse(c);
                _wiring[wireNumber][pointNumber] = _currentValue;
            });

            Z.onValueChanged.AddListener((c) =>
            {
                _currentValue.z = float.Parse(c);
                _wiring[wireNumber][pointNumber] = _currentValue;
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
