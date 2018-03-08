using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Numba.UI.Menu;
using SFB;
using EMSP.Communication;
using EMSP.App;

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

        private Panel _panel;
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

            _panel = ((Group)_context.ContextContainer).Panel;
        }

        public void NewProject()
        {
            ProjectManager.Instance.CreateNewProject();

            _panel.HideActiveContextAndStopAutoShow();
        }

        public void OpenProject()
        {
            print("Open Project");
        }

        public void SaveProject()
        {
            print("Save Project");
        }

        public void CloseProject()
        {
            ProjectManager.Instance.CloseProject();

            _panel.HideActiveContextAndStopAutoShow();
        }

        public void ImportModel()
        {
            string[] results = StandaloneFileBrowser.OpenFilePanel("Open Model", Application.dataPath, GameSettings.Instance.ModelExtensionFilter, false);

            if (results.Length == 0)
            {
                return;
            }

            ModelManager.Instance.CreateNewModel(results[0]);
            _panel.HideActiveContextAndStopAutoShow();
        }

        public void ImportWiring()
        {
            string[] results = StandaloneFileBrowser.OpenFilePanel("Open Wiring", Application.dataPath, GameSettings.Instance.WiringExtensionFilter, false);

            if (results.Length == 0)
            {
                return;
            }

            WiringManager.Instance.CreateNewWiring(results[0]);
            _panel.HideActiveContextAndStopAutoShow();
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