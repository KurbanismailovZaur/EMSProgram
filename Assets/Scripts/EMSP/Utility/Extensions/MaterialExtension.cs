using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Utility.Extensions
{
	public static class MaterialExtensions
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
        public static void SetRGB(this Material material, float r, float g, float b)
        {
            Color color = material.color;
            color.r = r;
            color.g = g;
            color.b = b;

            material.color = color;
        }

        public static void SetRGB(this Material material, Vector3 rgbVector)
        {
            Color color = material.color;
            color.r = rgbVector.x;
            color.g = rgbVector.y;
            color.b = rgbVector.z;

            material.color = color;
        }

        public static void SetAlpha(this Material material, float value)
        {
            Color color = material.color;
            color.a = value;

            material.color = color;
        }
		#endregion
		
		#region Indexers
		#endregion
		
		#region Events handlers
		#endregion
		#endregion
	}
}