using EMSP.UI;
using Numba;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App
{
	public class GameFacade : MonoSingleton<GameFacade> 
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
        private ViewBlocker _viewBlocker;
		#endregion
		
		#region Events
		#endregion
		
		#region Behaviour
		#region Properties
		#endregion
		
		#region Constructors
		#endregion
		
		#region Methods
        public void ActivateProjectEnvironment()
        {
            Camera.main.transform.position = GameSettings.Instance.CameraDefaultPosition;
            Camera.main.transform.rotation = GameSettings.Instance.CameraDefaultRotation;

            _viewBlocker.UnblockView();
        }

        public void DeactivateProjectEnvironment()
        {
            _viewBlocker.BlockView();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}