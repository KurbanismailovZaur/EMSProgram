using EMSP.App;
using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EMSP.Editor
{
	public class MenuExtensions 
	{
        [MenuItem("EMSP/Exclude/Vertices From OBJ File")]
		private static void ExcludeVertices()
        {
            GameFacade.Instance.ExportVerticesFromOBJ();
        }
	}
}