using EMSP.Communication;
using EMSP.Control;
using EMSP.UI;
using EMSP.UI.Dialogs.SaveProject;
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

        [SerializeField]
        private SaveProjectDialog _saveProjectDialog;

        [SerializeField]
        private OrbitController _orbitController;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        #region File context methods
        private void CreateNewProjectWithCheckToSave()
        {
            if (ProjectManager.Instance.Project != null && !ProjectManager.Instance.Project.IsChanged)
            {
                BlockCamera();
                _saveProjectDialog.Chosen.AddListener(SaveProjectDialog_Chosen);
                _saveProjectDialog.ShowModal();
                return;
            }

            CreateNewProject();
        }

        private void OpenProject()
        {
            print("Open Project");
        }

        private void SaveProject()
        {
            print("Save Project");
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
        #endregion

        private void CreateNewProject()
        {
            ProjectManager.Instance.CloseProject();
            ProjectManager.Instance.CreateNewProject();
        }

        private void ResetOrbitController()
        {
            _orbitController.TargetVector = GameSettings.Instance.OrbitControllerDefaultTargetVector;
            _orbitController.TargetUpVector = GameSettings.Instance.OrbitControllerDefaultTargetUpVector;
            _orbitController.Distance = GameSettings.Instance.OrbitControllerDefaultDistance;
        }

        public void ActivateProjectEnvironment()
        {
            ResetOrbitController();
            _viewBlocker.UnblockView();
        }

        public void DeactivateProjectEnvironment()
        {
            _viewBlocker.BlockView();
        }

        private void BlockCamera()
        {
            print("Block Camera");
        }

        private void UnblockCamera()
        {
            print("Unblock Camera");
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
                    CreateNewProjectWithCheckToSave();
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

        private void SaveProjectDialog_Chosen(SaveProjectDialog saveProjectDialog, SaveProjectDialog.Action action)
        {
            UnblockCamera();
            _saveProjectDialog.Chosen.RemoveListener(SaveProjectDialog_Chosen);

            switch (action)
            {
                case SaveProjectDialog.Action.Save:
                    SaveProject();
                    CreateNewProject();
                    break;
                case SaveProjectDialog.Action.DontSave:
                    CreateNewProject();
                    break;
                case SaveProjectDialog.Action.Cancel:
                    break;
            }
        }
        #endregion
        #endregion
    }
}