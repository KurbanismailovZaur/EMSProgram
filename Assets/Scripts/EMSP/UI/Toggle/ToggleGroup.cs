using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Toggle
{
	public class ToggleGroup : MonoBehaviour 
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
        public class TryToToggleBehaviourSelectingEvent : UnityEvent<ToggleGroup, ToggleBehaviour> { }

        [Serializable]
        public class ToggleBehaviourChangedEvent : UnityEvent<ToggleGroup, ToggleBehaviour> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private ToggleBehaviour _selectedToggleBehaviour;

        [SerializeField]
        private bool _allowSelfSelect;
        #endregion

        #region Events
        public TryToToggleBehaviourSelectingEvent TryToToggleBehaviourSelecting = new TryToToggleBehaviourSelectingEvent();

        public ToggleBehaviourChangedEvent ToggleBehaviourChanged = new ToggleBehaviourChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void ResolveSelectToggleBehaviour(ToggleBehaviour toggleBehaviour)
        {
            if (toggleBehaviour == _selectedToggleBehaviour)
            {
                return;
            }

            if (_allowSelfSelect)
            {
                SelectToggleBehaviour(toggleBehaviour);
            }
            else
            {
                TryToToggleBehaviourSelecting.Invoke(this, toggleBehaviour);
            }
        }

        public void SelectToggleBehaviour(ToggleBehaviour toggleBehaviour)
        {
            _selectedToggleBehaviour.State = false;

            _selectedToggleBehaviour = toggleBehaviour;
            _selectedToggleBehaviour.State = true;

            ToggleBehaviourChanged.Invoke(this, _selectedToggleBehaviour);
        }
		#endregion
		
		#region Indexers
		#endregion
		
		#region Events handlers
        public void ToggleBehaviour_TryToStateSwitching(ToggleBehaviour toggleBehaviour)
        {
            ResolveSelectToggleBehaviour(toggleBehaviour);
        }
		#endregion
		#endregion
	}
}