using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Menu.Contexts
{
	public class WindowContextMethods : ContextMethodsBase
    {
        #region Entities
        public enum ActionType
        {
            OpenProcessWindow
        }
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class SelectedEvent : UnityEvent<WindowContextMethods, ActionType> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        public SelectedEvent Selected = new SelectedEvent();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void OpenProcessesWindow()
        {
            Selected.Invoke(this, ActionType.OpenProcessWindow);
        }
		#endregion
		
		#region Indexers
		#endregion
			
		#region Events handlers
		#endregion
		#endregion
	}
}
