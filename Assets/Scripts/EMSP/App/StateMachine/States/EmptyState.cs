using EMSP.App;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.App.StateMachine.States
{
	public class EmptyState : GameState 
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
            _saveProjectButton.interactable = false;
            _closeProjectButton.interactable = false;
            _importModelButton.interactable = false;
            _importWiringButton.interactable = false;

            GameFacade.Instance.DeactivateProjectEnvironment();

            ProjectManager.Instance.ProjectCreated.AddListener(ProjectManager_ProjectCreated);
        }

        private void MoveToDefaultState()
        {
            _game.MoveToDefaultState();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        private void ProjectManager_ProjectCreated(Project project)
        {
            ProjectManager.Instance.ProjectCreated.RemoveListener(ProjectManager_ProjectCreated);

            MoveToDefaultState();
        }
        #endregion
        #endregion
    }
}