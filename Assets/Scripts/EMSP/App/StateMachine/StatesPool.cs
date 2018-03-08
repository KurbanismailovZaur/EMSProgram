using EMSP.App.StateMachine.States;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App.StateMachine
{
    public class StatesPool : MonoBehaviour
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
        [SerializeField]
        private EmptyState _emptyState;

        [SerializeField]
        private DefaultState _defaultState;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public EmptyState EmptyState { get { return _emptyState; } }

        public DefaultState DefaultState { get { return _defaultState; } }
        #endregion

        #region Methods
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}