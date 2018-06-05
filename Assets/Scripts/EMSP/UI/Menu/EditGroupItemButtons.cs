using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Menu
{
	public class EditGroupItemButtons : MonoBehaviour 
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
        private Button _removeModelButton;

        [SerializeField]
        private Button _removeWiringButton;

        [SerializeField]
        private Button _calculationRemoveMagneticTensionInSpaceButton;

        [SerializeField]
        private Button _calculationRemoveElectricFieldButton;

        [SerializeField]
        private Button _wiringButton;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Button RemoveModelButton { get { return _removeModelButton; } }

        public Button RemoveWiringButton { get { return _removeWiringButton; } }

        public Button CalculationRemoveMagneticTensionInSpaceButton { get { return _calculationRemoveMagneticTensionInSpaceButton; } }

        public Button CalculationRemoveElectricFieldButton { get { return _calculationRemoveElectricFieldButton; } }

        public Button WiringButton { get { return _wiringButton; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void SetAllButtonsInteractableTo(bool state)
        {
            _removeModelButton.interactable = state;
            _removeWiringButton.interactable = state;
            _calculationRemoveMagneticTensionInSpaceButton.interactable = state;
            _calculationRemoveElectricFieldButton.interactable = state;
            _wiringButton.interactable = state;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
