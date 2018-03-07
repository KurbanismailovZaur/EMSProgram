using EMSP.Data.XLS;
using Numba;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

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
        #endregion

        #region Behaviour
        #region Properties
        public ReadOnlyCollection<Wire> Wires { get { return _wires.AsReadOnly(); } }
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
                LineRenderer lineRenderer = new GameObject("Wire").AddComponent<LineRenderer>();
                lineRenderer.widthMultiplier = 0.2f;
                lineRenderer.numCornerVertices = 4;
                lineRenderer.numCapVertices = 4;

                Vector3[] points = wire.GetSequentialPoints().ToArray();
                lineRenderer.positionCount = points.Length;
                lineRenderer.SetPositions(points);

                lineRenderer.sharedMaterial = _lineMaterial;

                lineRenderer.transform.SetParent(transform, true);
            }
        }

        public void DestroyWiring()
        {
            List<Transform> childs = new List<Transform>();

            foreach (Transform child in transform)
            {
                childs.Add(child);
            }

            foreach (Transform child in childs)
            {
                Destroy(child.gameObject);
            }
        }
		#endregion
		
		#region Indexers
		#endregion
		
		#region Events handlers
		#endregion
		#endregion
	}
}