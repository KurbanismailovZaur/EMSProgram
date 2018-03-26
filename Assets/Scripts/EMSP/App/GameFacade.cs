using EMSP.Communication;
using EMSP.Control;
using EMSP.Environment.View;
using EMSP.Mathematic;
using EMSP.UI;
using EMSP.UI.Dialogs.SaveProject;
using EMSP.UI.Menu.Contexts;
using Numba;
using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = EMSP.Environment.Metrics.Grid;
using EMSP.UI.Dialogs.WiringEditor;
using EMSP.Data;

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

        [SerializeField]
        private WiringEditorDialog _wiringEditorDialog;

        private Exporter _exporter = new Exporter();
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
            ModelManager.Instance.DestroyModel();
            WiringManager.Instance.DestroyWiring();
            MathematicManager.Instance.DestroyCalculations();
            ProjectManager.Instance.CloseProject();

            Resources.UnloadUnusedAssets();
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

        private void ExportMagneticTensionInSpace()
        {
            string path = StandaloneFileBrowser.SaveFilePanel("Save Magnetic Tension in Space", Application.dataPath, GameSettings.Instance.MagneticTensionInSpaceDefaultName, GameSettings.Instance.WiringExtensionFilter);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            _exporter.ExportMagneticTensionInSpace(path);
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

        #region Edit context methods
        private void RemoveModel()
        {
            ModelManager.Instance.DestroyModel();
        }

        private void RemoveWiring()
        {
            WiringManager.Instance.DestroyWiring();
        }

        private void RemoveMagneticTensionInSpace()
        {
            MathematicManager.Instance.MagneticTensionInSpace.DestroyMagneticTensions();
        }

        private void EditWiring()
        {
            _wiringEditorDialog.StartWiringEditing(WiringManager.Instance.Wiring, WiringEditorDialog_OnWiringEdited);
        }
        #endregion

        #region Calculations context methods
        private void CalculateMagneticTensionsInSpace()
        {
            MathematicManager.Instance.Calculate(CalculationType.MagneticTensionInSpace);
            MathematicManager.Instance.Show(CalculationType.MagneticTensionInSpace);
        }
        #endregion

        private void CreateNewProject()
        {
            ProjectManager.Instance.CloseProject();
            ProjectManager.Instance.CreateNewProject();
        }

        private void ResetOrbitController()
        {
            _orbitController.SetTargetVectorAndZAngle(GameSettings.Instance.OrbitControllerDefaultTargetVector, GameSettings.Instance.OrbitControllerDefaultTargetUpAngle);
            _orbitController.Distance = GameSettings.Instance.OrbitControllerDefaultDistance;
        }

        public void InitializeActivateProjectEnvironment()
        {
            Grid.Instance.Visibility = true;
            ResetOrbitController();
            _viewBlocker.UnblockView();
        }

        public void DeactivateProjectEnvironment()
        {
            Grid.Instance.Visibility = false;
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
                    CreateNewProjectWithCheckToSave();
                    break;
                case FileContextMethods.ActionType.OpenProject:
                    OpenProject();
                    break;
                case FileContextMethods.ActionType.SaveProject:
                    SaveProject();
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
                case FileContextMethods.ActionType.ExportMagneticTensionInSpace:
                    ExportMagneticTensionInSpace();
                    break;
                case FileContextMethods.ActionType.Exit:
                    ExitApplication();
                    break;
            }

            fileContextMethods.Panel.HideActiveContextAndStopAutoShow();
        }

        public void EditContextMethods_Selected(EditContextMethods editContextMethods, EditContextMethods.ActionType actionType)
        {
            switch (actionType)
            {
                case EditContextMethods.ActionType.RemoveModel:
                    RemoveModel();
                    break;
                case EditContextMethods.ActionType.RemoveWiring:
                    RemoveWiring();
                    break;
                case EditContextMethods.ActionType.RemoveMagneticTensionInSpace:
                    RemoveMagneticTensionInSpace();
                    break;
                case EditContextMethods.ActionType.EditWiring:
                    EditWiring();
                    break;
            }

            editContextMethods.Panel.HideActiveContextAndStopAutoShow();
        }

        public void ViewContextMethods_Selected(ViewContextMethods viewContextMethods, ViewContextMethods.ActionType actionType)
        {
            switch (actionType) { }

            viewContextMethods.Panel.HideActiveContextAndStopAutoShow();
        }

        public void CalculationsContextMethods_Selected(CalculationsContextMethods calculationsContextMethods, CalculationsContextMethods.ActionType actionType)
        {
            switch (actionType)
            {
                case CalculationsContextMethods.ActionType.MagneticTensionInSpace:
                    CalculateMagneticTensionsInSpace();
                    break;
            }

            calculationsContextMethods.Panel.HideActiveContextAndStopAutoShow();
        }

        private void SaveProjectDialog_Chosen(SaveProjectDialog saveProjectDialog, SaveProjectDialog.Action action)
        {
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

        public void ViewCube_AxisSelected(ViewCube viewCube, AxisDirection axisDirection)
        {
            switch (axisDirection)
            {
                case AxisDirection.Right:
                    _orbitController.SetTargetVectorAndZAngle(Vector3.right, 0f);
                    break;
                case AxisDirection.Up:
                    _orbitController.SetTargetVectorAndZAngle(Vector3.up, 0f);
                    break;
                case AxisDirection.Forward:
                    _orbitController.SetTargetVectorAndZAngle(Vector3.forward, 0f);
                    break;
                case AxisDirection.Left:
                    _orbitController.SetTargetVectorAndZAngle(Vector3.left, 0f);
                    break;
                case AxisDirection.Down:
                    _orbitController.SetTargetVectorAndZAngle(Vector3.down, 0f);
                    break;
                case AxisDirection.Back:
                    _orbitController.SetTargetVectorAndZAngle(Vector3.back, 0f);
                    break;
            }
        }

        private void WiringEditorDialog_OnWiringEdited(Wiring wiring)
        {
            WiringManager.Instance.CreateNewWiring(wiring);
        }
        #endregion
        #endregion
    }
}