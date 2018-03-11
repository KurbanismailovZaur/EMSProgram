using Numba;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App.StateMachineBehaviour
{
	public class StateMachineManager : MonoSingleton<StateMachineManager>
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
        private StateMachine _rootStateMachine;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public StateMachine RootStateMachine { get { return _rootStateMachine; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}