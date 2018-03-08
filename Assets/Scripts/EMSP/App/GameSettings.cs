using Numba;
using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App
{
	public class GameSettings : MonoSingleton<GameSettings> 
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
        private ExtensionFilter[] _modelExtensionFilter = new ExtensionFilter[] { new ExtensionFilter("3D Model", "obj") };

        private ExtensionFilter[] _wiringExtensionFilter = new ExtensionFilter[] { new ExtensionFilter("Excel Worksheets 2003", "xls") };

        private string _projectDefaultName = "Untitled";

        private Vector3 _cameraDefaultPosition = new Vector3(0f, 8f, 0f);

        private Quaternion _cameraDefaultRotation = Quaternion.Euler(90f, 0f, 0f);
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public ExtensionFilter[] ModelExtensionFilter { get { return _modelExtensionFilter; } }

        public ExtensionFilter[] WiringExtensionFilter { get { return _wiringExtensionFilter; } }

        public string ProjectDefaultName { get { return _projectDefaultName; } }

        public Vector3 CameraDefaultPosition { get { return _cameraDefaultPosition; } }

        public Quaternion CameraDefaultRotation { get { return _cameraDefaultRotation; } }
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