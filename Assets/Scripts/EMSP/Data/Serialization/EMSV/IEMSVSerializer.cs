using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EMSP.Data.Serialization.EMSV
{
    public interface IEMSVSerializer
    {
        Version Version { get; }

        byte[] Serialize(Dictionary<string, List<Vector3>> materialVertexPacks);

        Dictionary<string, List<Vector3>> Deserialize(Stream stream);
    }
}