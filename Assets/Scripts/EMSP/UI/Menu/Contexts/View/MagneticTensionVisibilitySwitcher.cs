using EMSP.Mathematic;
using EMSP.Mathematic.Magnetic;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Menu.Contexts.View
{
	public class MagneticTensionVisibilitySwitcher : MonoBehaviour 
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
            MathematicManager.Instance.MagneticTension.IsVisible = !MathematicManager.Instance.MagneticTension.IsVisible;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void Button_OnClick()
        {
            TrySwitchVisibility();
        }

        public void MagneticTensionInSpace_Calculated(MathematicBase mathematicBase)
        {
            _stateImage.enabled = true;
        }

        public void MagneticTensionInSpace_Destroyed(MathematicBase mathematicBase)
        {
            _stateImage.enabled = false;
        }

        public void MagneticTensionInSpace_VisibilityChanged(MathematicBase mathematicBase, bool state)
        {
            _stateImage.enabled = state;
        }
        #endregion
        #endregion
    }
}