using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Environment.View
{
	public class ViewCube : MonoBehaviour 
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
        public class AxisSelectedEvent : UnityEvent<ViewCube, AxisDirection> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #endregion

        #region Events
        public AxisSelectedEvent AxisSelected = new AxisSelectedEvent();
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
        public void Axis_Selected(Axis axis, AxisDirection direction)
        {
            AxisSelected.Invoke(this, direction);
        }
		#endregion
		#endregion
	}
}