﻿using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EMSP.Mathematic.MagneticTension
{
	public class MagneticTensionCalculator : MathematicBase
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
        public Vector3 Calculate(Vector3 pointA, Vector3 pointB, Vector3 pointC, float amperage)
        {
            // Calculate AB, AC and BC lengths.
            float abLength = Vector3.Distance(pointA, pointB);
            float acLength = Vector3.Distance(pointA, pointC);
            float bcLength = Vector3.Distance(pointB, pointC);

            // Calculate CAB angle.
            float alpha1Angle = Mathf.Rad2Deg * Mathf.Acos((((pointC.x - pointA.x) * (pointB.x - pointA.x) + (pointC.y - pointA.y) * (pointB.y - pointA.y) + (pointC.z - pointA.z) * (pointB.z - pointA.z)) / (abLength * acLength)));

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

        public MagneticTensionDirectionResult Calculate(Wire wire, Vector3 targetPoint, float time)
        {
            float calculatedAmperage = wire.Amplitude * Mathf.Sin(2 * Mathf.PI * wire.Frequency * time);

            ReadOnlyCollection<Vector3> points = wire.WorldPoints;

            MagneticTensionDirectionResult magneticTensionDirectionResult = new MagneticTensionDirectionResult();
            for (int i = 0; i < points.Count - 1; i++)
            {
                magneticTensionDirectionResult.CalculatedAmperageResult += Calculate(points[i], points[i + 1], targetPoint, calculatedAmperage);
                magneticTensionDirectionResult.PrecomputedAmperageResult += Calculate(points[i], points[i + 1], targetPoint, wire.Amperage);
            }

            return magneticTensionDirectionResult;
        }

        public MagneticTensionResult Calculate(Wiring wires, Vector3 point, float time)
        {
            MagneticTensionDirectionResult magneticTensionDirectionResult = new MagneticTensionDirectionResult();

            foreach (Wire wire in wires)
            {
                magneticTensionDirectionResult += Calculate(wire, point, time);
            }

            return new MagneticTensionResult(magneticTensionDirectionResult.CalculatedAmperageResult.magnitude, magneticTensionDirectionResult.PrecomputedAmperageResult.magnitude);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}