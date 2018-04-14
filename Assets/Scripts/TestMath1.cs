using EMSP.Mathematic.MagneticTension;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Mynamespace
{
	public class TestMath1 : MonoBehaviour 
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
        private void Awake()
        {
            MagneticTensionCalculator mtCalculator = new MagneticTensionCalculator();

            Vector3 a = Vector3.zero;
            Vector3 b = new Vector3(-1f, 1f, -1f);
            Vector3 c = new Vector3(-0.09523785f, 0.09523821f, 0.09523821f);

            Vector3 result = mtCalculator.Calculate(a, b, c, 2f);
            print(result);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}