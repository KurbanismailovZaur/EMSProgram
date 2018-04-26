using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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

        public ReadOnlyCollection<Wire> Wires { get { return _wires.AsReadOnly(); } }

        public int Count { get { return _wires.Count; } }
        #endregion

        #region Constructors
        private Wiring() { }
        #endregion

        #region Methods
        public Wire CreateWire(string name, float amplitude, float frequency, float amperage)
        {
            Wire wire = _wireFactory.Create(name, amplitude, frequency, amperage);
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

        public Bounds GetBounds()
        {
            if (_wires.Count == 0)
            {
                return new Bounds(transform.position, Vector3.zero);
            }

            Bounds bounds = _wires[0].GetBounds();

            for (int i = 1; i < _wires.Count; i++)
            {
                bounds.Encapsulate(_wires[i].GetBounds());
            }

            return bounds;
        }

        public bool CheckPointsExist()
        {
            if (_wires.Count == 0)
            {
                return false;
            }

            for (int i = 0; i < _wires.Count; i++)
            {
                if (_wires[i].Count != 0)
                {
                    return true;
                }
            }

            return false;
        }
        #endregion

        #region Indexers
        public Wire this[int index]
        {
            get { return _wires[index]; }
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}