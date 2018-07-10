using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EMSP.Mathematic.Magnetic
{
	public class MagneticTensionCalculator : PointableMathematicCalculatorBase
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
        public override bool CheckIntersection(Wiring wiring, Vector3 point)
        {
            foreach (Wire wire in wiring)
            {
                for (int i = 0; i < wire.Count - 1; i++)
                {
                    Vector3 ab = wire[i + 1] - wire[i];
                    Vector3 ac = point - wire[i];

                    if (ab.normalized == ac.normalized && ac.sqrMagnitude <= ab.sqrMagnitude) return true;
                }
            }

            return false;
        }

        public override Data Calculate(Data settings)
        {
            // Extract data.
            AmperageMode amperageMode = settings.GetValue<AmperageMode>("amperageMode");
            Wiring wiring = settings.GetValue<Wiring>("wiring");
            Vector3 point = settings.GetValue<Vector3>("point");

            if (amperageMode == AmperageMode.Precomputed) return new Data() { { "result", CalculateWithPrecomputedAmperage(wiring, point) } };

            float time = settings.GetValue<float>("time");

            return new Data() { { "result", CalculateWithComputationalAmperage(wiring, point, time) } };
        }

        public Vector3 Calculate(Vector3 pointA, Vector3 pointB, Vector3 pointC, float amperage)
        {
            // Calculate AB, AC and BC lengths.
            float abLength = Vector3.Distance(pointA, pointB);
            float acLength = Vector3.Distance(pointA, pointC);
            float bcLength = Vector3.Distance(pointB, pointC);

            // Calculate CAB angle.
            float alpha1Angle = Mathf.Rad2Deg * Mathf.Acos(Mathf.Clamp((((pointC.x - pointA.x) * (pointB.x - pointA.x) + (pointC.y - pointA.y) * (pointB.y - pointA.y) + (pointC.z - pointA.z) * (pointB.z - pointA.z)) / (abLength * acLength)), -1f, 1f));

            // Calculate ABC (inversed) angle.
            float alpha2Angle = Vector3.Angle(pointC - pointB, pointB - pointA);

            // Calculate length of line which start from C and move in perpendicular direction to AB side.
            float perpendicularLength = acLength >= float.MinValue ? acLength * Mathf.Sin(Mathf.Deg2Rad * alpha1Angle) : 0f;

            // Calculate induction.
            float induction = perpendicularLength != 0f ? MAGNETIC_CONSTANT / (4 * Mathf.PI) * amperage / perpendicularLength * (Mathf.Cos(Mathf.Deg2Rad * alpha1Angle) - Mathf.Cos(Mathf.Deg2Rad * alpha2Angle)) : 0f;

            // Calculate induction direction.
            Vector3 inductionDirection = new Vector3
            {
                x = (pointB.y - pointA.y) * (pointC.z - pointA.z) - (pointB.z - pointA.z) * (pointC.y - pointA.y),
                y = (pointB.z - pointA.z) * (pointC.x - pointA.x) - (pointB.x - pointA.x) * (pointC.z - pointA.z),
                z = (pointB.x - pointA.x) * (pointC.y - pointA.y) - (pointB.y - pointA.y) * (pointC.x - pointA.x)
            };

            // Calculate induction length.
            float inductionDirectionLength = inductionDirection.magnitude;

            // Calculate magnet inductions vectors coordinates.
            Vector3 ppInductionDirection = new Vector3
            {
                x = inductionDirectionLength == 0 ? 0f : inductionDirection.x / inductionDirectionLength * induction,
                y = inductionDirectionLength == 0 ? 0f : inductionDirection.y / inductionDirectionLength * induction,
                z = inductionDirectionLength == 0 ? 0f : inductionDirection.z / inductionDirectionLength * induction
            };

            return ppInductionDirection;
        }

        public Vector3 CalculateWithAmperage(Wire wire, Vector3 targetPoint, float amperage)
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