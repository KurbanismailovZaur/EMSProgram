using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using EMSP.Communication;
using System.Linq;

namespace EMSP.UI.Windows.WiringEditor
{
    public class WireButton : MonoBehaviour
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
        static WireButton _currentActiveButton = null;

        public Button DeleteWireButton;
        
        public Color SelectedButtonColor;
        public Color SelectedTextColor;

        public Color NormalButtonColor;
        public Color NormalTextColor;


        private Image _image = null;
        private InputField _inputFieldComponent = null;
        private RectTransform _rectTransform = null;
        private string _preEditName;
        private float _preEditWidth = -1;
        private WiringEditorWindow _wiringManager;

        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public string WireName
        {
            get { return _preEditName; }
            set
            {
                InputFieldComponent.text = value;
                _preEditName = value;
            }
        }

        public WiringEditorWindow WiringManager { get { return _wiringManager; } set { _wiringManager = value; } }

        public static WireButton CurrentActiveWireButton { get { return _currentActiveButton; } }

        public int WireNumber { get; set; }

        private Image ImageComponent
        {
            get
            {
                if (_image == null)
                    _image = GetComponent<Image>();
                return _image;
            }

            set { _image = value; }
        }

        private InputField InputFieldComponent
        {
            get
            {
                if (_inputFieldComponent == null)
                    _inputFieldComponent = GetComponentInChildren<InputField>();
                return _inputFieldComponent;
            }

            set { _inputFieldComponent = value; }
        }

        private RectTransform RectTransformComponent
        {
            get
            {
                if (_rectTransform == null)
                    _rectTransform = GetComponentInChildren<RectTransform>();
                return _rectTransform;
            }

            set { _rectTransform = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods

        private void Awake()
        {
            InputFieldComponent.onValueChanged.RemoveAllListeners();
            InputFieldComponent.onEndEdit.RemoveAllListeners();


            InputFieldComponent.onValueChanged.AddListener((str) =>
            {
                if (!Wire.IsCorrectName(str) && !string.IsNullOrEmpty(str))
                    InputFieldComponent.text = InputFieldComponent.text.Substring(0, InputFieldComponent.text.Length - 1);

                StartCoroutine(DelayAndCheckWidth());
            });

            InputFieldComponent.onEndEdit.AddListener((str) =>
            {
                if (string.IsNullOrEmpty(str) || !IsUniqName(str))
                {
                    InputFieldComponent.text = _preEditName;
                    RectTransformComponent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, _preEditWidth);
                }
                else
                {
                    if(str.Length < _preEditName.Length)
                    {
                        RectTransformComponent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, str.Length * 8);

                        while(InputFieldComponent.textComponent.text.Length < InputFieldComponent.text.Length)
                        {
                            CheckWidth();
                        }

                    }

                    WiringManager.WiresNames[WireNumber] = str;
                    WireName = str;
                    _preEditWidth = RectTransformComponent.rect.size.x;
                }
            });
        }

        private void Start()
        {
            if(_currentActiveButton != this)
            {
                ImageComponent.color = NormalButtonColor;
                InputFieldComponent.image.color = NormalButtonColor;
                InputFieldComponent.textComponent.color = NormalTextColor;
                InputFieldComponent.image.enabled = false;
                InputFieldComponent.enabled = false;
                DeleteWireButton.gameObject.SetActive(false);
            }
        }

        private bool IsUniqName(string name)
        {
            foreach(string _name in WiringManager.WiresNames.Values.ToList())
            {
                if (_name == name)
                    return false;
            }

            return true;
        }

        private void CheckWidth()
        {
            RectTransformComponent.ForceUpdateRectTransforms();
            if (InputFieldComponent.textComponent.text.Length < InputFieldComponent.text.Length)
                RectTransformComponent.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, RectTransformComponent.rect.size.x + 15);
        }

        private IEnumerator DelayAndCheckWidth()
        {
            yield return null;

            while (InputFieldComponent.textComponent.text.Length < InputFieldComponent.text.Length)
            {
                CheckWidth();
            }

            if (_preEditWidth == -1)
                _preEditWidth = RectTransformComponent.rect.size.x;
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public static void OnEditorClosing()
        {
            _currentActiveButton = null;
        }

        public void OnClick()
        {
            ImageComponent.color = SelectedButtonColor;
            InputFieldComponent.image.color = SelectedButtonColor;
            InputFieldComponent.textComponent.color = SelectedTextColor;

            InputFieldComponent.image.enabled = true;
            InputFieldComponent.enabled = true;

            DeleteWireButton.gameObject.SetActive(true);


            if (_currentActiveButton != null)
                _currentActiveButton.OnDifferentWireButtonClick();

            _currentActiveButton = this;
        }

        public void OnDifferentWireButtonClick()
        {
            ImageComponent.color = NormalButtonColor;
            InputFieldComponent.enabled = false;
            InputFieldComponent.image.enabled = false;
            InputFieldComponent.image.color = NormalButtonColor;
            InputFieldComponent.textComponent.color = NormalTextColor;

            DeleteWireButton.gameObject.SetActive(false);
        }

        #endregion
        #endregion
    }
}
