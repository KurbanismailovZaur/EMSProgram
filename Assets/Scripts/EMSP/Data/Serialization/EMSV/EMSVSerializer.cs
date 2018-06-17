using EMSP.Data.Serialization.EMSV.Versions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityApplication = UnityEngine.Application;

namespace EMSP.Data.Serialization.EMSV
{
    public class EMSVSerializer
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
        private IEMSVSerializer _serializer;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public IEMSVSerializer Serializer { get { return _serializer; } }

        public static EMSVSerializer LatestVersion { get { return V1000; } }

        #region Serializer versions
        public static EMSVSerializer V1000 { get { return new EMSVSerializer(new EMSVSerializerV1000()); } }
        #endregion
        #endregion

        #region Methods
        private EMSVSerializer(IEMSVSerializer serializer)
        {
            _serializer = serializer;
        }

        #region Serialize
        public byte[] Serialize(Dictionary<string, List<Vector3>> materialVertexPacks)
        {
            return _serializer.Serialize(materialVertexPacks);
        }

        public void Serialize(string path, Dictionary<string, List<Vector3>> materialVertexPacks)
        {
            byte[] data = Serialize(materialVertexPacks);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, data);
        }
        #endregion

        #region Deserialize
        public Dictionary<string, List<Vector3>>  Deserialize(Stream stream)
        {
            return _serializer.Deserialize(stream);
        }

        public Dictionary<string, List<Vector3>> Deserialize(string path)
        {
            return Deserialize(File.OpenRead(path));
        }
        #endregion
        #endregion
        #endregion
    }
}