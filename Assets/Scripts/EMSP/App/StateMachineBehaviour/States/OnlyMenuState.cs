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
        private ViewGroupItemButtons _viewGroupItemButtons;

        [SerializeField]
        private CalculationsGroupItemButtons _calculationsGroupItemButtons;
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

            _viewGroupItemButtons.SetAllButtonsInteractableTo(false);

            _calculationsGroupItemButtons.SetAllButtonsInteractableTo(false);

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