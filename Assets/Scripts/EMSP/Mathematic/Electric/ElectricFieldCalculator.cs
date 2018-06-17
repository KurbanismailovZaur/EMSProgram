using EMSP.Communication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
    public class ElectricFieldCalculator : MathematicCalculator
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
        public override Vector3 Calculate(Vector3 pointA, Vector3 pointB, Vector3 pointC, float amperage)
        {
            return new Magnetic.MagneticTensionCalculator().Calculate(pointA, pointB, pointC, amperage);

            //Wiring.Factory wiringFactory = new Wiring.Factory();

            //Wiring wiring = wiringFactory.Create();

            //#region Create wiring
            //Wire wire0 = wiring.CreateWire("K0", 0, 300000000, 1);
            //wire0.Add(1f, 2f, 3f);
            //wire0.Add(7f, 4f, 7f);
            //wire0.Add(5f, 0f, 3f);
            //wire0.Add(6f, 9f, 4f);
            //wire0.Add(0f, 5f, 9f);
            //wire0.Add(2f, 5f, 3f);
            //wire0.Add(3f, 5f, 2f);
            //wire0.Add(4f, 3f, 1f);

            //Wire wire1 = wiring.CreateWire("K1", 0, 9000, 2);
            //wire1.Add(1f, 0f, 0f);
            //wire1.Add(8f, 9f, 1f);
            //wire1.Add(0f, 6f, 0f);
            //wire1.Add(9f, 3f, 1f);
            //wire1.Add(0f, 2f, 5f);
            //wire1.Add(8f, 7f, 1f);
            //wire1.Add(0f, 9f, 4f);
            //wire1.Add(2f, 9f, 0f);

            //Wire wire2 = wiring.CreateWire("K2", 0, 200000, 0.5f);
            //wire2.Add(2f, 0f, 0f);
            //wire2.Add(6f, 9f, 3f);
            //wire2.Add(9f, 7f, 6f);
            //wire2.Add(7f, 0f, 4f);
            //wire2.Add(9f, 3f, 2f);
            //wire2.Add(1f, 7f, 4f);
            //wire2.Add(3f, 4f, 4f);
            //wire2.Add(4f, 7f, 7f);

            //Wire wire3 = wiring.CreateWire("K3", 0, 1000000, -1);
            //wire3.Add(3f, 0f, 0f);
            //wire3.Add(8f, 2f, 6f);
            //wire3.Add(8f, 2f, 0f);
            //wire3.Add(9f, 1f, 8f);
            //wire3.Add(1f, 9f, 0f);
            //wire3.Add(9f, 8f, 4f);
            //wire3.Add(4f, 8f, 8f);
            //wire3.Add(8f, 7f, 7f);

            //Wire wire4 = wiring.CreateWire("K4", 0, 500000, -1.5f);
            //wire4.Add(3f, 0f, 0f);
            //wire4.Add(18f, 12f, 16f);
            //wire4.Add(18f, 12f, 10f);
            //wire4.Add(19f, 11f, 18f);
            //wire4.Add(11f, 19f, 10f);
            //wire4.Add(19f, 18f, 14f);
            //wire4.Add(14f, 18f, 18f);
            //wire4.Add(18f, 17f, 17f);
            //#endregion

            //return new Vector3();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
