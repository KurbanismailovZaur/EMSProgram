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
            string targetWireName = settings.GetValue<string>("name");
            int targetWireSegment = settings.GetValue<int>("segment");
            Wiring wiring = settings.GetValue<Wiring>("wiring");

            return new Data() { { "result", CalculateWiring(targetWireName, targetWireSegment, wiring) } };
        }

        private Dictionary<string, float> CalculateWiring(string targetWireName, int targetWireSegment, Wiring wiring)
        {
            Dictionary<string, float> result = new Dictionary<string, float>();
            Wire targetWire = wiring.GetWireByName(targetWireName);

            for(int wireIndex = 0; wireIndex < wiring.Count; ++wireIndex)
            {
                string currentWireName = wiring[wireIndex].Name;
                if (currentWireName == targetWireName)
                    continue;

                result.Add(currentWireName, targetWire.Amperage * (wiring[wireIndex].Amplitude + targetWireSegment));
            }

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