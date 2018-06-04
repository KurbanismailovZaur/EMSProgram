﻿using System;
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

namespace EMSP.Data.Serialization.EMSP.Versions
{
    /// <summary>
    /// This version work only with binary data (JsonEngine not used).
    /// Also this version start using MRB preamble and file versions writen in file.
    /// </summary>
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
            using (BinaryWriter writer = new BinaryWriter(new FileStream(TemporaryFileName, FileMode.Create)))
            {
                WritePreambleAndVersion(writer, _version);
                WriteSettings(writer, serializableProjectBatch.ProjectSettings);

                #region Write model
                bool hasModel = serializableProjectBatch.ModelGameObject != null;
                writer.Write(hasModel);

                if (hasModel)
                {
                    WriteModel(writer, serializableProjectBatch.ModelGameObject);
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
                bool hasMagneticTensionInSpace = serializableProjectBatch.PointsInfo != null;
                writer.Write(hasMagneticTensionInSpace);

                if (hasMagneticTensionInSpace)
                {
                    WriteMagneticTensionInSpace(writer, serializableProjectBatch.PointsInfo);
                }
                #endregion
            }

            byte[] emspData = File.ReadAllBytes(TemporaryFileName);
            File.Delete(TemporaryFileName);

            return emspData;
        }

        public override SerializableProjectBatch Deserialize(Stream stream)
        {
            SerializableProjectSettings settings;
            GameObject modelGameObject = null;
            Wiring wiring = null;
            MagneticTensionInSpace.PointsInfo pointsInfo = null;

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
                    List<MagneticTensionInSpace.PointInfo> ptsInfo = new List<MagneticTensionInSpace.PointInfo>();

                    for(int i = 0; i < pointsCount; ++i)
                    {
                        var position = ReadVector3(reader);
                        var precomputed = reader.ReadSingle();
                        List<CalculatedMagneticTensionInTime> calculatedMagneticTensionInTime = new List<CalculatedMagneticTensionInTime>();


                        float[] timeSteps = TimeManager.Instance.CalculateSteps(settings.TimeRange.Start, settings.TimeRange.End, settings.TimeStepsCount);
                        for (int j = 0; j < settings.TimeStepsCount; ++j)
                        {
                            calculatedMagneticTensionInTime.Add(new CalculatedMagneticTensionInTime(timeSteps[j], reader.ReadSingle()));
                        }

                        ptsInfo.Add(new MagneticTensionInSpace.PointInfo(position, precomputed, calculatedMagneticTensionInTime.ToArray()));
                    }

                    pointsInfo =  new MagneticTensionInSpace.PointsInfo(pointSize, ptsInfo);
                }
                #endregion

                return new SerializableProjectBatch(settings, modelGameObject, wiring, pointsInfo);
            }
        }
        #endregion
    }
}