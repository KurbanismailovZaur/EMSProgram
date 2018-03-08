using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App.StateMachine
{
    public abstract class GameState : MonoBehaviour
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
        protected Game _game;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            _game = GetComponentInParent<Game>();
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}