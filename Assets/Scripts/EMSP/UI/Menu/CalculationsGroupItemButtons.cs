using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Menu
{
	public class CalculationsGroupItemButtons : MonoBehaviour 
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
        private Button _computationMagneticTensionInSpaceButton;

        [SerializeField]
        private Button _ElectricFieldButton;

        [SerializeField]
        private Button _settingButton;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Button ComputationMagneticTensionInSpaceButton { get { return _computationMagneticTensionInSpaceButton; } }

        public Button ElectricFieldButton { get { return _ElectricFieldButton; } }

        public Button SettingButton { get { return _settingButton; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void SetAllButtonsInteractableTo(bool state)
        {
            _computationMagneticTensionInSpaceButton.interactable = state;
            _ElectricFieldButton.interactable = state;
            _settingButton.interactable = state;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
