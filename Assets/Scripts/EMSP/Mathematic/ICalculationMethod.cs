using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.Mathematic
{
	public interface ICalculationMethod
	{
        bool IsVisible { get; set; }

        bool IsCalculated { get; }

        CalculationType Type { get; }

        float CurrentModeMaxCalculatedValue { get; }

        void SetEntitiesToTime(int index); // points or vectors(wires)

        AmperageMode AmperageMode { get; set; }
    }
}
