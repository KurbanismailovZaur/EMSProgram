using EMSP.Data.OBJ;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP
{
	public class ModelManager : MonoSingleton<ModelManager> 
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
        public class ModelCreated : UnityEvent<Model> { }

        [Serializable]
        public class ModelDestroyed : UnityEvent<Model> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private OBJImporter _importer = new OBJImporter();

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
        public void CreateNewModel(string pathToOBJ)
        {
            DestroyModel();

            Model.Factory modelFactory = new Model.Factory();
            _model = modelFactory.MakeFactory(_importer.Import(pathToOBJ));

            _model.transform.position = Vector3.zero;
            _model.transform.SetParent(transform);
        }

        public void DestroyModel()
        {
            if (!_model)
            {
                return;
            }

            Destroy(_model.gameObject);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}