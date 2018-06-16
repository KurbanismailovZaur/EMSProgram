using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace EMSP.Logging
{
    public static class Log
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
        private static bool _isInitialized;

        private const string _filename = "log.txt";

        private static StreamWriter _writer;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
#if UNITY_EDITOR
        private static string PathToFile { get { return Path.Combine(Path.GetDirectoryName(Application.dataPath), _filename); } }
#elif UNITY_STANDALONE
        private static string PathToFile { get { return Path.Combine(Application.dataPath, _filename); } }
#endif

        #endregion

        #region Constructors
        #endregion

        #region Methods
        public static void Initialize()
        {
            if (_isInitialized) return;

            _isInitialized = true;

            string pathToDirectory = Path.GetDirectoryName(PathToFile);

            if (!Directory.Exists(pathToDirectory)) Directory.CreateDirectory(pathToDirectory);

            _writer = new StreamWriter(new FileStream(PathToFile, FileMode.OpenOrCreate, FileAccess.Write));
        }

        private static string GetCurrentDateTimeString()
        {
            return DateTime.Now.ToString("dd.MM.yyyy HH:mm:ss:ms");
        }

        public static void WriteOperation(string message)
        {
            WriteLineWithSquareBrackets(GetCurrentDateTimeString());
            WriteLineWithSquareBrackets("Operation");

            WriteLine(ReplaceLineBreaksToSpaces(message));

            WriteLine();
        }

        public static void WriteException(Exception exception)
        {
            WriteLineWithSquareBrackets(GetCurrentDateTimeString());
            WriteLineWithSquareBrackets("Exception");

            WriteLineField("Message", ReplaceLineBreaksToSpaces(exception.Message));
            WriteLineField("StackTrace", ReplaceLineBreaksToSpaces(exception.StackTrace));

            WriteLine();
        }

        private static string ReplaceLineBreaksToSpaces(string source)
        {
            if (string.IsNullOrEmpty(source)) return source;
            
            return source.Replace(System.Environment.NewLine, " ");
        }

        private static void WriteLineWithSquareBrackets(object obj)
        {
            _writer.WriteLine(WrapInSquareBrackets(obj));
        }

        private static string WrapInSquareBrackets(object obj)
        {
            return string.Format("[{0}]", obj.ToString());
        }

        private static void WriteLine()
        {
            _writer.WriteLine();
        }

        private static void WriteLine(object obj)
        {
            _writer.WriteLine(obj);
        }

        private static void WriteLineField(string fieldName, object fieldValue)
        {
            WriteLine(string.Format("{0}: {1}", fieldName, fieldValue));
        }

        public static void Dispose()
        {
            _writer.Close();
        }
#endregion

#region Indexers
#endregion

#region Events handlers
#endregion
#endregion
    }
}
