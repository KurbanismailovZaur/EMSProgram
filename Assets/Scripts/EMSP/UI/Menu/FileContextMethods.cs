using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Windows.Forms;
using Numba.UI.Menu;
using EMSP.Communication;

namespace EMSP.UI.Menu
{
    [RequireComponent(typeof(Context))]
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
        private Context _context;

        private OpenFileDialog _openModelFileDialog = new OpenFileDialog();

        private OpenFileDialog _openWiringFileDialog = new OpenFileDialog();
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            _context = GetComponent<Context>();

            _openModelFileDialog.Filter = "3D Model (*.obj)|*.obj";
            _openModelFileDialog.FilterIndex = 1;

            _openWiringFileDialog.Filter = "Excel Worksheets 2003 (*.xls)|*.xls";
            _openWiringFileDialog.FilterIndex = 1;
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
            if (_openModelFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            ModelManager.Instance.CreateNewModel(_openModelFileDialog.FileName);
            ((Group)_context.ContextContainer).Panel.HideActiveContextAndStopAutoShow();
        }

        public void ImportWiring()
        {
            if (_openWiringFileDialog.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            WiringManager.Instance.CreateNewWiring(_openWiringFileDialog.FileName);
            ((Group)_context.ContextContainer).Panel.HideActiveContextAndStopAutoShow();
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