using EMSP.Data.XLS;
using Numba;
using System.Collections;
using System.Collections.Generic;
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
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void CreateNewWiring(string pathToXLS)
        {
            List<Wire> wires = _wiringDataReader.ReadWiringFromFile(pathToXLS);
        }
		#endregion
		
		#region Indexers
		#endregion
		
		#region Events handlers
		#endregion
		#endregion
	}
}