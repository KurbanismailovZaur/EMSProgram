using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP
{
    [RequireComponent(typeof(Camera))]
	public class CameraSettingsController : MonoBehaviour 
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
        private Camera _camera;

        [SerializeField]
        private float _rectOffsetFromTop = 20f;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            _camera = GetComponent<Camera>();
        }

        private void Update()
        {
            float normilizedOffsetFromTop = _rectOffsetFromTop / Screen.height;

            _camera.aspect = (Screen.width - normilizedOffsetFromTop) / Screen.height;

            Rect cameraRect = _camera.rect;
            cameraRect.height = 1f - normilizedOffsetFromTop;
            _camera.rect = cameraRect;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}