using EMSP.Mathematic;
using EMSP.UI.Toggle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.UI.ViewHotKeys
{
	public class AmperageButtons : MonoBehaviour 
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
        private ToggleBehaviour _staticToggleBehaviour;

        [SerializeField]
        private ToggleBehaviour _dynamicToggleBehaviour;
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
            if (toggleBehaviour == _staticToggleBehaviour)
            {
                MathematicManager.Instance.AmperageMode = AmperageMode.Precomputed;
            }
            else
            {
                MathematicManager.Instance.AmperageMode = AmperageMode.Computational;
            }
        }

        public void MathematicManager_AmperageModeChanged(MathematicManager mathematicManager, AmperageMode amperageMode)
        {
            _toggleGroup.SelectToggleBehaviour(amperageMode == AmperageMode.Computational ? _dynamicToggleBehaviour : _staticToggleBehaviour);
        }
        #endregion
        #endregion
    }
}