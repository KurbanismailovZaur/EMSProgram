using EMSP.Communication;
using System;
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
        private Button _fileSaveProjectButton;

        [SerializeField]
        private Button _fileCloseProjectButton;

        [SerializeField]
        private Button _fileImportModelButton;

        [SerializeField]
        private Button _fileImportWiringButton;

        [SerializeField]
        private Button _viewGridVisibilityButton;

        [SerializeField]
        private Button _removeModelButton;

        [SerializeField]
        private Button _removeWiringButton;

        [SerializeField]
        private Button _viewModelIsVisibilityButton;

        [SerializeField]
        private Button _viewModelIsTransparentButton;

        [SerializeField]
        private Button _viewWiringIsVisibilityButton;

        [SerializeField]
        private Button _calculationsComputationMagneticTensionInSpaceButton;
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
            _fileSaveProjectButton.interactable = true;
            _fileCloseProjectButton.interactable = true;
            _fileImportModelButton.interactable = true;
            _fileImportWiringButton.interactable = true;
            _viewGridVisibilityButton.interactable = true;

            GameFacade.Instance.InitializeActivateProjectEnvironment();

            ProjectManager.Instance.ProjectDestroyed.AddListener(ProjectManager_ProjectDestroyed);

            ModelManager.Instance.ModelCreated.AddListener(ModelManager_ModelCreated);
            ModelManager.Instance.ModelDestroyed.AddListener(ModelManager_ModelDestroyed);

            WiringManager.Instance.WiringCreated.AddListener(WiringManager_WiringCreated);
            WiringManager.Instance.WiringDestroyed.AddListener(WiringManager_WiringDestroyed);
        }

        public override void OnExit()
        {
            ProjectManager.Instance.ProjectDestroyed.RemoveListener(ProjectManager_ProjectDestroyed);

            ModelManager.Instance.ModelCreated.RemoveListener(ModelManager_ModelCreated);
            ModelManager.Instance.ModelDestroyed.RemoveListener(ModelManager_ModelDestroyed);

            WiringManager.Instance.WiringCreated.RemoveListener(WiringManager_WiringCreated);
            WiringManager.Instance.WiringDestroyed.RemoveListener(WiringManager_WiringDestroyed);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void ProjectManager_ProjectDestroyed(Project project)
        {
            _stateMachine.MoveToState("OnlyMenu");
        }

        private void ModelManager_ModelCreated(Model model)
        {
            _removeModelButton.interactable = true;
            _viewModelIsVisibilityButton.interactable = true;
            _viewModelIsTransparentButton.interactable = true;
        }

        private void ModelManager_ModelDestroyed(Model model)
        {
            _removeModelButton.interactable = false;
            _viewModelIsVisibilityButton.interactable = false;
            _viewModelIsTransparentButton.interactable = false;
        }

        private void WiringManager_WiringCreated(Wiring arg0)
        {
            _removeWiringButton.interactable = true;
            _viewWiringIsVisibilityButton.interactable = true;
            _calculationsComputationMagneticTensionInSpaceButton.interactable = true;
        }

        private void WiringManager_WiringDestroyed(Wiring arg0)
        {
            _removeWiringButton.interactable = false;
            _viewWiringIsVisibilityButton.interactable = false;
            _calculationsComputationMagneticTensionInSpaceButton.interactable = false;
        }
        #endregion
        #endregion
    }
}