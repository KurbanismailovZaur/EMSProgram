using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Dialogs.WiringEditor
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
        
        public Color SelectedButtonColor;
        public Color SelectedTextColor;

        public Color NormalButtonColor;
        public Color NormalTextColor;


        private Image _image = null;
        private Text _textComponent = null;

        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
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

        private Text TextComponent
        {
            get
            {
                if (_textComponent == null)
                    _textComponent = GetComponentInChildren<Text>();
                return _textComponent;
            }

            set { _textComponent = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods


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
            TextComponent.color = SelectedTextColor;

            if (_currentActiveButton != null)
                _currentActiveButton.OnDifferentWireButtonClick();

            _currentActiveButton = this;
        }

        public void OnDifferentWireButtonClick()
        {
            ImageComponent.color = NormalButtonColor;
            TextComponent.color = NormalTextColor;
        }
        #endregion
        #endregion
    }
}
