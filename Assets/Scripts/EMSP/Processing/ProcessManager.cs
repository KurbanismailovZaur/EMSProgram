using EMSP.Data.Serialization.EMSV.Versions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Processing
{
    public class ProcessManager : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class ProcessCreatedEvent : UnityEvent<ProcessManager, IProcessable> { }

        //[Serializable]
        //public class ProcessDestroyedEvent : UnityEvent<ProcessManager, IProcessable> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        //private List<IProcessable> _processes = new List<IProcessable>();
        #endregion

        #region Events
        public ProcessCreatedEvent ProcessCreated = new ProcessCreatedEvent();

        //public ProcessDestroyedEvent ProcessDestroyed = new ProcessDestroyedEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void InvokeFromNewThread(Action method)
        {
            new Thread(() =>
            {
                method.Invoke();
            }).Start();
        }

        public void CreateGenerateVerticesBasedOnOBJProcess(string pathToOBJ)
        {
            string pathToEMSV = Path.Combine(Path.GetDirectoryName(pathToOBJ), string.Format("{0}.emsv", Path.GetFileNameWithoutExtension(pathToOBJ)));

            EMSVSerializerV1000 serializer = new EMSVSerializerV1000();
            //_processes.Add(serializer);

            ProcessCreated.Invoke(this, serializer);

            //serializer.ProgressChanged += Processable_ProgressChanged;

            InvokeFromNewThread(() => { serializer.ParseAndSerialize(pathToOBJ, pathToEMSV); });
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        //private void Processable_ProgressChanged(IProcessable processable,float progress)
        //{
        //    if (progress >= 1f) _processes.Remove(processable);
        //}
        #endregion
        #endregion
    }
}
