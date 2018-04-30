using EMSP.Mathematic;
using EMSP.UI.Toggle;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Menu.Contexts.View
{
	public class AmperageModeToggleGroupHandler : MonoBehaviour 
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
        private ToggleGroup _toggleGroup;

        [SerializeField]
        private ToggleBehaviour _computationalToggleBehaviour;

        [SerializeField]
        private ToggleBehaviour _precomputedToggleBehaviour;
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
        public void ToggleGroup_TryToToggleBehaviourSelecting(ToggleGroup toggleGroup, ToggleBehaviour toggleBehaviour)
        {
            if (toggleBehaviour == _computationalToggleBehaviour)
            {
                MathematicManager.Instance.AmperageMode = AmperageMode.Computational;
            }
            else
            {
                MathematicManager.Instance.AmperageMode = AmperageMode.Precomputed;
            }
        }

        public void MathematicManager_AmperageModeChanged(MathematicManager mathematicManager, AmperageMode amperageMode)
        {
            _toggleGroup.SelectToggleBehaviour(amperageMode == AmperageMode.Computational ? _computationalToggleBehaviour : _precomputedToggleBehaviour);
        }
		#endregion
		#endregion
	}
}