using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numba.UI.Menu
{
    public class Group : ContextContainer, IPointerEnterHandler
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
        private Panel _panel;

        [SerializeField]
        private Button _button;

        [SerializeField]
        private Context _context;

        private bool _showContextOnMouseEnter;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Panel Panel { get { return _panel; } }

        public Button Button { get { return _button; } }

        public Context Context { get { return _context; } }

        public bool ShowContextOnMouseEnter
        {
            get { return _showContextOnMouseEnter; }
            set { _showContextOnMouseEnter = value; }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void ShowContext()
        {
            _context.Show();
        }

        public void HideContext()
        {
            _context.Hide();
        }

        public void ToggleContext()
        {
            if (!_context.IsShowed)
            {
                ShowContext();
            }
            else
            {
                HideContext();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_showContextOnMouseEnter)
            {
                ShowContext();
            }
        }
        #endregion

        #region Events handlers
        public void Button_OnClick()
        {
            ToggleContext();
        }
        #endregion
        #endregion
    }
}