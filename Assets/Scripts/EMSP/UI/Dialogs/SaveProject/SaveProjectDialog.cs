using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Dialogs.SaveProject
{
    [RequireComponent(typeof(CanvasGroup))]
	public class SaveProjectDialog : MonoBehaviour 
	{
        #region Entities
        #region Enums
        public enum Action
        {
            Save,
            DontSave,
            Cancel
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class ChosenEvent : UnityEvent<SaveProjectDialog, Action> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private CanvasGroup _canvasGroup;
        #endregion

        #region Events
        public ChosenEvent Chosen = new ChosenEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        public void ShowModal()
        {
            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            _canvasGroup.blocksRaycasts = true;
        }

        public void Hide()
        {
            _canvasGroup.alpha = 0f;
            _canvasGroup.interactable = false;
            _canvasGroup.blocksRaycasts = false;
        }

        public void ChoseSave()
        {
            Hide();

            Chosen.Invoke(this, Action.Save);
        }

        public void ChoseDontSave()
        {
            Hide();

            Chosen.Invoke(this, Action.DontSave);
        }

        public void ChoseCancel()
        {
            Hide();

            Chosen.Invoke(this, Action.Cancel);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void SaveButton_OnClick()
        {
            ChoseSave();
        }

        public void DontSaveButton_OnClick()
        {
            ChoseDontSave();
        }

        public void CancelButton_OnClick()
        {
            ChoseCancel();
        }

        public void CloseButton_OnClick()
        {
            ChoseCancel();
        }
        #endregion
        #endregion
    }
}