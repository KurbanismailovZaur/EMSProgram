using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Menu.Contexts
{
    public class CalculationsContextMethods : ContextMethodsBase 
	{
        #region Entities
        #region Enums
        public enum ActionType
        {
            MagneticTensionInSpace
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class SelectedEvent : UnityEvent<CalculationsContextMethods, ActionType> { }
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
        public void CalculateMagneticTensionInSpace()
        {
            Selected.Invoke(this, ActionType.MagneticTensionInSpace);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}