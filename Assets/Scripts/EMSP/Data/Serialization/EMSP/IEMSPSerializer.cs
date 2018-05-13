using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EMSP.Data.Serialization.EMSP
{
    public interface IEMSPSerializer
    {
        Version Version { get; }

        byte[] Serialize();

        EMSPSerializerVersion.SerializableProjectBatch Deserialize(Stream stream);
    }
}