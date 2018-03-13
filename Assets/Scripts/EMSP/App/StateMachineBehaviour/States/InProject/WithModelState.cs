using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.App.StateMachineBehaviour.States.InProject
{
	public class WithModelState : State
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
        private Button _removeModelButton;

        [SerializeField]
        private Button _removeWiringButton;

        [SerializeField]
        private Button _viewModelIsVisibilityButton;

        [SerializeField]
        private Button _viewModelIsTransparentButton;

        [SerializeField]
        private Button _viewWiringIsVisibilityButton;
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
            _removeModelButton.interactable = true;
            _removeWiringButton.interactable = false;
            _viewModelIsVisibilityButton.interactable = true;
            _viewModelIsTransparentButton.interactable = true;
            _viewWiringIsVisibilityButton.interactable = false;

            ModelManager.Instance.ModelDestroyed.AddListener(ModelManager_ModelDestroyed);
            WiringManager.Instance.WiringCreated.AddListener(WiringManager_WiringCreated);
        }

        public override void OnExit()
        {
            ModelManager.Instance.ModelCreated.RemoveListener(ModelManager_ModelDestroyed);
            WiringManager.Instance.WiringCreated.RemoveListener(WiringManager_WiringCreated);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        private void ModelManager_ModelDestroyed(Model model)
        {
            _stateMachine.MoveToState("EmptyProject");
        }

        private void WiringManager_WiringCreated(Wiring wiring)
        {
            _stateMachine.MoveToState("WithModelAndWiring");
        }
        #endregion
        #endregion
    }
}