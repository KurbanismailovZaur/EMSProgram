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

        #region Behaviour
        #region Properties
        public Version Version { get { return _version; } }
        #endregion

        #region Methods
        public override byte[] Serialize()
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(_temporaryFileName, FileMode.Create)))
            {
                // <-- 0
                WritePreambleAndVersion(writer, _version);
                // --> 0

                // <-- 1
                writer.Write(MathematicManager.Instance.RangeLength);
                writer.Write(TimeManager.Instance.TimeRange.Start);
                writer.Write(TimeManager.Instance.TimeRange.End);
                writer.Write(TimeManager.Instance.StepsCount);
                // --> 1

                // <-- 2
                bool hasModel = (ModelManager.Instance.Model == null) ? false : true;

                writer.Write(hasModel);
                if (hasModel)
                {
                    GameObject target = ModelManager.Instance.Model.gameObject;

                    List<Material> materials = new List<Material>();
                    List<Texture2D> textures = new List<Texture2D>();
                    List<Shader> shaders = new List<Shader>();
                    List<Mesh> meshes = new List<Mesh>();

                    ExcludeData(target, materials, textures, shaders, meshes);

                    WriteMeshes(writer, meshes);
                    WriteTextures(writer, textures);
                    WriteShaders(writer, shaders);
                    WriteMaterials(writer, materials, shaders, textures);
                    WriteHierarchy(writer, target, meshes, materials);
                }
                // --> 2

                // <-- 3
                bool hasWiring = (WiringManager.Instance.Wiring == null) ? false : true;

                writer.Write(hasWiring);
                if (hasWiring)
                {
                    writer.Write(WiringManager.Instance.Wiring.Count);

                    foreach (var wire in WiringManager.Instance.Wiring)
                    {
                        WriteStringAsUnicode(writer, wire.Name);
                        writer.Write(wire.Amplitude);
                        writer.Write(wire.Frequency);
                        writer.Write(wire.Amperage);

                        writer.Write(wire.Count);
                        foreach(var point in wire)
                        {
                            WriteVector3(writer, point);
                        }
                    }
                }
                // --> 3

                // <-- 4
                bool hasMagneticTensionInSpace = (MathematicManager.Instance.MagneticTensionInSpace == null) ? false : true;

                writer.Write(hasMagneticTensionInSpace);
                if (hasMagneticTensionInSpace)
                {
                    writer.Write(MathematicManager.Instance.RangeLength);

                    foreach(Mathematic.MagneticTension.MagneticTensionPoint point in MathematicManager.Instance.MagneticTensionInSpace.MTPoints)
                    {
                        WriteVector3(writer, point.transform.position);

                        writer.Write(point.CurrentMagneticTension.PrecomputedAmperageResult);

                        for(int i = 0; i < point.MagneticTensionsInTime.Length; ++i)
                        {
                            writer.Write(point.MagneticTensionsInTime[i].MagneticTensionResult.CalculatedAmperageResult);
                        }
                    }
                }
                // --> 4
            }

            byte[] emspData = File.ReadAllBytes(_temporaryFileName);
            //File.Delete(_temporaryFileName);

            return emspData;
        }

        // needDelete
        public void DeserializeTest()
        {
            Deserialize(File.OpenRead(_temporaryFileName));
            File.Delete(_temporaryFileName);
        }
        public override GameObject Deserialize(Stream stream)
        {
            GameObject model = null;

            using (BinaryReader reader = new BinaryReader(stream))
            {
                // <-- 0
                ReadPreambleAndCheck(reader);

                Version version = ReadVersion(reader);
                if (version != _version)
                {
                    throw new EMSPVersionCompatibilityException(string.Format("File version is {0}, but you try to use serializer with {1} version", version, _version));
                }
                // --> 0

                // <-- 1
                MathematicManager.Instance.RangeLength = reader.ReadInt32();
                TimeManager.Instance.TimeRange = new Range(reader.ReadSingle(), reader.ReadSingle());
                TimeManager.Instance.StepsCount = reader.ReadInt32();
                // --> 1

                // <-- 2
                bool hasModel = reader.ReadBoolean();
                if (hasModel)
                {
                    List<Mesh> meshes = ReadMeshes(reader);
                    List<Texture2D> textures = ReadTextures(reader);
                    List<Shader> shaders = ReadShaders(reader);
                    List<Material> materials = ReadMaterials(reader, shaders, textures);
                    model = ReadHierarchy(reader, meshes, materials);
                }
                // --> 2

                // <-- 3
                bool hasWiring = reader.ReadBoolean();

                if (hasWiring)
                {
                    Wiring resultWiring = new Wiring.Factory().Create();
                    int wireCount = reader.ReadInt32();

                    for (int i = 0; i < wireCount; ++i)
                    {
                        Wire newWire = resultWiring.CreateWire(ReadStringAsUnicode(reader), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());

                        int pointCount = reader.ReadInt32();
                        for(int j = 0; j < pointCount; ++j)
                        {
                            newWire.Add(ReadVector3(reader));
                        }
                    }

                    WiringManager.Instance.CreateNewWiring(resultWiring);
                }
                // --> 3

                // <-- 4
                bool hasMagneticTensionInSpace = reader.ReadBoolean();

                if (hasMagneticTensionInSpace)
                {
                    MathematicManager.Instance.RangeLength = reader.ReadInt32();
                    int pointsCount = reader.ReadInt32();

                    //foreach (Mathematic.MagneticTension.MagneticTensionPoint point in MathematicManager.Instance.MagneticTensionInSpace.MTPoints)
                    //{
                    //    WriteVector3(writer, point.transform.position);

                    //    writer.Write(point.CurrentMagneticTension.PrecomputedAmperageResult);
                    //    writer.Write(point.MagneticTensionsInTime.Length);

                    //    for (int i = 0; i < point.MagneticTensionsInTime.Length; ++i)
                    //    {
                    //        writer.Write(point.MagneticTensionsInTime[i].MagneticTensionResult.CalculatedAmperageResult);
                    //    }
                    //}
                }
                // --> 4
            }

            return model;
        }
        #endregion
        #endregion
    }
}