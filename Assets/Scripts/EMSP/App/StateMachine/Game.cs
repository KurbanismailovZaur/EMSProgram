﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App.StateMachine
{
    public class Game : MonoBehaviour
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
        private StatesPool _statesPool;

        private GameState _gameState;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        private void Start()
        {
            MoveToEmptyState();
        }

        private void MoveToState(GameState state)
        {
            if (_gameState != null)
            {
                _gameState.OnExit();
            }

            _gameState = state;
            _gameState.OnEnter();
        }

        #region Transitions between states
        public void MoveToEmptyState()
        {
            MoveToState(_statesPool.EmptyState);
        }

        public void MoveToDefaultState()
        {
            MoveToState(_statesPool.DefaultState);
        }
        #endregion
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}