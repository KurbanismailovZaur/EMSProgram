using EMSP.UI.Windows.SaveProject;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.UI.Windows
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class ModalWindow : MonoBehaviour 
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
        private CanvasGroup _canvasGroup;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public bool IsShowing { get; private set; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public virtual void ShowModal()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;

            IsShowing = true;
        }

        public virtual void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;

            IsShowing = false;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}