using Numba;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Environment
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
        #endregion

        #region Behaviour
        #region Properties
        public bool Visibility
        {
            get { return _visibility; }
            set
            {
                _visibility = value;
                _gridRenderer.enabled = _visibility;
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