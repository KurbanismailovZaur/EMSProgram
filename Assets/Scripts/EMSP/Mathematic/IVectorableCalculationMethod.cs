using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Mathematic
{
    public interface IVectorableCalculationMethod : ICalculationMethod
    {
        void FilterVectorsByValue(Range range);
    }
}
