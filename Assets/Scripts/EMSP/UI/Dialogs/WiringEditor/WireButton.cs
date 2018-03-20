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
        
        public Color SelectedColor;
        public Color NormalColor;

        private Image _image = null;
        private Image image
        {
            get
            {
                if(_image == null)
                    _image = GetComponent<Image>();
                return _image;
            }

            set { _image = value; }
        }
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public int WireNumber { get; set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods

        public static void OnEditorClosing()
        {
            _currentActiveButton = null;
        }

        public void OnClick()
        {
            image.color = SelectedColor;
            if (_currentActiveButton != null)
                _currentActiveButton.OnDifferentWireButtonClick();

            _currentActiveButton = this;
        }

        public void OnDifferentWireButtonClick()
        {
            image.color = NormalColor;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
