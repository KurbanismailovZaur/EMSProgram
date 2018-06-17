using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Processing
{
    public interface IProcessable
    {
        float Progress { get; }

        string ProgressName { get; }

        event Action<float> ProgressChanged;

        event Action<string> ProgressNameChanged;
    }
}
