using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;
using EMSV.Data.Serialization.EMSV.Exceptions;
using System.Collections.ObjectModel;


namespace EMSV.Data.Serialization.EMSV.Versions
{
    /// <summary>
    /// This version work only with binary data (JsonEngine not used).
    /// Also this version start using MRB preamble and file versions writen in file.
    /// </summary>
    public class EMSVSerializerV1000 : EMSVSerializerVersion, IEMSVSerializer
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
        public override byte[] Serialize(Dictionary<string, List<Vector3>> materialVertexPacks)
        {
            using (BinaryWriter writer = new BinaryWriter(new FileStream(TemporaryFileName, FileMode.Create)))
            {
                WritePreambleAndVersion(writer, _version);
                writer.Write(materialVertexPacks.Count);

                foreach(var matVertexPair in materialVertexPacks)
                {
                    WriteStringAsUnicode(writer, matVertexPair.Key);
                    writer.Write(matVertexPair.Value.Count);

                    foreach(var vertex in matVertexPair.Value)
                    {
                        WriteVector3(writer, vertex);
                    }
                }
            }

            byte[] emsvData = File.ReadAllBytes(TemporaryFileName);
            File.Delete(TemporaryFileName);

            return emsvData;
        }

        public override Dictionary<string, List<Vector3>> Deserialize(Stream stream)
        {
            using (BinaryReader reader = new BinaryReader(stream))
            {
                #region Preamble and version
                ReadPreambleAndCheck(reader);

                Version version = ReadVersion(reader);
                if (version != _version)
                {
                    throw new EMSVVersionCompatibilityException(string.Format("File version is {0}, but you try to use serializer with {1} version", version, _version));
                }
                #endregion

                Dictionary<string, List<Vector3>> result = new Dictionary<string, List<Vector3>>();

                int packsCount = reader.ReadInt32();

                for(int matIndex = 0; matIndex < packsCount; ++matIndex)
                {
                    string matName = ReadStringAsUnicode(reader);

                    result.Add(matName, new List<Vector3>());

                    int vertCount = reader.ReadInt32();
                    for(int vertIndex = 0; vertIndex < vertCount; ++vertIndex)
                    {
                        result[matName].Add(ReadVector3(reader));
                    }
                }

                return result;
            }
        }
        #endregion
    }
}