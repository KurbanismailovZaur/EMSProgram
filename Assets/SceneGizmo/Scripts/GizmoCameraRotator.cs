using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace EMSP
{
    public class GizmoCameraRotator : MonoBehaviour
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


        private Transform _transform;
        private Transform _mainTransform;
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
            _transform = transform;
            _mainTransform = Camera.main.transform;
        }

        private void Update()
        {
            _transform.rotation = _mainTransform.rotation;
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}