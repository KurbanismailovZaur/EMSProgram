using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Menu
{
	public class FileGroupItemButtons : MonoBehaviour 
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
        private Button _saveProjectButton;

        [SerializeField]
        private Button _closeProjectButton;

        [SerializeField]
        private Button _importModelButton;

        [SerializeField]
        private Button _importWiringButton;

        [SerializeField]
        private Button _exportMagneticTensionInSpace;

        [SerializeField]
        private Button _exportWiring;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Button SaveProjectButton { get { return _saveProjectButton; } }

        public Button CloseProjectButton { get { return _closeProjectButton; } }

        public Button ImportModelButton { get { return _importModelButton; } }

        public Button ImportWiringButton { get { return _importWiringButton; } }

        public Button ExportMagneticTensionInSpace { get { return _exportMagneticTensionInSpace; } }

        public Button ExportWiring { get { return _exportWiring; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void SetAllButtonsInteractableTo(bool state)
        {
            _saveProjectButton.interactable = state;
            _closeProjectButton.interactable = state;
            _importModelButton.interactable = state;
            _importWiringButton.interactable = state;
            _exportMagneticTensionInSpace.interactable = state;
            _exportWiring.interactable = state;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
