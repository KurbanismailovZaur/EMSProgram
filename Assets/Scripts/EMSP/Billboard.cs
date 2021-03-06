﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP
{
	public class Billboard : MonoBehaviour 
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
        private Camera _camera;

        [SerializeField]
        private bool _useCameraUp;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void LateUpdate()
        {
            transform.rotation = Quaternion.LookRotation(transform.position - _camera.transform.position, _useCameraUp ? _camera.transform.up : Vector3.up);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}