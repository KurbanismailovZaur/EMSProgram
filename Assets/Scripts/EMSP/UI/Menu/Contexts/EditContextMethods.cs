using Numba.UI.Menu;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Menu.Contexts
{
    public class EditContextMethods : ContextMethodsBase 
	{
        #region Entities
        #region Enums
        public enum ActionType
        {
            RemoveModel,
            RemoveWiring,
            RemoveMagneticTension,
            RemoveElectricField,
            RemoveInduction,
            EditWiring
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class SelectedEvent : UnityEvent<EditContextMethods, ActionType> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #endregion

        #region Events
        public SelectedEvent Selected = new SelectedEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void RemoveModel()
        {
            Selected.Invoke(this, ActionType.RemoveModel);
        }

        public void RemoveWiring()
        {
            Selected.Invoke(this, ActionType.RemoveWiring);
        }

        public void RemoveMagneticTensionInSpace()
        {
            Selected.Invoke(this, ActionType.RemoveMagneticTension);
        }

        public void RemoveElectricField()
        {
            Selected.Invoke(this, ActionType.RemoveElectricField);
        }

        public void RemoveInduction()
        {
            Selected.Invoke(this, ActionType.RemoveInduction);
        }

        public void EditWiring()
        {
            Selected.Invoke(this, ActionType.EditWiring);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}