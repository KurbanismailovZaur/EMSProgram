using Numba;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP
{
	public class MaterialManager : MonoSingleton<MaterialManager> 
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
        private Material _defaultMaterialForModel;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Material DefaultMaterialForModel { get { return _defaultMaterialForModel; } }
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