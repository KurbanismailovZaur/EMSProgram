﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EMSP.Control;
using UnityEngine.EventSystems;

namespace EMSP.UI.Controls
{
    public class ControlsManager : MonoBehaviour 
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
        private float _moveSensivity = 1;
        [SerializeField]
        private float _zoomSensivity = 1;

        private SphereCameraRotator _rotator;
        private GameObject _emtyGO;
        private float _x = 0;
        private float _y = 0;
        private float _z = 0;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods

        private void Start()
        {
            _rotator = Camera.main.GetComponent<SphereCameraRotator>();
            _emtyGO = new GameObject("Empty");
        }

        public void OnUpClick()
        {
            _y += 0.01f * _moveSensivity;
        }

        public void OnDownClick()
        {
            _y -= 0.01f * _moveSensivity;
        }

        public void OnRightClick()
        {
            _x += 0.01f * _moveSensivity;
        }

        public void OnLeftClick()
        {
            _x -= 0.01f * _moveSensivity;
        }

        public void OnZoomInClick()
        {
            _z += 0.01f * _zoomSensivity;
        }

        public void OnZoomOutClick()
        {
            _z -= 0.01f * _zoomSensivity;
        }

        public void Deselect()
        {
            EventSystem.current.SetSelectedGameObject(_emtyGO);
        }

        private void Update()
        {
            _rotator.MoveCamera(_x, _y, _z);

            _x = 0;
            _y = 0;
            _z = 0;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}

