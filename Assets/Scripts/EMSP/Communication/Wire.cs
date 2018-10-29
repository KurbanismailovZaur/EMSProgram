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

        public class WireSegmentMath
        {
            public readonly Vector3 PointA;
            public readonly Vector3 PointB;
            public readonly Vector3 Vector;
            public readonly float Lenght;

            public WireSegmentMath(Vector3 a, Vector3 b)
            {
                PointA = a;
                PointB = b;

                //Vector3 vector = new Vector3();
                //for (int i = 0; i < 3; i++)
                //{
                //    vector[i] = b[i] - a[i];
                //}

                Vector = b - a;
                Lenght = Vector3.Distance(a, b);
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

        private List<WireSegmentMath> _segmentsMath = new List<WireSegmentMath>();

        private Dictionary<int, WireSegmentVisual> _segmentsVisual = new Dictionary<int, WireSegmentVisual>();
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

        public ReadOnlyCollection<WireSegmentVisual> SegmentsVisual
        {
            get
            {
                return _segmentsVisual.Values.ToList().AsReadOnly();
            }
        }

        public List<WireSegmentMath> SegmentsMath { get { return _segmentsMath; } }

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


            for (int i = 0; i < _segmentsVisual.Count; ++i)
            {
                Destroy(_segmentsVisual[i].gameObject);
            }
            transform.DetachChildren();
            _segmentsVisual.Clear();
            _segmentsMath.Clear();

            if (segmetsCount < 1)
                return;


            var segmentFactory = new WireSegmentVisual.Factory(this);
            for (int segmentIndex = 0; segmentIndex < segmetsCount; ++segmentIndex)
            {
                var segmentVisual = segmentFactory.Create(
                    transform.TransformPoint(_localPoints[segmentIndex]),
                    transform.TransformPoint(_localPoints[segmentIndex + 1]),
                    segmentIndex
                    );
                segmentVisual.transform.SetParent(transform, true);
                _segmentsVisual.Add(segmentIndex, segmentVisual);

                var segmentMath = new WireSegmentMath(_localPoints[segmentIndex], _localPoints[segmentIndex + 1]);
                _segmentsMath.Add(segmentMath);
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

        public WireSegmentVisual GetVisualSegment(int segmentNumber)
        {
            return _segmentsVisual[segmentNumber];
        }

        public void SetWireHighlight(bool value)
        {
            if(value)
            {
                foreach(var segment in _segmentsVisual.Values)
                {
                    segment.SetHighlight(Color.green);
                }
            }
            else
            {
                foreach (var segment in _segmentsVisual.Values)
                {
                    segment.DisableHighlight();
                }
            }
        }
        #endregion

        #region Indexers
        public Vector3 this[int index]
        {
            get { return _localPoints[index]; }
        }

        public static bool operator == (Wire a, Wire b)
        {
            return a.Name == b.Name;
        }

        public static bool operator !=(Wire a, Wire b)
        {
            return a.Name != b.Name;
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}