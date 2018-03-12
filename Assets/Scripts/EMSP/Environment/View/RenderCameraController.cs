using Numba.Geometry;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Environment.View
{
    [RequireComponent(typeof(Camera))]
	public class RenderCameraController : MonoBehaviour 
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
        private Transform _target;

        [SerializeField]
        private float _distanceFromOrigin = 1f;

        [SerializeField]
        private float _offsetFromTop = 20f;

        [SerializeField]
        private float _viewCubeRectPixelSize = 128;
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
            _camera.aspect = 1f;
        }

        private void Update()
        {
            UpdateTransform();
            UpdateRect();
        }

        private void UpdateTransform()
        {
            transform.position = _target.position.normalized * _distanceFromOrigin;
            transform.rotation = _target.rotation;
        }

        private void UpdateRect()
        {
            float viewCubeNormalizedWidth = _viewCubeRectPixelSize / Screen.width;
            float viewCubeNormalizedHeight = _viewCubeRectPixelSize / Screen.height;
            float normalizedOffsetFromTop = _offsetFromTop / Screen.height;

            Rect rect = new Rect(1f - viewCubeNormalizedWidth, 1f - viewCubeNormalizedHeight - normalizedOffsetFromTop, viewCubeNormalizedWidth, viewCubeNormalizedHeight);

            _camera.rect = rect;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}