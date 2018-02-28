using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestImport : MonoBehaviour
{
    public string Path = "";

    [ContextMenu("Import")]
    void Import()
    {
        var go = new ObjModelImporter().ImportModel(Path);
    }
}
