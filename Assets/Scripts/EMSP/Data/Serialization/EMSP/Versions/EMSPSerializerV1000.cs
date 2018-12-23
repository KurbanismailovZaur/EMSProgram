using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;
using EMSP.Data.Serialization.EMSP.Exceptions;
using EMSP.Mathematic;
using EMSP.Timing;
using EMSP.Communication;
using System.Collections.ObjectModel;
using EMSP.Mathematic.Magnetic;
using EMSP.Mathematic.Electric;
using EMSP.Logging;

namespace EMSP.Data.Serialization.EMSP.Versions
{
    public class EMSPSerializerV1000 : EMSPSerializerVersion, IEMSPSerializer
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
        private readonly Version _version = new Version(1, 0, 0, 0);
        #endregion

        #region Events
        #endregion

        #region Properties
        public Version Version { get { return _version; } }
        #endregion

        #region Methods
        public override byte[] Serialize(SerializableProjectBatch serializableProjectBatch)
        {
            Log.WriteOperation("Started_EMSPSerializer_Serialize");
            using (BinaryWriter writer = new BinaryWriter(new FileStream(TemporaryFileName, FileMode.Create)))
            {
                WritePreambleAndVersion(writer, _version);
                WriteSettings(writer, serializableProjectBatch.ProjectSettings);

                #region Write model
                bool hasModel = serializableProjectBatch.ModelGameObject != null;
                writer.Write(hasModel);

                if (hasModel)
                {
                    WriteGameObject(writer, serializableProjectBatch.ModelGameObject);
                }
                #endregion

                #region Write wiring
                bool hasWiring = serializableProjectBatch.Wiring != null;
                writer.Write(hasWiring);

                if (hasWiring)
                {
                    WriteWiring(writer, serializableProjectBatch.Wiring);
                }
                #endregion

                #region Write magnetic tension in space
                bool hasMagneticTensionInSpace = serializableProjectBatch.MagneticTensionPointsInfo != null;
                writer.Write(hasMagneticTensionInSpace);

                if (hasMagneticTensionInSpace)
                {
                    WriteMathematicPointsInfo(writer, serializableProjectBatch.MagneticTensionPointsInfo);
                }
                #endregion

                #region Write electric field
                bool hasElectricField = serializableProjectBatch.ElectricFieldPointsInfo != null;
                writer.Write(hasElectricField);

                if (hasElectricField)
                {
                    WriteMathematicPointsInfo(writer, serializableProjectBatch.ElectricFieldPointsInfo);
                }
                #endregion

                #region Induction
                bool hasInduction = serializableProjectBatch.InductionResults != null;
                writer.Write(hasInduction);

                if (hasInduction)
                {
                    WriteInductionResults(writer, serializableProjectBatch.InductionResults);
                }
                #endregion
            }

            byte[] emspData = File.ReadAllBytes(TemporaryFileName);
            File.Delete(TemporaryFileName);

            return emspData;
        }

