using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Numba.UI.Menu;
using SFB;
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

        private ExtensionFilter[] _modelExtensionFilter = new ExtensionFilter[] { new ExtensionFilter("3D Model", "obj") };

        private ExtensionFilter[] _wiringExtensionFilter = new ExtensionFilter[] { new ExtensionFilter("Excel Worksheets 2003", "xls") };
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
            string[] results = StandaloneFileBrowser.OpenFilePanel("Open Model", string.Empty, _modelExtensionFilter, false);

            if (results.Length == 0)
            {
                return;
            }

            ModelManager.Instance.CreateNewModel(results[0]);
            ((Group)_context.ContextContainer).Panel.HideActiveContextAndStopAutoShow();
        }

        public void ImportWiring()
        {
            string[] results = StandaloneFileBrowser.OpenFilePanel("Open Wiring", string.Empty, _wiringExtensionFilter, false);

            if (results.Length == 0)
            {
                return;
            }

            WiringManager.Instance.CreateNewWiring(results[0]);
            ((Group)_context.ContextContainer).Panel.HideActiveContextAndStopAutoShow();
        }

        public void Exit()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
            UnityEngine.Application.Quit();
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