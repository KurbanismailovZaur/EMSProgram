using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.App.StateMachineBehaviour.States.InProject
{
	public class WithWiringState : State
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
            _removeModelButton.interactable = false;
            _removeWiringButton.interactable = true;
            _viewModelIsVisibilityButton.interactable = false;
            _viewModelIsTransparentButton.interactable = false;
            _viewWiringIsVisibilityButton.interactable = true;

            WiringManager.Instance.WiringDestroyed.AddListener(WiringManager_WiringDestroyed);
            ModelManager.Instance.ModelCreated.AddListener(ModelManager_ModelCreated);
        }

        public override void OnExit()
        {
            WiringManager.Instance.WiringDestroyed.RemoveListener(WiringManager_WiringDestroyed);
            ModelManager.Instance.ModelCreated.RemoveListener(ModelManager_ModelCreated);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        private void WiringManager_WiringDestroyed(List<Wire> wires)
        {
            _stateMachine.MoveToState("EmptyProject");
        }

        private void ModelManager_ModelCreated(Model model)
        {
            _stateMachine.MoveToState("WithModelAndWiring");
        }
        #endregion
        #endregion
    }
}