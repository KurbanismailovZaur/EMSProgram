using EMSP.Data.XLS;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Communication
{
    public class WiringManager : MonoSingleton<WiringManager>
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
        public class WiringCreatedEvent : UnityEvent<List<Wire>> { }

        [Serializable]
        public class WiringDestroyedEvent : UnityEvent<List<Wire>> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private WiringDataReader _wiringDataReader = new WiringDataReader();

        [SerializeField]
        private Material _lineMaterial;

        private List<Wire> _wires = new List<Wire>();
        #endregion

        #region Events
        public WiringCreatedEvent WiringCreated = new WiringCreatedEvent();

        public WiringDestroyedEvent WiringDestroyed = new WiringDestroyedEvent();

        public UnityEvent WiringVisibilityChanged = new UnityEvent();
        #endregion

        #region Behaviour
        #region Properties
        public ReadOnlyCollection<Wire> Wires { get { return _wires.AsReadOnly(); } }

        public bool IsWiringVisible
        {
            get { return _wires[0].gameObject.activeSelf; }
            set
            {
                if (_wires[0].gameObject.activeSelf == value)
                {
                    return;
                }

                foreach (Wire wire in _wires)
                {
                    wire.gameObject.SetActive(value);
                }

                WiringVisibilityChanged.Invoke();
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void CreateNewWiring(string pathToXLS)
        {
            DestroyWiring();

            _wires = _wiringDataReader.ReadWiringFromFile(pathToXLS);

            foreach (Wire wire in _wires)
            {
                wire.LineMaterial = _lineMaterial;
                wire.transform.SetParent(transform, true);
            }

            WiringCreated.Invoke(_wires);
        }

        public void DestroyWiring()
        {
            foreach (Wire wire in _wires)
            {
                Destroy(wire.gameObject);
            }

            _wires.Clear();

            WiringDestroyed.Invoke(_wires);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}