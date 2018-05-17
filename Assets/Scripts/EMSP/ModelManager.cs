using EMSP.Data.OBJ;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

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
        public class ModelCreatedEvent : UnityEvent<Model> { }

        [Serializable]
        public class ModelDestroyedEvent : UnityEvent<Model> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private OBJImporter _importer = new OBJImporter();

        private Model _model;
        #endregion

        #region Events
        public ModelCreatedEvent ModelCreated = new ModelCreatedEvent();

        public ModelDestroyedEvent ModelDestroyed = new ModelDestroyedEvent();
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
            Material[] materials;
            GameObject modelGameObject = _importer.Import(pathToOBJ, out materials);

            CreateNewModel(modelGameObject);
        }

        public void CreateNewModel(GameObject gameObject)
        {
            DestroyModel();

            if (!gameObject) return;

            Model.Factory modelFactory = new Model.Factory();
            _model = modelFactory.MakeFactory(gameObject);

            _model.transform.position = Vector3.zero;
            _model.transform.SetParent(transform);

            ModelCreated.Invoke(_model);
        }

        public void DestroyModel()
        {
            if (!_model)
            {
                return;
            }

            Destroy(_model.gameObject);

            ModelDestroyed.Invoke(_model);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}