using EMSP.Communication;
using EMSP.Mathematic;
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

namespace EMSP.Data.Serialization
{
	public abstract class BinarySerializerVersion 
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
        protected abstract string Preamble { get; }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        #region Binary operation methods
        protected void WritePreamble(BinaryWriter writer)
        {
            writer.Write(Encoding.ASCII.GetBytes(Preamble));
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
            string preamble = Encoding.ASCII.GetString(reader.ReadBytes(Preamble.Length));
            if (preamble != Preamble)
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

        protected Range ReadRange(BinaryReader reader)
        {
            return new Range(reader.ReadSingle(), reader.ReadSingle());
        }
        #endregion
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}
