﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EMSP.Data.Serialization.EMSP
{
    public interface IEMSPSerializer
    {
        byte[] Serialize();

        EMSPSerializerVersion.SerializibleProjectBatch Deserialize(Stream stream);

        Version Version { get; }
    }
}