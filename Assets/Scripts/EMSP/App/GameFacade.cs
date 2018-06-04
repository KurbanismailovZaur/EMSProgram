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
using EMSP.Mathematic.Magnetic;
using EMSP.Timing;
using EMSP.UI.Dialogs.CalculationSettings;
using System;

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
        private CalculationSettingsDialog _calculationSettingsDialog;

        [SerializeField]
        private OrbitController _orbitController;

        [SerializeField]
        private WiringEditorDialog _wiringEditorDialog;

        private Exporter _exporter = new Exporter();

        [SerializeField]
        private TimeLine _timeLine;

        [SerializeField]
        private RangeSlider _tensionFilterSlider;

        [SerializeField]
        private GeneralPanel _generalPanel;
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
        private bool IsProjectChanged()
        {
            return ProjectManager.Instance.Project != null && ProjectManager.Instance.Project.IsChanged;
        }

        private void CreateNewProjectWithCheckToSave()
        {
            CloseProjectWithCheckToSaveAndDo(CreateNewProject, (action) => { OnSaveProjectDialog(action, CreateNewProject); });
        }

        private void OpenProjectWithCheckToSave()
        {
            string path = OpenProjectFilePanel();

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            CloseProjectWithCheckToSaveAndDo(() => { OpenProject(path); }, (action) => { OnSaveProjectDialog(action, () => { OpenProject(path); }); });
        }

        private void OnSaveProjectDialog(SaveProjectDialog.Action action, Action callback)
        {
            switch (action)
            {
                case SaveProjectDialog.Action.Save:
                    if (!SaveProject()) return;
                    goto case SaveProjectDialog.Action.DontSave;
                case SaveProjectDialog.Action.DontSave:
                    CloseProject();
                    if (callback != null) callback.Invoke();
                    break;
                case SaveProjectDialog.Action.Cancel:
                    break;
            }
        }

        private string OpenProjectFilePanel()
        {
            string[] results = StandaloneFileBrowser.OpenFilePanel("Открыть Проект", Application.dataPath, GameSettings.Instance.ProjectExtensionFilter, false);

            if (results.Length == 0)
            {
                return string.Empty;
            }

            return results[0];
        }

        private void OpenProject(string path)
        {
            ProjectManager.Instance.OpenProject(path);

            MathematicManager.Instance.Show(CalculationType.MagneticTensionInSpace);

            UpdateTimeAndTensionSlider();
        }

        private bool SaveProject()
        {
            if (!ProjectManager.Instance.Project.IsStored)
            {
                string path = StandaloneFileBrowser.SaveFilePanel("Сохранить проект", Application.dataPath, GameSettings.Instance.ProjectDefaultName, GameSettings.Instance.ProjectExtensionFilter);

                if (string.IsNullOrEmpty(path)) return false;

                ProjectManager.Instance.SaveProject(path);
            }
            else
            {
                ProjectManager.Instance.ResaveProject();
            }

            return true;
        }

        private void CloseProjectWithCheckToSaveAndDo(Action notChangedAction, Action<SaveProjectDialog.Action> changedAction)
        {
            if (!IsProjectChanged())
            {
                CloseProject();

                if (notChangedAction != null) notChangedAction.Invoke();
            }
            else
            {
                _saveProjectDialog.ShowModal(changedAction);
            }
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
            string[] results = StandaloneFileBrowser.OpenFilePanel("Открыть Модель", Application.dataPath, GameSettings.Instance.ModelExtensionFilter, false);

            if (results.Length == 0)
            {
                return;
            }

            ModelManager.Instance.CreateNewModel(results[0]);
        }

        private void ImportWiring()
        {
            string[] results = StandaloneFileBrowser.OpenFilePanel("Открыть Проводку", Application.dataPath, GameSettings.Instance.WiringExtensionFilter, false);

            if (results.Length == 0)
            {
                return;
            }

            WiringManager.Instance.CreateNewWiring(results[0]);
        }

        private void ExportMagneticTensionInSpace()
        {
            string path = StandaloneFileBrowser.SaveFilePanel("Экспорт Магнитного Напряжения в Пространстве", Application.dataPath, GameSettings.Instance.MagneticTensionInSpaceDefaultName, GameSettings.Instance.WiringExtensionFilter);

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            _exporter.ExportMagneticTensionInSpace(path);
        }

        private void ExportWiring()
        {
            string path = StandaloneFileBrowser.SaveFilePanel("Сохранить проводку", Application.dataPath, "Проводка", "xls");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            WiringManager.Instance.SaveWiring(path);
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
        private void Calculate(CalculationType calculationType)
        {
            MathematicManager.Instance.Calculate(calculationType);
            MathematicManager.Instance.Show(calculationType);

            UpdateTimeAndTensionSlider();
        }

        private void UpdateTimeAndTensionSlider()
        {
            TimeManager.Instance.TimeIndex = 0;

            UpdateTensionSlider();
        }

        private void UpdateTensionSlider(bool affectOnCurrent = true)
        {
            _tensionFilterSlider.SetRangeLimits(0f, MathematicManager.Instance.MagneticTensionInSpace.CurrentModeMaxMagneticTension);

            if (affectOnCurrent)
            {
                _tensionFilterSlider.SetMin(0f);
                _tensionFilterSlider.SetMax(MathematicManager.Instance.MagneticTensionInSpace.CurrentModeMaxMagneticTension);
            }
        }

        private void OpenParameters()
        {
            _calculationSettingsDialog.ShowModal();
        }
        #endregion

        private void CreateNewProject()
        {
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
                    OpenProjectWithCheckToSave();
                    break;
                case FileContextMethods.ActionType.SaveProject:
                    SaveProject();
                    break;
                case FileContextMethods.ActionType.CloseProject:
                    CloseProjectWithCheckToSaveAndDo(null, (action) => { OnSaveProjectDialog(action, null); });
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
                case FileContextMethods.ActionType.ExportWiring:
                    ExportWiring();
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
                    Calculate(CalculationType.MagneticTensionInSpace);
                    break;
                case CalculationsContextMethods.ActionType.ElectricField:
                    Calculate(CalculationType.ElectricField);
                    break;
                case CalculationsContextMethods.ActionType.Parameters:
                    OpenParameters();
                    break;
            }

            calculationsContextMethods.Panel.HideActiveContextAndStopAutoShow();
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

        public void MagneticTensionInSpace_VisibilityChanged(MagneticTensionInSpace magneticTensionInSpace, bool state)
        {
            if (state)
            {
                if (MathematicManager.Instance.AmperageMode == AmperageMode.Computational && MathematicManager.Instance.MagneticTensionInSpace.IsCalculated)
                {
                    _timeLine.Show();
                }

                _tensionFilterSlider.Show();
            }
            else
            {
                _timeLine.Stop();
                _timeLine.Hide();

                _tensionFilterSlider.Hide();
            }
        }

        public void TimeManager_TimeIndexChanged(TimeManager timeManager, int index)
        {
            MathematicManager.Instance.MagneticTensionInSpace.SetPointsToTime(index);
        }

        public void CalculationSettingsDialog_Applyed(CalculationSettingsDialog calculationSettingsDialog, CalculationSettingsDialog.Settings settings)
        {
            MathematicManager.Instance.RangeLength = settings.RangeLength;
            TimeManager.Instance.SetTimeParameters(settings.TimeRange, settings.TimeStepsCount);

            MathematicManager.Instance.DestroyCalculations();
        }

        public void FilterRangeSlider_OnValueChanged(RangeSlider rangeSlider, Range range)
        {
            MathematicManager.Instance.MagneticTensionInSpace.FilterPointsByTension(range);
        }

        public void MagneticTensionInSpace_CurrentTensionFilterRangeChanged(MagneticTensionInSpace magneticTensionInSpace, Range range)
        {
            _tensionFilterSlider.SetMin(range.Min);
            _tensionFilterSlider.SetMax(range.Max);
        }

        public void MathematicManager_AmperageModeChanged(MathematicManager mathematicManager, AmperageMode amperageMode)
        {
            if (amperageMode == AmperageMode.Computational && MathematicManager.Instance.MagneticTensionInSpace.IsCalculated)
            {
                _timeLine.Show();
            }
            else
            {
                _timeLine.Stop();
                _timeLine.Hide();
            }
        }
        #endregion
        #endregion
    }
}