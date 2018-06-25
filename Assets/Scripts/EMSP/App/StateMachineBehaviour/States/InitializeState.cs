using EMSP.Logging;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using UnityEngine;

namespace EMSP.App.StateMachineBehaviour.States
{
	public class InitializeState : State 
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
            Log.Initialize();
            Log.WriteOperation("Hello World!");

            SetupGlobalization();
            
            _stateMachine.MoveToState("OnlyMenu");
        }

        private void SetupGlobalization()
        {
            CultureInfo customCulture = (CultureInfo)Thread.CurrentThread.CurrentCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = GameSettings.Instance.NumberDecimalSeparator;

            Thread.CurrentThread.CurrentCulture = customCulture;

        }

        private void OnDestroy()
        {
            Log.Dispose();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}