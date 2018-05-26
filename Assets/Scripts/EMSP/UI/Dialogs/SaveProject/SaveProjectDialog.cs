using EMSP.UI.Dialogs;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Dialogs.SaveProject
{
	public class SaveProjectDialog : ModalDialog 
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
        private Action<Action> _callback;
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
        public void ShowModal(Action<Action> callback)
        {
            _callback = callback;
            ShowModal();
        }

        private void InvokeCallback(Action action)
        {
            if (_callback != null)
            {
                _callback.Invoke(action);
            }
        }

        public void ChoseSave()
        {
            Hide();

            InvokeCallback(Action.Save);

            Chosen.Invoke(this, Action.Save);
        }

        public void ChoseDontSave()
        {
            Hide();

            InvokeCallback(Action.DontSave);

            Chosen.Invoke(this, Action.DontSave);
        }

        public void ChoseCancel()
        {
            Hide();

            InvokeCallback(Action.Cancel);

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