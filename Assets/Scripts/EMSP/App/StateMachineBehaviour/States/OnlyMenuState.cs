using EMSP.App;
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
        private Button _fileSaveProjectButton;

        [SerializeField]
        private Button _fileCloseProjectButton;

        [SerializeField]
        private Button _fileImportModelButton;

        [SerializeField]
        private Button _fileImportWiringButton;

        [SerializeField]
        private Button _editRemoveModelButton;

        [SerializeField]
        private Button _editRemoveWiringButton;

        [SerializeField]
        private Button _viewModelIsVisibilityButton;

        [SerializeField]
        private Button _viewModelIsTransparentButton;

        [SerializeField]
        private Button _viewWiringIsVisibilityButton;

        [SerializeField]
        private Button _viewGridVisibilityButton;
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
            _fileSaveProjectButton.interactable = false;
            _fileCloseProjectButton.interactable = false;
            _fileImportModelButton.interactable = false;
            _fileImportWiringButton.interactable = false;
            _editRemoveModelButton.interactable = false;
            _editRemoveWiringButton.interactable = false;
            _viewModelIsVisibilityButton.interactable = false;
            _viewModelIsTransparentButton.interactable = false;
            _viewWiringIsVisibilityButton.interactable = false;
            _viewGridVisibilityButton.interactable = false;

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