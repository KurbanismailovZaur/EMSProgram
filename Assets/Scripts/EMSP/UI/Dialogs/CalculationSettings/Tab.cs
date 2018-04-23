using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Dialogs.CalculationSettings
{
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
        #endregion

        #region Events
        public ClickedEvent Clicked = new ClickedEvent();
		#endregion
		
		#region Behaviour
		#region Properties
		#endregion
		
		#region Constructors
		#endregion
		
		#region Methods
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