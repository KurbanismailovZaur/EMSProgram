using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Environment.Metrics
{
    public class Grid : MonoSingleton<Grid>
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class VisibilityChangedEvent : UnityEvent<Grid, bool> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private bool _visibility = true;

        [SerializeField]
        private Renderer _gridRenderer;
        #endregion

        #region Events
        public VisibilityChangedEvent VisibilityChanged = new VisibilityChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public bool Visibility
        {
            get { return _visibility; }
            set
            {
                if (_visibility == value)
                {
                    return;
                }

                _gridRenderer.enabled = value;
                _visibility = value;

                VisibilityChanged.Invoke(this, _visibility);
            }
        }
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