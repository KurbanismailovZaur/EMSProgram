using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.App.StateMachineBehaviour.States
{
    public class InProjectState : State
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
        private Button _saveProjectButton;

        [SerializeField]
        private Button _closeProjectButton;

        [SerializeField]
        private Button _importModelButton;

        [SerializeField]
        private Button _importWiringButton;
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
            _saveProjectButton.interactable = true;
            _closeProjectButton.interactable = true;
            _importModelButton.interactable = true;
            _importWiringButton.interactable = true;

            GameFacade.Instance.ActivateProjectEnvironment();

            ProjectManager.Instance.ProjectDestroyed.AddListener(ProjectManager_ProjectDestroyed);

            _stateMachine.MoveToState("EmptyProject");
        }

        public override void OnExit()
        {
            ProjectManager.Instance.ProjectDestroyed.RemoveListener(ProjectManager_ProjectDestroyed);

            _stateMachine.ExitFromCurrentState();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void ProjectManager_ProjectDestroyed(Project project)
        {
            _stateMachine.ParentStateMachine.MoveToState("OnlyMenu");
        }
        #endregion
        #endregion
    }
}