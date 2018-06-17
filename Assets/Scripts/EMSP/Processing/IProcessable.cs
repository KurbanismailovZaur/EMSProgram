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

        event Action<IProcessable, float> ProgressChanged;

        event Action<IProcessable, string> ProgressNameChanged;

        event Action<IProcessable> ProgressCanceled;

        void Cancel();
    }
}
