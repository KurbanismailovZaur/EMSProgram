using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EMSP.Data.OBJ
{
	public class OBJImporter 
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
		#endregion
		
		#region Constructors
		#endregion
		
		#region Methods
        public GameObject Import(string pathToOBJ)
        {
            GameObject objObject = ObjImporter.Import(File.ReadAllText(pathToOBJ));
            objObject.name = string.Format("Model [{0}]", Path.GetFileNameWithoutExtension(pathToOBJ));

            return objObject;
        }
		#endregion
		
		#region Indexers
		#endregion
		
		#region Events handlers
		#endregion
		#endregion
	}
}