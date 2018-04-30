﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.UI.Toggle
{
	public class ToggleReactor : MonoBehaviour 
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
        private Behaviour _behaviour;
		#endregion
		
		#region Events
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
        public void ToggleBehaviour_StateChanged(ToggleBehaviour toggleBehaviour, bool state)
        {
            _behaviour.enabled = state;
        }
		#endregion
		#endregion
	}
}