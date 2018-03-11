using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App.StateMachineBehaviour
{
    public abstract class State : MonoBehaviour
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
        protected StateMachine _stateMachine;

        private bool _isSubStateMachine;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public bool IsSubStateMachine { get { return _isSubStateMachine; } }
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            _stateMachine = GetComponentInParent<StateMachine>();
            _isSubStateMachine = _stateMachine.gameObject == gameObject;
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}