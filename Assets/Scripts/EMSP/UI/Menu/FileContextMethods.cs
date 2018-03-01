using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.UI.Menu
{
    public class FileContextMethods : MonoBehaviour
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
        public void NewProject()
        {
            
        }

        public void OpenProject()
        {
            print("Open project");
        }

        public void SaveProject()
        {
            print("Save Project");
        }

        public void ImportModel()
        {
            print("Import Model");
        }

        public void ImportWiring()
        {
            print("Import Wiring");
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            Application.Quit();
#endif
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}