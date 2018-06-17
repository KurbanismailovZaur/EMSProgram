using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;
using System.Collections.ObjectModel;
using OrbCreationExtensions;
using EMSP.Data.Serialization.EMSV.Exceptions;

namespace EMSP.Data.Serialization.EMSV.Versions
{
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

        public override void Serialize(string objFilePathForRead, string emsvFilePathForWrite)
        {
            var materialVertexPack = GetDataFromOBJ(objFilePathForRead);
            byte[] bytes = Serialize(materialVertexPack);

            File.WriteAllBytes(emsvFilePathForWrite, bytes);
        }

        private Dictionary<string, List<Vector3>> GetDataFromOBJ(string pathToOBJ)
        {
            Dictionary<string, List<Vector3>> materialVertexesPacks = new Dictionary<string, List<Vector3>>();

            using (StreamReader sr = new StreamReader(pathToOBJ))
            {
                List<Vector3> tempList = new List<Vector3>();

                while (!sr.EndOfStream)
                {
                    string line = sr.ReadLine();

                    if (line.StartsWith("usemtl"))
                    {
                        string matName = line.Substring(7);

                        if (!materialVertexesPacks.ContainsKey(matName)) materialVertexesPacks.Add(matName, new List<Vector3>());

                        materialVertexesPacks[matName].AddRange(tempList);
                        tempList.Clear();
                    }
                    else if (line.StartsWith("v"))
                    {
                        string vertexString = line.Substring(2);
                        tempList.Add(GetVector3FromObjString(vertexString));
                    }
                }
            }

            return materialVertexesPacks;
        }

        private Vector3 GetVector3FromObjString(string str)
        {
            Vector3 vec = new Vector3(0, 0, 0);
            int i = 0;
            for (int elem = 0; elem < 3; elem++)
            {
                int e = str.IndexOf(' ', i);
                if (e < 0) e = str.Length;
                vec[elem] = str.Substring(i, e - i).MakeFloat();
                i = str.EndOfCharRepetition(e);
            }
            return vec;
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