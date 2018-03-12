using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EMSP
{
    public class SceneGizmo : MonoSingleton<SceneGizmo>
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
        [Range(0,10)]
        public float Offset = 0;

        [SerializeField]
        private Transform _topPoint;
        [SerializeField]
        private Transform _bottomPoint;

        private Camera _gizmoCamera;
        private Transform _transform;

        private Vector3 _baseOffset;
        private float _tenthPartOfGizmoSize;

        private bool _isInitialized = false;
        private float _currentScreenWidth;
        private float _currentScreenHeight;
        #endregion

        #region Events
        public event Action<Vector3> GizmoDirectionClickEvent = (v) => { };

        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        private void Start()
        {
            if (!_isInitialized)
            {
                _transform = transform;
                _gizmoCamera = GameObject.FindGameObjectWithTag("GizmoCamera").GetComponent<Camera>();

                foreach (var handler in GetComponentsInChildren<GizmoDirectionsClickHandler>())
                {
                    handler.ClickEvent += OnGizmoDirectionClick;
                }

                _isInitialized = true;
            }

            _currentScreenWidth = Screen.width;
            _currentScreenHeight = Screen.height;

            var rawHeight = Vector3.Magnitude(_topPoint.position - _bottomPoint.position);
            _transform.position = _gizmoCamera.ScreenToWorldPoint(new Vector3(Screen.width / 2, Screen.height / 2, rawHeight));
            _transform.rotation = _gizmoCamera.transform.rotation;

            var scrTop = _gizmoCamera.WorldToScreenPoint(_topPoint.position);
            var scrBottom = _gizmoCamera.WorldToScreenPoint(_bottomPoint.position);
            var gizmoHeight = Vector3.Magnitude(scrTop - scrBottom);

            _tenthPartOfGizmoSize = gizmoHeight / 10;
            _baseOffset = new Vector3(gizmoHeight / 2, gizmoHeight / 2, rawHeight);
        }

        private void LateUpdate()
        {
            var newScreenPosition = new Vector3(
                Screen.width - (_baseOffset.x + Offset * _tenthPartOfGizmoSize), 
                Screen.height - 20 - (_baseOffset.y + Offset * _tenthPartOfGizmoSize), // 20 - menu panel height
                _baseOffset.z
                );

            _transform.position = _gizmoCamera.ScreenToWorldPoint(newScreenPosition);
            _transform.rotation = Quaternion.identity;        
        }

        private void Update()
        {
            if (_currentScreenWidth != Screen.width || _currentScreenHeight != Screen.height)
                Start();
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers

        private void OnGizmoDirectionClick(Vector3 dir)
        {
            Debug.Log(dir);
            GizmoDirectionClickEvent(dir);
        }
        #endregion
        #endregion
    }
}