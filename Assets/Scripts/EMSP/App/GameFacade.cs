using EMSP.Communication;
using EMSP.UI;
using EMSP.UI.Menu;
using Numba;
using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.App
{
    public class GameFacade : MonoSingleton<GameFacade>
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
        [SerializeField]
        private ViewBlocker _viewBlocker;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void CreateNewProject()
        {
            if (ProjectManager.Instance.Project != null && ProjectManager.Instance.Project.IsChanged)
            {

                return;
            }

            ProjectManager.Instance.CreateNewProject();
        }

        private void CloseProject()
        {
            ProjectManager.Instance.CloseProject();
        }

        private void ImportModel()
        {
            string[] results = StandaloneFileBrowser.OpenFilePanel("Open Model", Application.dataPath, GameSettings.Instance.ModelExtensionFilter, false);

            if (results.Length == 0)
            {
                return;
            }

            ModelManager.Instance.CreateNewModel(results[0]);
        }

        private void ImportWiring()
        {
            string[] results = StandaloneFileBrowser.OpenFilePanel("Open Wiring", Application.dataPath, GameSettings.Instance.WiringExtensionFilter, false);

            if (results.Length == 0)
            {
                return;
            }

            WiringManager.Instance.CreateNewWiring(results[0]);
        }

        private void ExitApplication()
        {
#if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_STANDALONE
                    UnityEngine.Application.Quit();
#endif
        }

        public void ActivateProjectEnvironment()
        {
            Camera.main.transform.position = GameSettings.Instance.CameraDefaultPosition;
            Camera.main.transform.rotation = GameSettings.Instance.CameraDefaultRotation;

            _viewBlocker.UnblockView();
        }

        public void DeactivateProjectEnvironment()
        {
            _viewBlocker.BlockView();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void FileContextMethods_Selected(FileContextMethods fileContextMethods, FileContextMethods.ActionType actionType)
        {
            switch (actionType)
            {
                case FileContextMethods.ActionType.NewProject:
                    CreateNewProject();
                    break;
                case FileContextMethods.ActionType.OpenProject:
                    print("Open Project");
                    break;
                case FileContextMethods.ActionType.SaveProject:
                    print("Save Project");
                    break;
                case FileContextMethods.ActionType.CloseProject:
                    CloseProject();
                    break;
                case FileContextMethods.ActionType.ImportModel:
                    ImportModel();
                    break;
                case FileContextMethods.ActionType.ImportWiring:
                    ImportWiring();
                    break;
                case FileContextMethods.ActionType.Exit:
                    ExitApplication();
                    break;
            }

            fileContextMethods.Panel.HideActiveContextAndStopAutoShow();
        }
        #endregion
        #endregion
    }
}