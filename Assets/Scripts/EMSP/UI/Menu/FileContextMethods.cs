using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;

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
        private OpenFileDialog _openFileDialog = new OpenFileDialog();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Start()
        {
            _openFileDialog.Filter = "3D Model (*.obj)|*.obj";
            _openFileDialog.FilterIndex = 1;
        }

        public void NewProject()
        {
            print("new Project");
        }

        public void OpenProject()
        {
            print("Open Project");
        }

        public void SaveProject()
        {
            print("Save Project");
        }

        public void ImportModel()
        {
            if (_openFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            ModelManager.Instance.CreateNewModel(_openFileDialog.FileName);
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