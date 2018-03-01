using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Application
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
        #endregion
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}