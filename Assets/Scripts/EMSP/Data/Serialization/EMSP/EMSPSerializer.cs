using EMSP.Data.Serialization.EMSP.Versions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityApplication = UnityEngine.Application;

namespace EMSP.Data.Serialization.EMSP
{
    public class EMSPSerializer
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
        private IEMSPSerializer _serializer;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public IEMSPSerializer Serializer { get { return _serializer; } }

        public static EMSPSerializer LatestVersion { get { return V1000; } }

        #region Serializer versions
        public static EMSPSerializer V1000 { get { return new EMSPSerializer(new EMSPSerializerV1000()); } }
        #endregion
        #endregion

        #region Methods
        private EMSPSerializer(IEMSPSerializer serializer)
        {
            _serializer = serializer;
        }

        #region Serialize
        public byte[] Serialize(EMSPSerializerVersion.SerializableProjectBatch serializableProjectBatch)
        {
            return _serializer.Serialize(serializableProjectBatch);
        }

        public void Serialize(string path, EMSPSerializerVersion.SerializableProjectBatch serializableProjectBatch)
        {
            byte[] data = Serialize(serializableProjectBatch);

            if (File.Exists(path))
            {
                File.Delete(path);
            }

            File.WriteAllBytes(path, data);
        }
        #endregion

        #region Deserialize
        public EMSPSerializerVersion.SerializableProjectBatch Deserialize(Stream stream)
        {
            return _serializer.Deserialize(stream);
        }

        public EMSPSerializerVersion.SerializableProjectBatch Deserialize(string path)
        {
            return Deserialize(File.OpenRead(path));
        }
        #endregion
        #endregion
        #endregion
    }
}