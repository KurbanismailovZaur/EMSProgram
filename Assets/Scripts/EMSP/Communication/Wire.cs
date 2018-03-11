using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EMSP.Communication
{
    [RequireComponent(typeof(LineRenderer))]
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
            public Wire Create()
            {
                Wire wire = new GameObject("Wire").AddComponent<Wire>();

                wire._lineRenderer = wire.GetComponent<LineRenderer>();
                wire._lineRenderer.widthMultiplier = 0.02f;
                wire._lineRenderer.numCornerVertices = 4;
                wire._lineRenderer.numCapVertices = 4;

                return wire;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private LineRenderer _lineRenderer;

        List<Vector3> _points = new List<Vector3>();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public ReadOnlyCollection<Vector3> Points { get { return _points.AsReadOnly(); } }

        public Material LineMaterial
        {
            get { return _lineRenderer.sharedMaterial; }
            set { _lineRenderer.sharedMaterial = value; }
        }
        #endregion

        #region Constructors
        private Wire() { }
        #endregion

        #region Methods
        public IEnumerator<Vector3> GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _points.GetEnumerator();
        }

        public void Add(Vector3 point)
        {
            _points.Add(point);
            UpdateLineRendererPoints();
        }

        public void AddRange(IEnumerable<Vector3> points)
        {
            _points.AddRange(points);
            UpdateLineRendererPoints();
        }

        public void Add(float x, float y, float z)
        {
            Add(new Vector3(x, y, z));
        }

        private void UpdateLineRendererPoints()
        {
            _lineRenderer.positionCount = _points.Count;
            _lineRenderer.SetPositions(_points.ToArray());
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}