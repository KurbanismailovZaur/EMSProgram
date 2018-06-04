using EMSP.Communication;
using EMSP.Data.Serialization;
using EMSP.Data.Serialization.EMSP;
using EMSP.Mathematic;
using EMSP.Mathematic.Magnetic;
using EMSP.Timing;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App
{
	public class Project
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
        private bool _isChanged;

        private string _path;

        private bool _isStored;

        private EMSPSerializer _serializer = EMSPSerializer.LatestVersion;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public bool IsChanged { get { return _isChanged; } }

        public string Path { get { return _path; } }

        public bool IsStored { get { return _isStored; } }
        #endregion

        #region Constructors
        public Project()
        {
            MathematicManager.Instance.RangeLength = GameSettings.Instance.CalculationDefaultMinRangeLength;
            TimeManager.Instance.TimeRange = GameSettings.Instance.DefaultTimeRange;
            TimeManager.Instance.StepsCount = GameSettings.Instance.DefaultTimeStepsCount;

            MathematicManager.Instance.RangeLengthChanged.AddListener(MathematicManager_RangeLengthChanged);
            TimeManager.Instance.TimeParametersChanged.AddListener(TimeManager_TimeParametersChanged);

            ModelManager.Instance.ModelCreated.AddListener(ModelManager_ModelCreated);
            ModelManager.Instance.ModelDestroyed.AddListener(ModelManager_ModelDestroyed);

            WiringManager.Instance.WiringCreated.AddListener(WiringManager_WiringCreated);
            WiringManager.Instance.WiringDestroyed.AddListener(WiringManager_WiringDestroyed);

            MathematicManager.Instance.MagneticTensionInSpace.Calculated.AddListener(MagneticTensionInSpace_Calculated);
            MathematicManager.Instance.MagneticTensionInSpace.Destroyed.AddListener(MagneticTensionInSpace_Destroyed);
        }

        public void Save(string path)
        {
            _path = path;

            Save();

            _isStored = true;
        }

        private void Save()
        {
            EMSPSerializerVersion.SerializableProjectSettings serializableSettings = new EMSPSerializerVersion.SerializableProjectSettings(MathematicManager.Instance.RangeLength, TimeManager.Instance.TimeRange, TimeManager.Instance.StepsCount);
            EMSPSerializerVersion.SerializableProjectBatch serializableProjectBatch = new EMSPSerializerVersion.SerializableProjectBatch(serializableSettings, ModelManager.Instance.Model, WiringManager.Instance.Wiring, MathematicManager.Instance.MagneticTensionInSpace.GetPointsInfo());

            _serializer.Serialize(_path, serializableProjectBatch);

            _isChanged = false;
        }

        public void Resave()
        {
            Save();
        }

        public void Load(string path)
        {
            _path = path;
            _isStored = true;

            EMSPSerializerVersion.SerializableProjectBatch serializableProjectBatch = _serializer.Deserialize(path);

            MathematicManager.Instance.RangeLength = serializableProjectBatch.ProjectSettings.RangeLength;
            TimeManager.Instance.SetTimeParameters(serializableProjectBatch.ProjectSettings.TimeRange, serializableProjectBatch.ProjectSettings.TimeStepsCount);

            ModelManager.Instance.CreateNewModel(serializableProjectBatch.ModelGameObject);
            WiringManager.Instance.CreateNewWiring(serializableProjectBatch.Wiring);

            MathematicManager.Instance.MagneticTensionInSpace.Restore(serializableProjectBatch.PointsInfo);

            _isChanged = false;
        }
        #endregion

        #region Methods
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        private void MathematicManager_RangeLengthChanged(MathematicManager mathematicManager, int rangeLength)
        {
            _isChanged = true;
        }

        private void TimeManager_TimeParametersChanged(TimeManager timeManager)
        {
            _isChanged = true;
        }

        private void ModelManager_ModelCreated(Model model)
        {
            _isChanged = true;
        }

        private void ModelManager_ModelDestroyed(Model model)
        {
            _isChanged = true;
        }

        private void WiringManager_WiringCreated(Wiring wiring)
        {
            _isChanged = true;
        }

        private void WiringManager_WiringDestroyed(Wiring wiring)
        {
            _isChanged = true;
        }

        private void MagneticTensionInSpace_Calculated(MagneticTensionInSpace magneticTensionInSpace)
        {
            _isChanged = true;
        }

        private void MagneticTensionInSpace_Destroyed(MagneticTensionInSpace magneticTensionInSpace)
        {
            _isChanged = true;
        }
        #endregion
        #endregion
    }
}