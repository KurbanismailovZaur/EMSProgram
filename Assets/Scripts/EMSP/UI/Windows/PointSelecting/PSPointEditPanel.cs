using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Windows.PointSelecting
{
    public class PSPointEditPanel : MonoBehaviour
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

        public void Initialize(PointSelectingWindow wiringManager, Vector3 point)
        {
            X.contentType = InputField.ContentType.DecimalNumber;
            Y.contentType = InputField.ContentType.DecimalNumber;
            Z.contentType = InputField.ContentType.DecimalNumber;

            _currentValue = point;

            StartCoroutine(UpdatePointNumberAndSelectable());

            X.text = _currentValue.x.ToString();
            Y.text = _currentValue.y.ToString();
            Z.text = _currentValue.z.ToString();

            X.onValueChanged.AddListener((str) =>
            {
                float newX;
                if (float.TryParse(str, out newX))
                {
                    _currentValue.x = newX;
                }
            });

            X.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    _currentValue.x = 0;
                    X.text = 0.ToString();
                }
            });

            Y.onValueChanged.AddListener((str) =>
            {
                float newY;
                if (float.TryParse(str, out newY))
                {
                    _currentValue.y = newY;
                }
            });

            Y.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    _currentValue.y = 0;
                    Y.text = 0.ToString();
                }
            });

            Z.onValueChanged.AddListener((str) =>
            {
                float newZ;
                if (float.TryParse(str, out newZ))
                {
                    _currentValue.z = newZ;
                }
            });


            Z.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || str == "-")
                {
                    _currentValue.z = 0;
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
                transform.parent.GetChild(sibIndex - 1).GetComponent<PSPointEditPanel>().DownSelectable = UpSelectable;
                UpSelectable = transform.parent.GetChild(sibIndex - 1).GetComponent<PSPointEditPanel>().DownSelectable;
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