        public override SerializableProjectBatch Deserialize(Stream stream)
        {
            Log.WriteOperation("Started_EMSPSerializer_Deserialize");

            SerializableProjectSettings settings;
            GameObject modelGameObject = null;
            Wiring wiring = null;
            MagneticTension.PointsInfo mtPointsInfo = null;
            ElectricField.PointsInfo efPointsInfo = null;
            Mathematic.Induction.InductionCalculator.InductionResultCalculation[] inductionRes = null;

            using (BinaryReader reader = new BinaryReader(stream))
            {
                #region Preamble and version
                ReadPreambleAndCheck(reader);

                Version version = ReadVersion(reader);
                if (version != _version)
                {
                    throw new EMSPVersionCompatibilityException(string.Format("File version is {0}, but you try to use serializer with {1} version", version, _version));
                }
                #endregion

                settings = ReadProjectSettings(reader);

                #region Model
                bool hasModel = reader.ReadBoolean();
                if (hasModel)
                {
                    List<Mesh> meshes = ReadMeshes(reader);
                    List<Texture2D> textures = ReadTextures(reader);
                    List<Shader> shaders = ReadShaders(reader);
                    List<Material> materials = ReadMaterials(reader, shaders, textures);
                    modelGameObject = ReadHierarchy(reader, meshes, materials);
                }
                #endregion

                #region Wiring
                bool hasWiring = reader.ReadBoolean();

                if (hasWiring)
                {
                    wiring = new Wiring.Factory().Create();
                    int wireCount = reader.ReadInt32();

                    for (int i = 0; i < wireCount; ++i)
                    {
                        Wire newWire = wiring.CreateWire(ReadStringAsUnicode(reader), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                        int pointCount = reader.ReadInt32();
                        for (int j = 0; j < pointCount; ++j)
                        {
                            newWire.Add(ReadVector3(reader));
                        }
                    }
                }
                #endregion

                #region Magnetic tension in space
                bool hasMagneticTensionInSpace = reader.ReadBoolean();

                if (hasMagneticTensionInSpace)
                {
                    float pointSize = reader.ReadSingle();

                    int pointsCount = (int)Mathf.Pow(settings.RangeLength, 3);
                    List<MagneticTension.PointInfo> ptsInfo = new List<MagneticTension.PointInfo>();

                    for(int i = 0; i < pointsCount; ++i)
                    {
                        var position = ReadVector3(reader);
                        var precomputed = reader.ReadSingle();
                        List<PointableCalculatedValueInTime> calculatedMagneticTensionInTime = new List<PointableCalculatedValueInTime>();


                        float[] timeSteps = TimeManager.Instance.CalculateSteps(settings.TimeRange.Start, settings.TimeRange.End, settings.TimeStepsCount);
                        for (int j = 0; j < settings.TimeStepsCount; ++j)
                        {
                            calculatedMagneticTensionInTime.Add(new PointableCalculatedValueInTime(timeSteps[j], reader.ReadSingle()));
                        }

                        ptsInfo.Add(new MagneticTension.PointInfo(position, precomputed, calculatedMagneticTensionInTime.ToArray()));
                    }

                    mtPointsInfo =  new MagneticTension.PointsInfo(pointSize, ptsInfo);
                }
                #endregion

                #region ElectricField
                bool hasElectricField = reader.ReadBoolean();

                if (hasElectricField)
                {
                    float pointSize = reader.ReadSingle();

                    int pointsCount = (int)Mathf.Pow(settings.RangeLength, 3);
                    List<ElectricField.PointInfo> ptsInfo = new List<ElectricField.PointInfo>();

                    for (int i = 0; i < pointsCount; ++i)
                    {
                        var position = ReadVector3(reader);
                        var precomputed = reader.ReadSingle();
                        List<PointableCalculatedValueInTime> calculatedElectricFieldInTime = new List<PointableCalculatedValueInTime>();


                        float[] timeSteps = TimeManager.Instance.CalculateSteps(settings.TimeRange.Start, settings.TimeRange.End, settings.TimeStepsCount);
                        for (int j = 0; j < settings.TimeStepsCount; ++j)
                        {
                            calculatedElectricFieldInTime.Add(new PointableCalculatedValueInTime(timeSteps[j], reader.ReadSingle()));
                        }

                        ptsInfo.Add(new ElectricField.PointInfo(position, precomputed, calculatedElectricFieldInTime.ToArray()));
                    }

                    efPointsInfo = new ElectricField.PointsInfo(pointSize, ptsInfo);
                }
                #endregion

                #region Induction
                bool hasInduction = reader.ReadBoolean();

                if (hasInduction)
                {
                    int count = reader.ReadInt32();
                    inductionRes = new Mathematic.Induction.InductionCalculator.InductionResultCalculation[count];

                    for (int i = 0; i < count; ++i)
                    {
                        inductionRes[i].WireA = wiring.GetWireByName(ReadStringAsUnicode(reader));
                        inductionRes[i].WireB = wiring.GetWireByName(ReadStringAsUnicode(reader));
                        inductionRes[i].Value = reader.ReadSingle();             
                    }
                }
                #endregion

                return new SerializableProjectBatch(settings, modelGameObject, wiring, mtPointsInfo, efPointsInfo, inductionRes);
            }
        }
        #endregion
    }
}