using EMSP.Logging;
using Numba.Geometry;
using OrbCreationExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EMSP.Data.OBJ
{
    public class OBJImporter
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
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public GameObject Import(string pathToOBJ)
        {
            GameObject modelObject = new GameObject("Model");

            GameObject obj = ObjImporter.Import(File.ReadAllText(pathToOBJ));

            obj.name = Path.GetFileNameWithoutExtension(pathToOBJ);

            Bounds bounds = BoundsUtility.GetGlobalBounds(obj, BoundsUtility.BoundsCreateOption.Mesh);

            modelObject.transform.position = bounds.center;
            obj.transform.SetParent(modelObject.transform);

            return modelObject;
        }

        public GameObject Import(string pathToOBJ, out Material[] materials)
        {
            string mtlLibKeyword = "mtllib";

            string objText = File.ReadAllText(pathToOBJ);
            string mtlPath = "";

            StringReader reader = new StringReader(objText);
            do
            {
                string line = reader.ReadLine();
                if (line.StartsWith(mtlLibKeyword))
                {
                    mtlPath = line.Substring(mtlLibKeyword.Length + 1);
                    if (!Path.IsPathRooted(mtlPath))
                    {
                        mtlPath = Path.Combine(Path.GetDirectoryName(pathToOBJ), mtlPath);
                        if (!File.Exists(mtlPath))
                        {
                            Debug.LogError("Incorrect path to MTL file. Start importing without materials.");
                            materials = new Material[0];
                            return Import(pathToOBJ);
                        }
                    }
                    break;
                }
            } while (reader.Peek() != -1);
            reader.Dispose();


            string mtlText = File.ReadAllText(mtlPath);
            List<Texture2D> TexturesList = new List<Texture2D>();

            reader = new StringReader(mtlText);
            do
            {
                string line = reader.ReadLine();
                if (line.StartsWith("map_Kd"))
                {
                    Texture2D texture = new Texture2D(1, 1);

                    string path = "";
                    string[] chunks = null;
                    chunks = line.Split(' ');

                    if (Path.IsPathRooted(chunks[chunks.Length - 1]))
                    {
                        path = chunks[chunks.Length - 1];
                    }
                    else
                    {
                        path = Path.Combine(Path.GetDirectoryName(pathToOBJ), chunks[chunks.Length - 1]);
                    }

                    if (!File.Exists(path))
                    {
                        Debug.LogError("Incorrect path to texture:" + path);
                        break;
                    }
                    var extension = Path.GetExtension(path);

                    if (extension == ".jpg" || extension == ".png")
                    {
                        texture.LoadImage(File.ReadAllBytes(path));
                        texture.Apply();
                        TexturesList.Add(texture);
                    }
                    else
                    {
                        Debug.LogError("Invalid mainTexture file extension");
                    }
                }

            } while (reader.Peek() != -1);
            reader.Dispose();

            GameObject modelObject = new GameObject("Model");

            GameObject obj = ObjImporter.Import(objText, mtlText, TexturesList.ToArray());

            obj.name = Path.GetFileNameWithoutExtension(pathToOBJ);

            Bounds bounds = BoundsUtility.GetGlobalBounds(obj, BoundsUtility.BoundsCreateOption.Mesh);

            modelObject.transform.position = bounds.center;
            obj.transform.SetParent(modelObject.transform);

            List<Material> _materials = new List<Material>();
            foreach (var renderer in obj.GetComponentsInChildren<Renderer>())
            {
                if (renderer.sharedMaterials.Length > 0)
                {
                    _materials.AddRange(renderer.sharedMaterials);
                }
            }
            materials = _materials.ToArray();

            return modelObject;
        }

        public Dictionary<string, List<Vector3>> GetVerticesInfoFromOBJ(string pathToOBJ, Func<bool> cancellationExpression = null)
        {
            if (Log.IsInitialized)
                Log.WriteOperation("Started_OBJImporter_GetVerticesInfoFromOBJ");

            Dictionary<string, List<Vector3>> materialVertexesPacks = new Dictionary<string, List<Vector3>>();

            using (StreamReader sr = new StreamReader(pathToOBJ))
            {
                List<Vector3> vertices = new List<Vector3>();

                string materialName = null;
                List<int> materialVerticesIndexes = new List<int>();

                while (!sr.EndOfStream)
                {
                    if (cancellationExpression != null && cancellationExpression()) return null;

                    string line = sr.ReadLine();

                    if (line.StartsWith("v "))
                    {
                        string vertexString = line.Substring(2);
                        vertices.Add(GetVector3FromObjString(vertexString));
                    }
                    else if (line.StartsWith("usemtl "))
                    {
                        if (materialName != null)
                            if (!materialVertexesPacks.ContainsKey(materialName))
                                materialVertexesPacks.Add(materialName, IndexesToVertices(vertices, materialVerticesIndexes));
                            else
                                materialVertexesPacks[materialName].AddRange(IndexesToVertices(vertices, materialVerticesIndexes));

                        materialName = line.Substring(7);
                        materialVerticesIndexes.Clear();
                    }
                    else if (line.StartsWith("f "))
                    {
                        string[] indexes = line.Substring(2).Split(' ');

                        for (int i = 0; i < indexes.Length; i++)
                        {
                            int index = GetFirstNumber(indexes[i]);
                            if (!materialVerticesIndexes.Contains(index)) materialVerticesIndexes.Add(index);
                        }
                    }
                }

                if (!materialVertexesPacks.ContainsKey(materialName))
                    materialVertexesPacks.Add(materialName, IndexesToVertices(vertices, materialVerticesIndexes));
            }

            return materialVertexesPacks;
        }

        private int GetFirstNumber(string str)
        {
            int i = 0;
            while (char.IsNumber(str[i])) { ++i; };

            return int.Parse(str.Substring(0, i));
        }

        private Vector3 GetVector3FromObjString(string str)
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

        private List<Vector3> IndexesToVertices(List<Vector3> vertices, List<int> indexes)
        {
            List<Vector3> indexesVertices = new List<Vector3>(indexes.Count);

            for (int i = 0; i < indexes.Count; i++)
            {
                indexesVertices.Add(vertices[indexes[i] - 1]);
            }

            return indexesVertices;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}