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
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}