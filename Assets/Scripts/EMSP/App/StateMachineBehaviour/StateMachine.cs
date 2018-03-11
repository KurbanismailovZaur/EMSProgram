using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App.StateMachineBehaviour
{
    public class StateMachine : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        [Serializable]
        public struct OnStartSettings
        {
            [SerializeField]
            private bool _enterToStateOnStart;

            [SerializeField]
            private string _startStateName;

            public bool EnterToStateOnStart { get { return _enterToStateOnStart; } }

            public string StartStateName { get { return _startStateName; } }
        }
        #endregion

        #region Classes
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private List<string> _statesNames;

        [SerializeField]
        private List<State> _states;

        [SerializeField]
        private OnStartSettings _onStartSettings;

        private State _currentState;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public State State { get { return _currentState; } }
        #endregion

        #region Methods
        private void Start()
        {
            if (_onStartSettings.EnterToStateOnStart)
            {
                MoveToState(_onStartSettings.StartStateName);
            }
        }

        public void MoveToState(string stateName)
        {
            MoveToState(_states[_statesNames.IndexOf(stateName)]);
        }

        private void MoveToState(State state)
        {
            if (_currentState != null)
            {
                _currentState.OnExit();
            }

            _currentState = state;
            _currentState.OnEnter();
        }
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}