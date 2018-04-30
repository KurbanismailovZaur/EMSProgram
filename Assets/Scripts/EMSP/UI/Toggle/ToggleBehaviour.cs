using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Toggle
{
    public class ToggleBehaviour : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class TryToStateSwitchingEvent : UnityEvent<ToggleBehaviour> { }

        [Serializable]
        public class StateChangedEvent : UnityEvent<ToggleBehaviour, bool> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private bool _state;

        [SerializeField]
        private bool _allowSelfSwitch;
        #endregion

        #region Events
        public TryToStateSwitchingEvent TryToStateSwitching = new TryToStateSwitchingEvent();

        public StateChangedEvent StateChanged = new StateChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public bool State
        {
            get { return _state; }
            set
            {
                if (_state == value)
                {
                    return;
                }

                _state = value;

                StateChanged.Invoke(this, _state);
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void SwitchState()
        {
            if (_allowSelfSwitch)
            {
                State = !State;
            }
            else
            {
                TryToStateSwitching.Invoke(this);
            }
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}