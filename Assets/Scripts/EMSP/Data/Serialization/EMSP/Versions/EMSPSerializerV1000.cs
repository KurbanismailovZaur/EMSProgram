using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using System.Linq;
using EMSP.Data.Serialization.EMSP.Exceptions;

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
            return null;
        }

        public override GameObject Deserialize(Stream stream)
        {
            // Readme.
            // Use ReadPreambleAndCheck(reader) for check file format.
            // Use ReadVersion(reader) for check version capability and if is necessary throw EMSPVersionCompatibilityException exception.

            return null;
        }
        #endregion
        #endregion
    }
}