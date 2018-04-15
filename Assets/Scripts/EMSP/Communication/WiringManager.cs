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
        public class WiringCreatedEvent : UnityEvent<Wiring> { }

        [Serializable]
        public class WiringDestroyedEvent : UnityEvent<Wiring> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private WiringDataReader _wiringDataReader = new WiringDataReader();

        [SerializeField]
        private Material _lineMaterial;

        private Wiring _wiring;
        #endregion

        #region Events
        public WiringCreatedEvent WiringCreated = new WiringCreatedEvent();

        public WiringDestroyedEvent WiringDestroyed = new WiringDestroyedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public Wiring Wiring { get { return _wiring; } }

        //public bool IsWiringVisible
        //{
        //    get { return _wires[0].gameObject.activeSelf; }
        //    set
        //    {
        //        if (_wires[0].gameObject.activeSelf == value)
        //        {
        //            return;
        //        }

        //        foreach (Wire wire in _wires)
        //        {
        //            wire.gameObject.SetActive(value);
        //        }

        //        WiringVisibilityChanged.Invoke();
        //    }
        //}
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void CreateNewWiring(string pathToXLS)
        {
            Wiring wiring;

            if (_wiringDataReader.ReadWiringFromFile(pathToXLS, out wiring))
            {
                CreateNewWiring(wiring);
            }
        }

        public void CreateNewWiring(Wiring wiring)
        {
            DestroyWiring();

            if (!wiring.CheckPointsExist())
            {
                return;
            }

            _wiring = wiring;
            _wiring.SetWiringMaterial(_lineMaterial);
            _wiring.transform.SetParent(transform, true);

            WiringCreated.Invoke(_wiring);
        }

        public void DestroyWiring()
        {
            if (!_wiring)
            {
                return;
            }

            Destroy(_wiring.gameObject);
            _wiring = null;

            WiringDestroyed.Invoke(_wiring);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}