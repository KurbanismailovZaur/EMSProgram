using EMSP.Communication;
using EMSP.Data.Serialization;
using EMSP.Mathematic.Electric;
using EMSP.Mathematic.Magnetic;
using EMSP.Processing;
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
    public abstract class EMSVSerializerVersion : BinarySerializerVersion, IProcessable
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
        // 64 bytes preamble for file
        private readonly string _preamble = @"0M5OUaB4eDFPwfyOywRH1CUwaP7#hqwKfSVmmyufFgw#BKfcRnXD2BrmRYbO@Hij";

        protected float _progress;

        protected string _progressName;

        protected bool _isCanceled;
        #endregion

        #region Events
        public event Action<IProcessable, float> ProgressChanged;

        public event Action<IProcessable, string> ProgressNameChanged;

        public event Action<IProcessable> ProgressCanceled;
        #endregion

        #region Behaviour
        #region Properties
        protected override string Preamble { get { return _preamble; } }

        public float Progress
        {
            get { return _progress; }
            protected set
            {
                if (_progress == value) return;

                _progress = value;

                if (ProgressChanged != null) ProgressChanged.Invoke(this, _progress);
            }
        }

        public string ProgressName
        {
            get { return _progressName; }
            protected set
            {
                if (_progressName == value) return;

                _progressName = value;

                if (ProgressNameChanged != null) ProgressNameChanged.Invoke(this, _progressName);
            }
        }
        #endregion

        #region Methods
        public abstract byte[] Serialize(Dictionary<string, List<Vector3>> materialVertexPack);

        public abstract void Serialize(Dictionary<string, List<Vector3>> materialVertexPack, string pathToEMSV);

        public abstract void ParseAndSerialize(string pathToOBJ, string pathToEMSV);

        public abstract Dictionary<string, List<Vector3>> Deserialize(Stream stream);

        public void Cancel()
        {
            if (_isCanceled) return;

            _isCanceled = true;

            if (ProgressCanceled != null) ProgressCanceled.Invoke(this);
        }
        #endregion
        #endregion
    }
}