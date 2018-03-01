using EMSP.Data.OBJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP
{
	public class ModelManager : MonoBehaviour 
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
        private ObjModelImporter _importer = new ObjModelImporter();

        private Model _model;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Model Model { get { return _model; } }
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