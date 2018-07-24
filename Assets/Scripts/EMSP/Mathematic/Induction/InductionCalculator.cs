using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;

namespace EMSP.Mathematic.Induction
{
    public class InductionCalculator : VectorableMathematicCalculatorBase
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
        public override Data Calculate(Data settings)
        {
            AmperageMode amperageMode = settings.GetValue<AmperageMode>("amperageMode");
            string targetWireName = settings.GetValue<string>("name");
            int targetWireSegment = settings.GetValue<int>("segment");
            Wiring wiring = settings.GetValue<Wiring>("wiring");

            float summaryValue;

            if (amperageMode == AmperageMode.Precomputed) return new Data()
            {
                 { "result", PrecomputeCalculateWiring(targetWireName, targetWireSegment, wiring, 
                 out summaryValue) },

                 { "summary", summaryValue }
            };


            float time = settings.GetValue<float>("time");
            return new Data()
            {
                { "result", CalculateWiring(targetWireName, targetWireSegment, wiring, time, 
                out summaryValue) },

                { "summary", summaryValue }
            };
        }

        private Dictionary<Wire, float> CalculateWiring(string targetWireName, int targetWireSegment, Wiring wiring, float time, out float summaryValue)
        {
            Dictionary<Wire, float> result = new Dictionary<Wire, float>();
            Wire targetWire = wiring.GetWireByName(targetWireName);

            float maxValue = 0;

            for (int wireIndex = 0; wireIndex < wiring.Count; ++wireIndex)
            {
                string currentWireName = wiring[wireIndex].Name;
                if (currentWireName == targetWireName)
                    continue;


                float value = targetWire.Amperage * (wiring[wireIndex].Amplitude + targetWireSegment) * (float)Math.Cos(time * targetWireSegment);
                if (value > maxValue) maxValue = value;

                result.Add(wiring[wireIndex], value);
            }

            summaryValue = maxValue;
            return result;
        }

        private Dictionary<Wire, float> PrecomputeCalculateWiring(string targetWireName, int targetWireSegment, Wiring wiring, out float summaryValue)
        {
            Dictionary<Wire, float> result = new Dictionary<Wire, float>();
            Wire targetWire = wiring.GetWireByName(targetWireName);

            float maxValue = 0;

            for (int wireIndex = 0; wireIndex < wiring.Count; ++wireIndex)
            {
                string currentWireName = wiring[wireIndex].Name;
                if (currentWireName == targetWireName)
                    continue;


                float value = targetWire.Amperage * (wiring[wireIndex].Amplitude + targetWireSegment);
                if (value > maxValue) maxValue = value;

                result.Add(wiring[wireIndex], value);
            }

            summaryValue = maxValue;
            return result;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}