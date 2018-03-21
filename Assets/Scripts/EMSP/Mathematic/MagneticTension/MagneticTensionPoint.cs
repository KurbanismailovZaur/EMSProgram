using EMSP.Utility.Extensions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic.MagneticTension
{
	public class MagneticTensionPoint : MonoBehaviour 
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
            public MagneticTensionPoint Create(PrimitiveType primitiveType, Transform parent, Material material, float size, float alpha, MagneticTensionInSpace.MagneticTensionsInfo mtInfo)
            {
                MagneticTensionPoint magneticTensionPoint = GameObject.CreatePrimitive(primitiveType).AddComponent<MagneticTensionPoint>();
                magneticTensionPoint.name = "Point";
                magneticTensionPoint.transform.position = mtInfo.Point;
                magneticTensionPoint.transform.localScale = new Vector3(size, size, size);
                magneticTensionPoint.transform.SetParent(parent, true);

                Renderer magneticTensionRenderer = magneticTensionPoint.GetComponent<Renderer>();

                magneticTensionPoint._material = new Material(material);
                magneticTensionPoint._material.SetAlpha(alpha);

                magneticTensionRenderer.material = magneticTensionPoint._material;

                magneticTensionPoint._magneticTension = mtInfo.MagneticTension;

                return magneticTensionPoint;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private Material _material;

        private float _magneticTension;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public float MagneticTension { get { return _magneticTension; } }

        public float Alpha { get { return _material.color.a; } }
        #endregion

        #region Constructors
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