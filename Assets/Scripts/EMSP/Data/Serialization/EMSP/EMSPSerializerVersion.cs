using EMSP.Communication;
using EMSP.Mathematic.Electric;
using EMSP.Mathematic.Magnetic;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using UnityEngine;
using UnityApplication = UnityEngine.Application;

namespace EMSP.Data.Serialization
{
    public abstract class EMSPSerializerVersion
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        public struct SerializableProjectSettings
        {
            public readonly int RangeLength;

            public readonly Range TimeRange;

            public readonly int TimeStepsCount;

            public SerializableProjectSettings(int rangeLength, Range timeRange, int timeStepsCount)
            {
                RangeLength = rangeLength;
                TimeRange = timeRange;
                TimeStepsCount = timeStepsCount;
            }
        }
        #endregion

        #region Classes
        public class SerializableProjectBatch
        {
            public readonly SerializableProjectSettings ProjectSettings;

            public readonly GameObject ModelGameObject;

            public readonly Wiring Wiring;

            public readonly MagneticTension.PointsInfo MagneticTensionPointsInfo;
            public readonly ElectricField.PointsInfo ElectricFieldPointsInfo;

            public SerializableProjectBatch(SerializableProjectSettings settings, Model model, Wiring wiring, MagneticTension.PointsInfo mtPointsInfo, ElectricField.PointsInfo efPointsInfo) : this(settings, model != null ? model.gameObject : null, wiring, mtPointsInfo, efPointsInfo) { }

