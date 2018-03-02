using Numba.Geometry;
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
            GameObject modelObject = new GameObject("Model");

            GameObject obj = ObjImporter.Import(File.ReadAllText(pathToOBJ));
            
            obj.name = Path.GetFileNameWithoutExtension(pathToOBJ);

            Bounds bounds = BoundsUtility.GetGlobalBounds(obj, BoundsUtility.BoundsCreateOption.Mesh);

            modelObject.transform.position = bounds.center;
            obj.transform.SetParent(modelObject.transform);

            return modelObject;
        }
		#endregion
		
		#region Indexers
		#endregion
		
		#region Events handlers
		#endregion
		#endregion
	}
}