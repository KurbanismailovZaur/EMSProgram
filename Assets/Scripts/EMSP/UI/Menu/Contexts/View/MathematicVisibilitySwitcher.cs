using EMSP.Mathematic;
using EMSP.Mathematic.Magnetic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Menu.Contexts.View
{
	public class MathematicVisibilitySwitcher : MonoBehaviour 
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
        private Image _stateImage;

        [SerializeField]
        private CalculationType _calculationType;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void TrySwitchVisibility()
        {
            MathematicManager.Instance.SwitchVisibility(_calculationType);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void Button_OnClick()
        {
            TrySwitchVisibility();
        }

        public void MathematicBase_VisibilityChanged(MathematicBase mathematicBase, bool state)
        {
            _stateImage.enabled = state;
        }
        #endregion
        #endregion
    }
}