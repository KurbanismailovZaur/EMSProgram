using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Menu
{
	public class ViewGroupItemButtons : MonoBehaviour 
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
        private Button _modelIsVisibilityButton;

        [SerializeField]
        private Button _modelIsTransparentButton;

        [SerializeField]
        private Button _wiringIsVisibilityButton;

        [SerializeField]
        private Button _computationMagneticTensionInSpaceIsVisibleButton;

        [SerializeField]
        private Button _gridVisibilityButton;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Button ModelIsVisibilityButton { get { return _modelIsVisibilityButton; } }

        public Button ModelIsTransparentButton { get { return _modelIsTransparentButton; } }

        public Button WiringIsVisibilityButton { get { return _wiringIsVisibilityButton; } }

        public Button ComputationMagneticTensionInSpaceIsVisibleButton { get { return _computationMagneticTensionInSpaceIsVisibleButton; } }

        public Button GridVisibilityButton { get { return _gridVisibilityButton; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void SetAllButtonsInteractableTo(bool state)
        {
            _modelIsVisibilityButton.interactable = state;
            _modelIsTransparentButton.interactable = state;
            _wiringIsVisibilityButton.interactable = state;
            _computationMagneticTensionInSpaceIsVisibleButton.interactable = state;
            _gridVisibilityButton.interactable = state;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
