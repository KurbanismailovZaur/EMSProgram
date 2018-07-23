using EMSP.Utility;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace EMSP.Communication
{
    public class Wire : MonoBehaviour, IEnumerable<Vector3>
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
            public Wire Create(string name, float amplitude, float frequency, float amperage)
            {
                Wire wire = new GameObject("Wire").AddComponent<Wire>();
                wire._name = name;
                wire._amplitude = amplitude;
                wire._frequency = frequency;
                wire._amperage = amperage;

                return wire;
            }
        }

        [Serializable]
        public class GeometryChangedEvent : UnityEvent<Wire, ReadOnlyCollection<Vector3>> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private string _name;

        private float _amplitude;

        private float _frequency;

        private float _amperage;

        private List<Vector3> _localPoints = new List<Vector3>();

        private Dictionary<int, WireSegment> _segments = new Dictionary<int, WireSegment>();
        #endregion

        #region Events
        public GeometryChangedEvent GeometryChanged = new GeometryChangedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public string Name { get { return _name; } }

        public ReadOnlyCollection<Vector3> LocalPoints { get { return _localPoints.AsReadOnly(); } }

        public ReadOnlyCollection<Vector3> WorldPoints
        {
            get
            {
                List<Vector3> worldPoints = Lists.RepeatedDefault<Vector3>(_localPoints.Count);

                for (int i = 0; i < _localPoints.Count; i++)
                {
                    worldPoints[i] = transform.TransformPoint(_localPoints[i]);
                }

                return worldPoints.AsReadOnly();
            }
        }

        public int Count { get { return _localPoints.Count; } }

        public float Amplitude { get { return _amplitude; } }

        public float Frequency { get { return _frequency; } }

        public float Amperage { get { return _amperage; } }

        #endregion

        #region Constructors
        private Wire() { }
        #endregion

        #region Methods

        public static bool IsCorrectName(string name)
        {
            return name.All(char.IsLetterOrDigit);
        }

        public IEnumerator<Vector3> GetEnumerator()
        {
            return _localPoints.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _localPoints.GetEnumerator();
        }

        public void Add(Vector3 point, Space relativeTo = Space.World)
        {
            if (relativeTo == Space.World)
            {
                _localPoints.Add(transform.InverseTransformPoint(point));
            }
            else
            {
                _localPoints.Add(point);
            }

            OnGeometryChanged();
        }

        public void Add(float x, float y, float z, Space relativeTo = Space.World)
        {
            Add(new Vector3(x, y, z), relativeTo);
        }

        public void AddRange(IEnumerable<Vector3> points, Space relativeTo = Space.World)
        {
            _localPoints.AddRange(points);
            OnGeometryChanged();
        }

        public bool Remove(Vector3 point)
        {
            bool result = _localPoints.Remove(point);
            OnGeometryChanged();

            return result;
        }

        public bool Remove(float x, float y, float z)
        {
            return Remove(new Vector3(x, y, z));
        }

        public void RemoveAt(int index)
        {
            _localPoints.RemoveAt(index);
            OnGeometryChanged();
        }

        public void Insert(int index, Vector3 point)
        {
            _localPoints.Insert(index, point);
            OnGeometryChanged();
        }

        public void Insert(int index, float x, float y, float z)
        {
            _localPoints.Insert(index, new Vector3(x, y, z));
            OnGeometryChanged();
        }

        public void InsertRange(int index, IEnumerable<Vector3> points)
        {
            _localPoints.InsertRange(index, points);
            OnGeometryChanged();
        }

        private void OnGeometryChanged()
        {
            GeometryChanged.Invoke(this, LocalPoints);
            UpdateSegments();
        }

        private void UpdateSegments()
        {
            int segmetsCount = _localPoints.Count - 1;


            for (int i = 0; i < _segments.Count; ++i)
            {
                Destroy(_segments[i].gameObject);
            }
            transform.DetachChildren();
            _segments.Clear();
            if (segmetsCount < 1)
                return;


            var segmentFactory = new WireSegment.Factory(this);
            for (int segmentIndex = 0; segmentIndex < segmetsCount; ++segmentIndex)
            {
                var segment = segmentFactory.Create(
                    transform.TransformPoint(_localPoints[segmentIndex]),
                    transform.TransformPoint(_localPoints[segmentIndex + 1]),
                    segmentIndex
                    );
                segment.transform.SetParent(transform, true);

                _segments.Add(segmentIndex, segment);
            }
        }

        public Bounds GetBounds()
        {
            if (_localPoints.Count == 0)
            {
                return new Bounds(transform.position, Vector3.zero);
            }

            Bounds bounds = new Bounds(transform.TransformPoint(_localPoints[0]), Vector3.zero);

            for (int i = 1; i < _localPoints.Count; i++)
            {
                bounds.Encapsulate(transform.TransformPoint(_localPoints[i]));
            }

            return bounds;
        }

        public WireSegment GetSegment(int segmentNumber)
        {
            return _segments[segmentNumber];
        }
        #endregion

        #region Indexers
        public Vector3 this[int index]
        {
            get { return _localPoints[index]; }
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}