﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Communication
{
    public class Wiring : MonoBehaviour, IEnumerable<Wire>
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        public class Factory
        {
            public Wiring Create()
            {
                return new GameObject("Wiring").AddComponent<Wiring>();
            }
        }

        [Serializable]
        public class VisibilityChangedEvent : UnityEvent<Wiring, bool> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private Wire.Factory _wireFactory = new Wire.Factory();

        private List<Wire> _wires = new List<Wire>();

        private bool _isVisible = true;
        #endregion

        #region Events
        public VisibilityChangedEvent VisibilityChanged = new VisibilityChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public bool IsVisible
        {
            get { return _isVisible; }
            set
            {
                if (_isVisible == value)
                {
                    return;
                }

                gameObject.SetActive(value);

                _isVisible = value;

                VisibilityChanged.Invoke(this, _isVisible);
            }
        }
        #endregion

        #region Constructors
        private Wiring() { }
        #endregion

        #region Methods
        public Wire CreateWire()
        {
            Wire wire = _wireFactory.Create();
            wire.transform.SetParent(transform, false);

            _wires.Add(wire);

            return wire;
        }

        public void SetWiringMaterial(Material material)
        {
            foreach (Wire wire in _wires)
            {
                wire.LineMaterial = material;
            }
        }

        public IEnumerator<Wire> GetEnumerator()
        {
            return _wires.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _wires.GetEnumerator();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}