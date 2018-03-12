using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.App.StateMachineBehaviour.States.InProject
{
	public class WithModelAndWiringState : State
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
        private Button _viewModelIsTransparentButton;
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
            _removeWiringButton.interactable = true;
            _viewModelIsTransparentButton.interactable = true;

            ModelManager.Instance.ModelDestroyed.AddListener(ModelManager_ModelDestroyed);
            WiringManager.Instance.WiringDestroyed.AddListener(WiringManager_WiringDestroyed);
        }

        public override void OnExit()
        {
            WiringManager.Instance.WiringDestroyed.RemoveListener(WiringManager_WiringDestroyed);
            ModelManager.Instance.ModelDestroyed.RemoveListener(ModelManager_ModelDestroyed);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        private void WiringManager_WiringDestroyed(List<Wire> wiring)
        {
            _stateMachine.MoveToState("WithModel");
        }

        private void ModelManager_ModelDestroyed(Model model)
        {
            _stateMachine.MoveToState("WithWiring");
        }
        #endregion
        #endregion
    }
}