using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Communication
{
	public class Wire 
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
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public List<Segment> Segments { get; set; }
        #endregion

        #region Constructors
        public Wire() : this(new Segment[0]) { }

        public Wire(IEnumerable<Segment> segments)
        {
            Segments = new List<Segment>(segments);
        }
        #endregion

        #region Methods
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