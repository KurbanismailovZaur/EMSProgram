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
        public Button DeletePointButton;
        public InputField X;
        public InputField Y;
        public InputField Z;

        private int _currentPointNumber;
        private Vector3 _currentValue;
        public Vector3 CurrentValue { get { return _currentValue; } }


        #endregion

        #region Events
        #endregion

        #region Behaviour

        #region Properties

        public Selectable UpSelectable
        {
            get { return X; }
            set
            {
                var navigation = X.navigation;
                navigation.selectOnUp = value;
                X.navigation = navigation;
            }
        }

        public Selectable DownSelectable
        {
            get { return Z; }
            set
            {
                var navigation = Z.navigation;
                navigation.selectOnDown = value;
                Z.navigation = navigation;
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods

        public void Initialize(int wireNumber, int pointNumber)
        {
            X.contentType = InputField.ContentType.DecimalNumber;
            Y.contentType = InputField.ContentType.DecimalNumber;
            Z.contentType = InputField.ContentType.DecimalNumber;
            _currentPointNumber = pointNumber;

            Vector3 point = WiringEditorDialog.Instance.Wiring[wireNumber][_currentPointNumber];
            _currentValue = point;

            StartCoroutine(UpdatePointNumberAndSelectable());

            X.text = point.x.ToString();
            Y.text = point.y.ToString();
            Z.text = point.z.ToString();

            X.onValueChanged.AddListener((str) =>
            {
                float newX;
                if (float.TryParse(str, out newX))
                {
                    _currentValue.x = newX;
                    WiringEditorDialog.Instance.Wiring[wireNumber][_currentPointNumber] = _currentValue;
                }
            });

            X.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    _currentValue.x = 0;
                    WiringEditorDialog.Instance.Wiring[wireNumber][_currentPointNumber] = _currentValue;
                    X.text = 0.ToString();
                }
            });

            Y.onValueChanged.AddListener((str) =>
            {
                float newY;
                if (float.TryParse(str, out newY))
                {
                    _currentValue.y = newY;
                    WiringEditorDialog.Instance.Wiring[wireNumber][_currentPointNumber] = _currentValue;
                }
            });

            Y.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    _currentValue.y = 0;
                    WiringEditorDialog.Instance.Wiring[wireNumber][_currentPointNumber] = _currentValue;
                    Y.text = 0.ToString();
                }
            });

            Z.onValueChanged.AddListener((str) =>
            {
                float newZ;
                if (float.TryParse(str, out newZ))
                {
                    _currentValue.z = newZ;
                    WiringEditorDialog.Instance.Wiring[wireNumber][_currentPointNumber] = _currentValue;
                }
            });


            Z.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    _currentValue.z = 0;
                    WiringEditorDialog.Instance.Wiring[wireNumber][_currentPointNumber] = _currentValue;
                    Z.text = 0.ToString();
                }
            });

        }

        public void UpdatePointNumberAndSelectableImmediate()
        {
            int sibIndex = GetComponent<RectTransform>().GetSiblingIndex();

            PointNumberField.text = sibIndex.ToString();
            _currentPointNumber = sibIndex;

            if (sibIndex != 0)
            {
                transform.parent.GetChild(sibIndex - 1).GetComponent<PointEditPanel>().DownSelectable = UpSelectable;
                UpSelectable = transform.parent.GetChild(sibIndex - 1).GetComponent<PointEditPanel>().DownSelectable;
            }
        }

        public IEnumerator UpdatePointNumberAndSelectable()
        {
            yield return null;

            UpdatePointNumberAndSelectableImmediate();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
