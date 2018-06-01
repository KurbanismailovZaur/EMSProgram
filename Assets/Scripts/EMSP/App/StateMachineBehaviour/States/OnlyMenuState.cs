using EMSP.App;
using EMSP.UI.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.App.StateMachineBehaviour.States
{
	public class OnlyMenuState : State 
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
        [Header("Menus")]
        [SerializeField]
        private FileGroupItemButtons _fileGroupItemButtons;

        [SerializeField]
        private EditGroupItemButtons _editGroupItemButtons;

        [SerializeField]
        private Button _viewModelIsVisibilityButton;

        [SerializeField]
        private Button _viewModelIsTransparentButton;

        [SerializeField]
        private Button _viewWiringIsVisibilityButton;

        [SerializeField]
        private Button _viewComputationMagneticTensionInSpaceIsVisibleButton;

        [SerializeField]
        private Button _viewGridVisibilityButton;

        [SerializeField]
        private Button _calculationsComputationMagneticTensionInSpaceButton;

        [SerializeField]
        private Button _calculationsSettingButton;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public override void OnEnter()
        {
            _fileGroupItemButtons.SetAllButtonsInteractableTo(false);
            _editGroupItemButtons.SetAllButtonsInteractableTo(false);

            _viewModelIsVisibilityButton.interactable = false;
            _viewModelIsTransparentButton.interactable = false;
            _viewWiringIsVisibilityButton.interactable = false;
            _viewComputationMagneticTensionInSpaceIsVisibleButton.interactable = false;
            _viewGridVisibilityButton.interactable = false;

            _calculationsComputationMagneticTensionInSpaceButton.interactable = false;
            _calculationsSettingButton.interactable = false;

            GameFacade.Instance.DeactivateProjectEnvironment();

            ProjectManager.Instance.ProjectCreated.AddListener(ProjectManager_ProjectCreated);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        private void ProjectManager_ProjectCreated(Project project)
        {
            ProjectManager.Instance.ProjectCreated.RemoveListener(ProjectManager_ProjectCreated);

            _stateMachine.MoveToState("InProject");
        }
        #endregion
        #endregion
    }
}