            public SerializableProjectBatch(SerializableProjectSettings settings, GameObject modelGameObject, Wiring wiring, MagneticTension.PointsInfo mtPointsInfo, ElectricField.PointsInfo efPointsInfo)
            {
                ProjectSettings = settings;
                ModelGameObject = modelGameObject;
                Wiring = wiring;
                MagneticTensionPointsInfo = mtPointsInfo;
                ElectricFieldPointsInfo = efPointsInfo;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        protected string TemporaryFileName { get { return string.Format("{0}/serializedata.emsp", UnityApplication.temporaryCachePath); } }

        // 64 bytes preamble for file
        private readonly string _preamble = @"H6FIPXTYXqDxCT92Y@$gsOfQ$301nGQT0BRtcqO80e6x3dnNh9TFiS3lsxCOfp$x";
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        public abstract byte[] Serialize(SerializableProjectBatch serializableProjectBatch);

        public abstract SerializableProjectBatch Deserialize(Stream stream);

        #region Binary operation methods
        protected void WritePreamble(BinaryWriter writer)
        {
            writer.Write(Encoding.ASCII.GetBytes(_preamble));
        }

        protected void WriteVersion(BinaryWriter writer, Version version)
        {
            writer.Write(version.Major);
            writer.Write(version.Minor);
            writer.Write(version.Build);
            writer.Write(version.Revision);
        }

        protected void WritePreambleAndVersion(BinaryWriter writer, Version version)
        {
            WritePreamble(writer);
            WriteVersion(writer, version);
        }

        protected void ExcludeData(GameObject target, List<Material> materials, List<Texture2D> textures, List<Shader> shaders, List<Mesh> meshes)
        {
            MeshRenderer meshRenderer = target.GetComponent<MeshRenderer>();
            MeshFilter meshFilter = target.GetComponent<MeshFilter>();

            if (meshRenderer != null)
            {
                foreach (Material material in meshRenderer.sharedMaterials)
                {
                    if (materials.Contains(material))
                    {
                        continue;
                    }

                    materials.Add(material);

                    if (!shaders.Contains(material.shader))
                    {
                        shaders.Add(material.shader);
                    }

                    if (material.mainTexture)
                    {
                        if (!textures.Contains((Texture2D)material.mainTexture))
                        {
                            textures.Add((Texture2D)material.mainTexture);
                        }
                    }
                }
            }

            if (meshFilter)
            {
                Mesh mesh = meshFilter.sharedMesh;
                if (mesh)
                {
                    if (!meshes.Contains(mesh))
                    {
                        meshes.Add(mesh);
                    }
                }
            }

            foreach (Transform child in target.transform)
            {
                ExcludeData(child.gameObject, materials, textures, shaders, meshes);
            }
        }

        protected void WriteArray<T>(BinaryWriter writer, T[] array)
        {
            writer.Write(array.Length);

            byte[] arrayBytes = ArrayToBytes(array);

            writer.Write(arrayBytes);
        }

        protected void WriteStringAsUnicode(BinaryWriter writer, string data)
        {
            byte[] dataBytes = Encoding.Unicode.GetBytes(data);

            writer.Write(dataBytes.Length);
            writer.Write(dataBytes);
        }

        protected void WriteStringArrayAsUnicode(BinaryWriter writer, string[] stringArray)
        {
            writer.Write(stringArray.Length);

            foreach (string data in stringArray)
            {
                WriteStringAsUnicode(writer, data);
            }
        }

        protected byte[] ArrayToBytes<T>(T[] array)
        {
            int arrayByteSize = array.Length * Marshal.SizeOf(typeof(T));
            byte[] arrayBytes = new byte[arrayByteSize];

            IntPtr pointerToArray = Marshal.AllocHGlobal(arrayByteSize);
            long currentElementAddress = pointerToArray.ToInt64();

            for (int j = 0; j < array.Length; j++)
            {
                Marshal.StructureToPtr(array[j], new IntPtr(currentElementAddress), true);
                currentElementAddress = currentElementAddress + Marshal.SizeOf(typeof(T));
            }

            Marshal.Copy(pointerToArray, arrayBytes, 0, arrayBytes.Length);
            Marshal.FreeHGlobal(pointerToArray);

            return arrayBytes;
        }

        protected void WriteList<T>(BinaryWriter writer, List<T> list)
        {
            writer.Write(list.Count);

            byte[] arrayBytes = ListToBytes(list);

            writer.Write(arrayBytes);
        }

        protected byte[] ListToBytes<T>(List<T> array)
        {
            int arrayByteSize = array.Count * Marshal.SizeOf(typeof(T));
            byte[] arrayBytes = new byte[arrayByteSize];

            IntPtr pointerToArray = Marshal.AllocHGlobal(arrayByteSize);
            long currentElementAddress = pointerToArray.ToInt64();

            for (int j = 0; j < array.Count; j++)
            {
                Marshal.StructureToPtr(array[j], new IntPtr(currentElementAddress), true);
                currentElementAddress = currentElementAddress + Marshal.SizeOf(typeof(T));
            }

            Marshal.Copy(pointerToArray, arrayBytes, 0, arrayBytes.Length);
            Marshal.FreeHGlobal(pointerToArray);

            return arrayBytes;
        }

        protected int GetUVSetsCount(Mesh mesh)
        {
            int i = 0;
            List<Vector2> uvs = new List<Vector2>();

            try
            {
                for (; i < 8; i++)
                {
                    mesh.GetUVs(i, uvs);
                }
            }
            catch (Exception) { }

            return i;
        }

        protected void WriteColor(BinaryWriter writer, Color color)
        {
            writer.Write(color.r);
            writer.Write(color.g);
            writer.Write(color.b);
            writer.Write(color.a);
        }

        protected void WriteVector2(BinaryWriter writer, Vector2 vector2)
        {
            writer.Write(vector2.x);
            writer.Write(vector2.y);
        }

        protected void WriteVector3(BinaryWriter writer, Vector3 vector3)
        {
            WriteVector2(writer, vector3);
            writer.Write(vector3.z);
        }

        protected void WriteQuaternion(BinaryWriter writer, Quaternion quaternion)
        {
            writer.Write(quaternion.x);
            writer.Write(quaternion.y);
            writer.Write(quaternion.z);
            writer.Write(quaternion.w);
        }

        protected void WriteTransform(BinaryWriter writer, Transform transform)
        {
            WriteVector3(writer, transform.position);
            WriteQuaternion(writer, transform.rotation);
            WriteVector3(writer, transform.localScale);
        }

        protected void WriteMeshes(BinaryWriter writer, List<Mesh> meshes)
        {
            writer.Write(meshes.Count);

            for (int i = 0; i < meshes.Count; i++)
            {
                WriteStringAsUnicode(writer, meshes[i].name);

                WriteArray(writer, meshes[i].vertices);
                WriteArray(writer, meshes[i].colors);
                WriteArray(writer, meshes[i].normals);

                int uvSetCount = GetUVSetsCount(meshes[i]);
                writer.Write(uvSetCount);

                for (int j = 0; j < uvSetCount; j++)
                {
                    List<Vector2> uvSet = new List<Vector2>();
                    meshes[i].GetUVs(j, uvSet);

                    WriteList(writer, uvSet);
                }

                writer.Write(meshes[i].subMeshCount);

                for (int j = 0; j < meshes[i].subMeshCount; j++)
                {
                    List<int> triangles = new List<int>();
                    meshes[i].GetTriangles(triangles, j);

                    WriteList(writer, triangles);
                }
            }
        }

        protected void WriteTextures(BinaryWriter writer, List<Texture2D> textures)
        {
            writer.Write(textures.Count);

            foreach (Texture2D texture in textures)
            {
                WriteStringAsUnicode(writer, texture.name);

                writer.Write(texture.width);
                writer.Write(texture.height);

                WriteStringAsUnicode(writer, texture.format.ToString());

                writer.Write(texture.mipmapCount);

                WriteArray(writer, texture.GetRawTextureData());
            }
        }

        protected void WriteShaders(BinaryWriter writer, List<Shader> shaders)
        {
            string[] shaderNames = shaders.Select((shader) => { return shader.name; }).ToArray();
            WriteStringArrayAsUnicode(writer, shaderNames);
        }

        protected void WriteMaterials(BinaryWriter writer, List<Material> materials, List<Shader> shaders, List<Texture2D> textures)
        {
            writer.Write(materials.Count);

            foreach (Material material in materials)
            {
                WriteStringAsUnicode(writer, material.name);
                writer.Write(shaders.IndexOf(material.shader));
                WriteColor(writer, material.color);

                if (material.mainTexture)
                {
                    writer.Write(true);

                    writer.Write(textures.IndexOf((Texture2D)material.mainTexture));
                    WriteVector2(writer, material.mainTextureScale);
                    WriteVector2(writer, material.mainTextureOffset);
                }
                else
                {
                    writer.Write(false);
                }
            }
        }

        protected void WriteHierarchy(BinaryWriter writer, GameObject parent, List<Mesh> meshes, List<Material> materials)
        {
            WriteStringAsUnicode(writer, parent.name);

            // Write activeSelf as always true
            writer.Write(true);

            WriteTransform(writer, parent.transform);

            MeshFilter meshFilter = parent.GetComponent<MeshFilter>();
            MeshRenderer meshRenderer = parent.GetComponent<MeshRenderer>();

            if (meshFilter)
            {
                writer.Write(true);

                writer.Write(meshes.IndexOf(meshFilter.sharedMesh));
            }
            else
            {
                writer.Write(false);
            }

            if (meshRenderer)
            {
                writer.Write(true);

                int[] materialIndexes = meshRenderer.sharedMaterials.Select((x) => { return materials.IndexOf(x); }).ToArray();
                WriteArray(writer, materialIndexes);
            }
            else
            {
                writer.Write(false);
            }

            writer.Write(parent.transform.childCount);
            foreach (Transform child in parent.transform)
            {
                WriteHierarchy(writer, child.gameObject, meshes, materials);
            }
        }

        protected void WriteModel(BinaryWriter writer, Model model)
        {
            GameObject modelGameObject = model.gameObject;
            WriteModel(writer, modelGameObject);
        }

        protected void WriteModel(BinaryWriter writer, GameObject modelGameObject)
        {
            List<Material> materials = new List<Material>();
            List<Texture2D> textures = new List<Texture2D>();
            List<Shader> shaders = new List<Shader>();
            List<Mesh> meshes = new List<Mesh>();

            ExcludeData(modelGameObject, materials, textures, shaders, meshes);

            WriteMeshes(writer, meshes);
            WriteTextures(writer, textures);
            WriteShaders(writer, shaders);
            WriteMaterials(writer, materials, shaders, textures);
            WriteHierarchy(writer, modelGameObject, meshes, materials);
        }

        protected void WriteSettings(BinaryWriter writer, SerializableProjectSettings serializableProjectSettings)
        {
            writer.Write(serializableProjectSettings.RangeLength);
            writer.Write(serializableProjectSettings.TimeRange.Start);
            writer.Write(serializableProjectSettings.TimeRange.End);
            writer.Write(serializableProjectSettings.TimeStepsCount);
        }

        protected void WriteWiring(BinaryWriter writer, Wiring wiring)
        {
            writer.Write(wiring.Count);

            foreach (Wire wire in wiring)
            {
                WriteStringAsUnicode(writer, wire.Name);
                writer.Write(wire.Amplitude);
                writer.Write(wire.Frequency);
                writer.Write(wire.Amperage);

                writer.Write(wire.Count);
                foreach (Vector3 point in wire)
                {
                    WriteVector3(writer, point);
                }
            }
        }

        protected void WriteMagneticTensionInSpace(BinaryWriter writer, MagneticTension.PointsInfo pointsInfo)
        {
            writer.Write(pointsInfo.PointsSize);

            foreach (MagneticTension.PointInfo pointInfo in pointsInfo.Infos)
            {
                WriteVector3(writer, pointInfo.Position);

                writer.Write(pointInfo.PrecomputedValue);

                for (int i = 0; i < pointInfo.CalculatedValuesInTime.Length; ++i)
                {
                    writer.Write(pointInfo.CalculatedValuesInTime[i].CalculatedValue);
                }
            }
        }

        protected void WriteElectricField(BinaryWriter writer, ElectricField.PointsInfo pointsInfo)
        {
            writer.Write(pointsInfo.PointsSize);

            foreach (ElectricField.PointInfo pointInfo in pointsInfo.Infos)
            {
                WriteVector3(writer, pointInfo.Position);

                writer.Write(pointInfo.PrecomputedValue);

                for (int i = 0; i < pointInfo.CalculatedValuesInTime.Length; ++i)
                {
                    writer.Write(pointInfo.CalculatedValuesInTime[i].CalculatedValue);
                }
            }
        }

        protected string ReadStringAsUnicode(BinaryReader reader)
        {
            int stringBytesLength = reader.ReadInt32();
            byte[] stringBytes = reader.ReadBytes(stringBytesLength);

            return Encoding.Unicode.GetString(stringBytes);
        }

        protected string[] ReadStringArrayAsUnicode(BinaryReader reader)
        {
            string[] array;
            array = new string[reader.ReadInt32()];

            for (int i = 0; i < array.Length; i++)
            {
                array[i] = ReadStringAsUnicode(reader);
            }

            return array;
        }

        protected void ReadPreambleAndCheck(BinaryReader reader)
        {
            string preamble = Encoding.ASCII.GetString(reader.ReadBytes(_preamble.Length));
            if (preamble != _preamble)
            {
                throw new InvalidDataException("File is not present EMSP structure");
            }
        }

        protected Version ReadVersion(BinaryReader reader)
        {
            return new Version(reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32(), reader.ReadInt32());
        }

        protected Vector3[] ReadVector3Array(BinaryReader reader)
        {
            int elementsCount = reader.ReadInt32();

            Vector3[] array = new Vector3[elementsCount];
            for (int i = 0; i < elementsCount; i++)
            {
                array[i] = new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }

            return array;
        }

        protected List<Vector2> ReadVector2List(BinaryReader reader)
        {
            int elementsCount = reader.ReadInt32();

            List<Vector2> list = new List<Vector2>(elementsCount);
            for (int i = 0; i < elementsCount; i++)
            {
                list.Add(new Vector2(reader.ReadSingle(), reader.ReadSingle()));
            }

            return list;
        }

        protected int[] ReadIntArray(BinaryReader reader)
        {
            int elementsCount = reader.ReadInt32();

            int[] array = new int[elementsCount];
            for (int i = 0; i < elementsCount; i++)
            {
                array[i] = reader.ReadInt32();
            }

            return array;
        }

        protected Color[] ReadColorArray(BinaryReader reader)
        {
            int elementsCount = reader.ReadInt32();

            Color[] array = new Color[elementsCount];
            for (int i = 0; i < elementsCount; i++)
            {
                array[i] = new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
            }

            return array;
        }

        protected Color ReadColor(BinaryReader reader)
        {
            return new Color(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        protected Vector2 ReadVector2(BinaryReader reader)
        {
            return new Vector2(reader.ReadSingle(), reader.ReadSingle());
        }

        protected Vector3 ReadVector3(BinaryReader reader)
        {
            return new Vector3(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        protected Quaternion ReadQuaternion(BinaryReader reader)
        {
            return new Quaternion(reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle(), reader.ReadSingle());
        }

        protected List<Mesh> ReadMeshes(BinaryReader reader)
        {
            List<Mesh> meshes = new List<Mesh>();

            int meshesCount = reader.ReadInt32();

            for (int i = 0; i < meshesCount; i++)
            {
                Mesh mesh = new Mesh();

                mesh.name = ReadStringAsUnicode(reader);

                mesh.vertices = ReadVector3Array(reader);
                mesh.colors = ReadColorArray(reader);
                mesh.normals = ReadVector3Array(reader);

                int uvSetsCount = reader.ReadInt32();

                for (int j = 0; j < uvSetsCount; j++)
                {
                    mesh.SetUVs(j, ReadVector2List(reader));
                }

                int subMeshCount = reader.ReadInt32();
                mesh.subMeshCount = subMeshCount;

                for (int j = 0; j < subMeshCount; j++)
                {
                    mesh.SetTriangles(ReadIntArray(reader), j);
                }

                meshes.Add(mesh);
            }

            return meshes;
        }

        protected List<Texture2D> ReadTextures(BinaryReader reader)
        {
            List<Texture2D> textures = new List<Texture2D>();

            int texturesCount = reader.ReadInt32();

            for (int i = 0; i < texturesCount; i++)
            {
                string name = ReadStringAsUnicode(reader);
                int width = reader.ReadInt32();
                int height = reader.ReadInt32();
                TextureFormat format = (TextureFormat)Enum.Parse(typeof(TextureFormat), ReadStringAsUnicode(reader));
                int mipmapCount = reader.ReadInt32();

                int bytesLength = reader.ReadInt32();
                byte[] bytes = reader.ReadBytes(bytesLength);

                Texture2D texture = new Texture2D(width, height, format, mipmapCount > 1 ? true : false)
                {
                    name = name
                };

                texture.LoadRawTextureData(bytes);
                texture.Apply();

                textures.Add(texture);
            }

            return textures;
        }

        protected List<Shader> ReadShaders(BinaryReader reader)
        {
            return new List<Shader>(ReadStringArrayAsUnicode(reader).Select((shaderName) => { return Shader.Find(shaderName); }));
        }

        protected List<Material> ReadMaterials(BinaryReader reader, List<Shader> shaders, List<Texture2D> textures)
        {
            List<Material> materials = new List<Material>();

            int materialsCount = reader.ReadInt32();

            for (int i = 0; i < materialsCount; i++)
            {
                string name = ReadStringAsUnicode(reader);
                Material material = new Material(shaders[reader.ReadInt32()])
                {
                    name = name,
                    color = ReadColor(reader)
                };

                // Main texture handle
                if (reader.ReadBoolean())
                {
                    material.mainTexture = textures[reader.ReadInt32()];
                    material.mainTextureScale = ReadVector2(reader);
                    material.mainTextureOffset = ReadVector2(reader);
                }

                materials.Add(material);
            }

            return materials;
        }

        protected GameObject ReadHierarchy(BinaryReader reader, List<Mesh> meshes, List<Material> materials)
        {
            GameObject gameObject = new GameObject(ReadStringAsUnicode(reader));
            gameObject.SetActive(reader.ReadBoolean());

            gameObject.transform.position = ReadVector3(reader);
            gameObject.transform.rotation = ReadQuaternion(reader);
            gameObject.transform.localScale = ReadVector3(reader);

            if (reader.ReadBoolean())
            {
                MeshFilter meshFilter = gameObject.AddComponent<MeshFilter>();

                int meshIndex = reader.ReadInt32();
                if (meshIndex != -1)
                {
                    meshFilter.sharedMesh = meshes[meshIndex];
                }
            }

            if (reader.ReadBoolean())
            {
                MeshRenderer meshRenderer = gameObject.AddComponent<MeshRenderer>();

                int[] materialsIndexes = ReadIntArray(reader);
                meshRenderer.sharedMaterials = (materialsIndexes.Select((x) => { return materials[x]; })).ToArray();
            }

            int childCount = reader.ReadInt32();
            for (int i = 0; i < childCount; i++)
            {
                GameObject hierarchyObject = ReadHierarchy(reader, meshes, materials);

                Vector3 originScale = hierarchyObject.transform.localScale;
                hierarchyObject.transform.SetParent(gameObject.transform, true);
                hierarchyObject.transform.localScale = originScale;
            }

            return gameObject;
        }

        protected Range ReadRange(BinaryReader reader)
        {
            return new Range(reader.ReadSingle(), reader.ReadSingle());
        }

        protected SerializableProjectSettings ReadProjectSettings(BinaryReader reader)
        {
            return new SerializableProjectSettings(reader.ReadInt32(), ReadRange(reader), reader.ReadInt32());
        }
        #endregion
        #endregion
        #endregion
    }
}