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
        private StateMachine _parentStateMachine;

        [SerializeField]
        private List<string> _statesNames = new List<string>();

        [SerializeField]
        private List<State> _states = new List<State>();

        [SerializeField]
        private OnStartSettings _onStartSettings;

        private State _currentState;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public StateMachine ParentStateMachine { get { return _parentStateMachine; } }

        public State State { get { return _currentState; } }
        #endregion

        #region Methods
        private void Awake()
        {
            StateMachine[] stateMachines = GetComponentsInParent<StateMachine>();

            if (stateMachines.Length > 1)
            {
                _parentStateMachine = stateMachines[stateMachines.Length - 1];
            }
        }

        private void Start()
        {
            if (_onStartSettings.EnterToStateOnStart)
            {
                MoveToState(_onStartSettings.StartStateName);
            }
        }

        public void MoveToState(string stateName)
        {
            int index = _statesNames.IndexOf(stateName);

            if (index == -1)
            {
                throw new ArgumentException(string.Format("State with name \"{0}\" not exist.", stateName));
            }

            MoveToState(_states[index]);
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

        public void ExitFromCurrentState()
        {
            if (_currentState)
            {
                _currentState.OnExit();
                _currentState = null;
            }
        }
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}