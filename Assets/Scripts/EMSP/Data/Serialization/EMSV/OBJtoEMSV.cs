using OrbCreationExtensions;
using SFB;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EMSV.Data
{
    public class OBJtoEMSV
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
        private Dictionary<string, List<Vector3>> materialVertexesPacks = new Dictionary<string, List<Vector3>>();
        private string _currentHandlingMaterial = "";
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        private static Vector3 GetVector3FromObjString(string str)
        {
            Vector3 vec = new Vector3(0, 0, 0);
            int i = 0;
            for (int elem = 0; elem < 3; elem++)
            {
                int e = str.IndexOf(' ', i);
                if (e < 0) e = str.Length;
                vec[elem] = str.Substring(i, e - i).MakeFloat();
                i = str.EndOfCharRepetition(e);
            }
            return vec;
        }

        public void ExportVertexDataFromOBJtoEMSV(string objFilePathForRead, string emsvFilePathForWrite)
        {
            ReadOBJ(objFilePathForRead);
            WriteToEMSV(emsvFilePathForWrite);
        }

        private void ReadOBJ(string pathToOBJ)
        {
            using (StreamReader sr = new StreamReader(pathToOBJ))
            {
                while(!sr.EndOfStream)
                {
                    string line = sr.ReadLine();
                    if(line.StartsWith("o") || line.StartsWith("O"))
                    {
                        string matName = line.Substring(2);
                        _currentHandlingMaterial = matName;

                        if(!materialVertexesPacks.ContainsKey(_currentHandlingMaterial))
                            materialVertexesPacks.Add(matName, new List<Vector3>());
                    }
                    else if(line.StartsWith("v") || line.StartsWith("V"))
                    {
                        string vertexString = line.Substring(2);
                        materialVertexesPacks[_currentHandlingMaterial].Add(GetVector3FromObjString(vertexString));
                    }
                }
            }
        }
        private void WriteToEMSV(string pathForSave)
        {
            var serializer = new Serialization.EMSV.Versions.EMSVSerializerV1000();

            using (var stream = File.Create(pathForSave))
            {
                byte[] data = serializer.Serialize(materialVertexesPacks);
                stream.Write(data, 0, data.Length);
            }
        }

        #endregion
        #endregion
    }
}