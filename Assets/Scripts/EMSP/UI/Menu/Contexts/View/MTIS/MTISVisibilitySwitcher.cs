using EMSP.Mathematic;
using EMSP.Mathematic.MagneticTension;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Menu.Contexts.View.MTIS
{
	public class MTISVisibilitySwitcher : MonoBehaviour 
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
            MathematicManager.Instance.MagneticTensionInSpace.IsVisible = !MathematicManager.Instance.MagneticTensionInSpace.IsVisible;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void Button_OnClick()
        {
            TrySwitchVisibility();
        }

        public void MagneticTensionInSpace_Calculated(MagneticTensionInSpace magneticTensionInSpace)
        {
            MathematicManager.Instance.MagneticTensionInSpace.VisibilityChanged.AddListener(MagneticTensionInSpace_VisibilityChanged);
            _stateImage.enabled = true;
        }

        public void MagneticTensionInSpace_Destroyed(MagneticTensionInSpace magneticTensionInSpace)
        {
            _stateImage.enabled = false;
        }

        public void MagneticTensionInSpace_VisibilityChanged(MagneticTensionInSpace magneticTensionInSpace, bool state)
        {
            _stateImage.enabled = state;
        }
        #endregion
        #endregion
    }
}