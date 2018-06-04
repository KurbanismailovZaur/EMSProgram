using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EMSP.Mathematic
{
	public abstract class MathematicCalculator : MathematicCalculatorBase 
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
        public abstract Vector3 Calculate(Vector3 pointA, Vector3 pointB, Vector3 pointC, float amperage);

        private Vector3 CalculateWithAmperage(Wire wire, Vector3 targetPoint, float amperage)
        {
            ReadOnlyCollection<Vector3> points = wire.WorldPoints;

            Vector3 directionResult = new Vector3();

            for (int i = 0; i < points.Count - 1; i++)
            {
                directionResult += Calculate(points[i], points[i + 1], targetPoint, amperage);
            }

            return directionResult;
        }

        public Vector3 CalculateWithComputationalAmperage(Wire wire, Vector3 targetPoint, float time)
        {
            float calculatedAmperage = wire.Amplitude * Mathf.Sin(2 * Mathf.PI * wire.Frequency * time);

            return CalculateWithAmperage(wire, targetPoint, calculatedAmperage);
        }

        public Vector3 CalculateWithPrecomputedAmperage(Wire wire, Vector3 targetPoint)
        {
            return CalculateWithAmperage(wire, targetPoint, wire.Amperage);
        }

        private float CalculateWholeWiring(Wiring wires, Vector3 point, Func<Wire, Vector3, Vector3> calculationMethodSelector)
        {
            Vector3 directionResult = new Vector3();

            foreach (Wire wire in wires)
            {
                directionResult += calculationMethodSelector.Invoke(wire, point);
            }

            return directionResult.magnitude;
        }

        public float CalculateWithComputationalAmperage(Wiring wires, Vector3 point, float time)
        {
            return CalculateWholeWiring(wires, point, (w, p) => { return CalculateWithComputationalAmperage(w, p, time); });
        }

        public float CalculateWithPrecomputedAmperage(Wiring wires, Vector3 point)
        {
            return CalculateWholeWiring(wires, point, (w, p) => { return CalculateWithPrecomputedAmperage(w, p); });
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
