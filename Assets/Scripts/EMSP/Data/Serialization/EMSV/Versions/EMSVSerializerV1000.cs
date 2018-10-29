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
using EMSP.Logging;
using EMSP.Data.OBJ;

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
        private void SetProgressState(float progress, string name)
        {
            Progress = progress;
            ProgressName = name;
        }

        private void CallMethodWithProgressTrack(float startProgress, string startProgressName, Action method)
        {
            SetProgressState(startProgress, startProgressName);
            method.Invoke();
            SetProgressState(1f, "Завершен");
        }


        public override byte[] Serialize(Dictionary<string, List<Vector3>> materialVertexPacks)
        {
            SetProgressState(0.5f, "Формирование данных");
            byte[] bytes = SerializeWitoutEvents(materialVertexPacks);
            SetProgressState(1f, "Завершен");

            return bytes;
        }

        private byte[] SerializeWitoutEvents(Dictionary<string, List<Vector3>> materialVertexPacks)
        {

            string temporaryFileName = Path.GetTempFileName();
            using (BinaryWriter writer = new BinaryWriter(new FileStream(temporaryFileName, FileMode.Create)))
            {
                WritePreambleAndVersion(writer, _version);
                writer.Write(materialVertexPacks.Count);

                int index = 0;
                float delta = 0.3f / materialVertexPacks.Count;

                foreach (var matVertexPair in materialVertexPacks)
                {
                    if (_isCanceled) return null;

                    Progress = 0.5f + (index++ * delta);
                    WriteStringAsUnicode(writer, matVertexPair.Key);
                    writer.Write(matVertexPair.Value.Count);

                    foreach (var vertex in matVertexPair.Value)
                    {
                        if (_isCanceled) return null;

                        WriteVector3(writer, vertex);
                    }
                }
            }

            byte[] emsvData = File.ReadAllBytes(temporaryFileName);
            File.Delete(temporaryFileName);

            return emsvData;
        }


        public override void Serialize(Dictionary<string, List<Vector3>> materialVertexPack, string pathToEMSV)
        {
            CallMethodWithProgressTrack(0.5f, "Формирование данных", () => { SerializeWitoutEvents(materialVertexPack, pathToEMSV); });
        }

        private void SerializeWitoutEvents(Dictionary<string, List<Vector3>> materialVertexPack, string pathToEMSV)
        {
            byte[] bytes = SerializeWitoutEvents(materialVertexPack);

            if (_isCanceled) return;

            SetProgressState(0.9f, "Запись в файл");
            File.WriteAllBytes(pathToEMSV, bytes);
        }


        public override void ParseAndSerialize(string objFilePathForRead, string emsvFilePathForWrite)
        {
            CallMethodWithProgressTrack(0.3f, "Обработка файла OBJ", () => { ParseAndSerializeWithoutEvents(objFilePathForRead, emsvFilePathForWrite); });
        }

        private void ParseAndSerializeWithoutEvents(string objFilePathForRead, string emsvFilePathForWrite)
        {
            var materialVertexPack = GetDataFromOBJ(objFilePathForRead);

            if (_isCanceled) return;

            SetProgressState(0.5f, "Формиравание данных");
            SerializeWitoutEvents(materialVertexPack, emsvFilePathForWrite);
        }


        private Dictionary<string, List<Vector3>> GetDataFromOBJ(string pathToOBJ)
        {
            OBJImporter importer = new OBJImporter();
            return importer.GetVerticesInfoFromOBJ(pathToOBJ, () => _isCanceled);
        }

        public override Dictionary<string, List<Vector3>> Deserialize(Stream stream)
        {
            Log.WriteOperation("Started_EMSVSerializer_Deserialize");

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