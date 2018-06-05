using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
	public interface IPointableCalculationMethod : ICalculationMethod
	{
        void FilterPointsByValue(Range range);
    }
}
