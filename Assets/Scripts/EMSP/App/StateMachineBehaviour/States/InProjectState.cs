﻿using EMSP.Communication;
using EMSP.Mathematic;
using EMSP.Mathematic.Magnetic;
using EMSP.UI.Menu;
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
            _fileGroupItemButtons.SaveProjectButton.interactable = true;
            _fileGroupItemButtons.CloseProjectButton.interactable = true;
            _fileGroupItemButtons.ImportModelButton.interactable = true;
            _fileGroupItemButtons.ImportWiringButton.interactable = true;

            _editGroupItemButtons.WiringButton.interactable = true;
            _viewGroupItemButtons.GridVisibilityButton.interactable = true;
            _calculationsGroupItemButtons.SettingButton.interactable = true;

            GameFacade.Instance.InitializeActivateProjectEnvironment();

            ProjectManager.Instance.ProjectDestroyed.AddListener(ProjectManager_ProjectDestroyed);

            ModelManager.Instance.ModelCreated.AddListener(ModelManager_ModelCreated);
            ModelManager.Instance.ModelDestroyed.AddListener(ModelManager_ModelDestroyed);

            WiringManager.Instance.WiringCreated.AddListener(WiringManager_WiringCreated);
            WiringManager.Instance.WiringDestroyed.AddListener(WiringManager_WiringDestroyed);

            MathematicManager.Instance.MagneticTensionInSpace.Calculated.AddListener(MagneticTensionInSpace_Calculated);
            MathematicManager.Instance.MagneticTensionInSpace.Destroyed.AddListener(MagneticTensionInSpace_Destroyed);
        }

        public override void OnExit()
        {
            ProjectManager.Instance.ProjectDestroyed.RemoveListener(ProjectManager_ProjectDestroyed);

            ModelManager.Instance.ModelCreated.RemoveListener(ModelManager_ModelCreated);
            ModelManager.Instance.ModelDestroyed.RemoveListener(ModelManager_ModelDestroyed);

            WiringManager.Instance.WiringCreated.RemoveListener(WiringManager_WiringCreated);
            WiringManager.Instance.WiringDestroyed.RemoveListener(WiringManager_WiringDestroyed);

            MathematicManager.Instance.MagneticTensionInSpace.Calculated.RemoveListener(MagneticTensionInSpace_Calculated);
            MathematicManager.Instance.MagneticTensionInSpace.Destroyed.RemoveListener(MagneticTensionInSpace_Destroyed);
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
            _editGroupItemButtons.RemoveModelButton.interactable = true;
            _viewGroupItemButtons.ModelIsVisibilityButton.interactable = true;
            _viewGroupItemButtons.ModelIsTransparentButton.interactable = true;
        }

        private void ModelManager_ModelDestroyed(Model model)
        {
            _editGroupItemButtons.RemoveModelButton.interactable = false;
            _viewGroupItemButtons.ModelIsVisibilityButton.interactable = false;
            _viewGroupItemButtons.ModelIsTransparentButton.interactable = false;
        }

        private void WiringManager_WiringCreated(Wiring wiring)
        {
            _fileGroupItemButtons.ExportWiring.interactable = true;
            _editGroupItemButtons.RemoveWiringButton.interactable = true;
            _viewGroupItemButtons.WiringIsVisibilityButton.interactable = true;
            _calculationsGroupItemButtons.ComputationMagneticTensionInSpaceButton.interactable = true;
            _calculationsGroupItemButtons.ElectricFieldButton.interactable = true;
        }

        private void WiringManager_WiringDestroyed(Wiring wiring)
        {
            _fileGroupItemButtons.ExportWiring.interactable = false;
            _editGroupItemButtons.RemoveWiringButton.interactable = false;
            _viewGroupItemButtons.WiringIsVisibilityButton.interactable = false;
            _calculationsGroupItemButtons.ComputationMagneticTensionInSpaceButton.interactable = false;
            _calculationsGroupItemButtons.ElectricFieldButton.interactable = false;
        }

        private void MagneticTensionInSpace_Calculated(MagneticTensionInSpace magneticTensionInSpace)
        {
            _fileGroupItemButtons.ExportMagneticTensionInSpace.interactable = true;
            _editGroupItemButtons.CalculationRemoveMagneticTensionInSpaceButton.interactable = true;
            _viewGroupItemButtons.ComputationMagneticTensionInSpaceIsVisibleButton.interactable = true;
        }

        private void MagneticTensionInSpace_Destroyed(MagneticTensionInSpace magneticTensionInSpace)
        {
            _fileGroupItemButtons.ExportMagneticTensionInSpace.interactable = false;
            _editGroupItemButtons.CalculationRemoveMagneticTensionInSpaceButton.interactable = false;
            _viewGroupItemButtons.ComputationMagneticTensionInSpaceIsVisibleButton.interactable = false;
        }
        #endregion
        #endregion
    }
}