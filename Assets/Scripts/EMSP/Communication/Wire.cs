using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EMSP.Communication
{
	public class Wire : IEnumerable<Segment>
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
        private List<Segment> _segments = new List<Segment>();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public ReadOnlyCollection<Segment> Segments { get { return _segments.AsReadOnly(); } }
        #endregion

        #region Constructors
        public Wire() : this(new Segment[0]) { }

        public Wire(IEnumerable<Segment> segments)
        {
            _segments.AddRange(segments);
        }
        #endregion

        #region Methods
        public IEnumerator<Segment> GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _segments.GetEnumerator();
        }

        public void Add(Segment segment)
        {
            _segments.Add(segment);
        }

        public void Add(Vector3 pointA, Vector3 pointB)
        {
            Add(new Segment(pointA, pointB));
        }

        public List<Vector3> GetSequentialPoints()
        {
            List<Vector3> points = new List<Vector3>();

            foreach (Segment segment in Segments)
            {
                points.Add(segment.pointA);
            }

            if (Segments.Count != 0)
            {
                points.Add(Segments[Segments.Count - 1].pointB);
            }

            return points;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}