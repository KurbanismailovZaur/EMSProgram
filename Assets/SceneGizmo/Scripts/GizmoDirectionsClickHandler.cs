using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace EMSP
{
    public class GizmoDirectionsClickHandler : MonoBehaviour, IPointerClickHandler
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
        private Vector3 _direction;
        #endregion

        #region Events

        public event Action<Vector3> ClickEvent = (v) => { };
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion

        #region Indexers
        #endregion

        #region Events handlers

        public void OnPointerClick(PointerEventData eventData)
        {
            ClickEvent(_direction);
        }
        #endregion
        #endregion
    }
}