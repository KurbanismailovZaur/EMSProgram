using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EMSP.UI.Windows.CalculationSettings
{
    [RequireComponent(typeof(RawImage))]
    public class Tab : MonoBehaviour
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
        public class ClickedEvent : UnityEvent<Tab> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private RawImage _image;

        private bool _isSelected;

        [SerializeField]
        private Panel _associatedPanel;
        #endregion

        #region Events
        public ClickedEvent Clicked = new ClickedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public Color Color
        {
            get { return _image.color; }
            set { _image.color = value; }
        }

        public Panel AssociatedPanel { get { return _associatedPanel; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            _image = GetComponent<RawImage>();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void Button_OnClick()
        {
            Clicked.Invoke(this);
        }
        #endregion
        #endregion
    }
}