using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace EMSP.Environment.View
{
	public class Axis : MonoBehaviour 
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
        public class SelectedEvent : UnityEvent<Axis, AxisDirection> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private AxisDirection _direction;
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
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void EventTrigger_PointerClick(BaseEventData eventData)
        {
            Selected.Invoke(this, _direction);
        }
		#endregion
		#endregion
	}
}