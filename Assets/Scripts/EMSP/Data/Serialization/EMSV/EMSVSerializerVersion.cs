using EMSP.Communication;
using EMSP.Data.Serialization;
using EMSP.Mathematic.Electric;
using EMSP.Mathematic.Magnetic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityApplication = UnityEngine.Application;

namespace EMSP.Data.Serialization.EMSV
{
    public abstract class EMSVSerializerVersion : BinarySerializerVersion
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
        protected string TemporaryFileName { get { return string.Format("{0}/serializedata.emsv", UnityApplication.temporaryCachePath); } }

        // 64 bytes preamble for file
        private readonly string _preamble = @"0M5OUaB4eDFPwfyOywRH1CUwaP7#hqwKfSVmmyufFgw#BKfcRnXD2BrmRYbO@Hij";
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        protected override string Preamble { get { return _preamble; } }
        #endregion

        #region Methods
        public abstract byte[] Serialize(Dictionary<string, List<Vector3>> materialVertexPacks);

        public abstract void Serialize(string pathToOBJ, string pathToEMSV);

        public abstract Dictionary<string, List<Vector3>> Deserialize(Stream stream);
        #endregion
        #endregion
    }
}