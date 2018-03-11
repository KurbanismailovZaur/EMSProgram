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
        public Vector3 ViewportOffset = new Vector3(0.8f, 0.8f, 0.1f);
        private Camera _gizmoCamera;
        private Transform _transform;


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
            _transform = transform;
            _gizmoCamera = GameObject.FindGameObjectWithTag("GizmoCamera").GetComponent<Camera>();

            foreach(var handler in GetComponentsInChildren<GizmoDirectionsClickHandler>())
            {
                handler.ClickEvent += OnGizmoDirectionClick;
            }
        }

        private void LateUpdate()
        {
            _transform.position = _gizmoCamera.ViewportToWorldPoint(new Vector3(ViewportOffset.x, ViewportOffset.y, Camera.main.nearClipPlane + ViewportOffset.z));
            _transform.rotation = Quaternion.identity;
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