using EMSP.Communication;
using EMSP.Control;
using EMSP.Environment.View;
using EMSP.Mathematic;
using EMSP.UI;
using EMSP.UI.Windows.SaveProject;
using EMSP.UI.Menu.Contexts;
using Numba;
using SFB;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Grid = EMSP.Environment.Metrics.Grid;
using EMSP.UI.Windows.WiringEditor;
using EMSP.Data;
using EMSP.Mathematic.Magnetic;
using EMSP.Timing;
using EMSP.UI.Windows.CalculationSettings;
using System;
using EMSP.UI.Windows.Processing;
using EMSP.Data.Serialization.EMSV;
using EMSP.Data.Serialization.EMSV.Versions;
using System.IO;
using System.Threading;
using EMSP.Processing;
using EMSP.Logging;

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
        private SaveProjectWindow _saveProjectDialog;

        [SerializeField]
        private CalculationSettingsWindow _calculationSettingsDialog;

        [SerializeField]
        private ProcessWindow _processWindow;

        [SerializeField]
        private OrbitController _orbitController;

        [SerializeField]
        private WiringEditorWindow _wiringEditorDialog;

        private Exporter _exporter = new Exporter();

        [SerializeField]
        private TimeLine _timeLine;

        [SerializeField]
        private RangeSlider _tensionFilterSlider;

        [SerializeField]
        private GeneralPanel _generalPanel;

        [SerializeField]
        private ProcessManager _processManager;
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

        private void OnSaveProjectDialog(SaveProjectWindow.Action action, Action callback)
        {
            switch (action)
            {
                case SaveProjectWindow.Action.Save:
                    if (!SaveProject()) return;
                    goto case SaveProjectWindow.Action.DontSave;
                case SaveProjectWindow.Action.DontSave:
                    CloseProject();
                    if (callback != null) callback.Invoke();
                    break;
                case SaveProjectWindow.Action.Cancel:
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

            //MathematicManager.Instance.ShowFirstExistCalculation();

            //UpdateTimeAndTensionSlider();
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

        private void CloseProjectWithCheckToSaveAndDo(Action notChangedAction, Action<SaveProjectWindow.Action> changedAction)
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
            MathematicManager.Instance.DestroyAllCalculations();
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

        private void GenerateVerticesBasedOnOBJ()
        {
            string[] results = StandaloneFileBrowser.OpenFilePanel("Генерировать данные вершин", Application.dataPath, GameSettings.Instance.ModelExtensionFilter, true);

            if (results.Length == 0)
            {
                return;
            }

            foreach (var pathToOBJ in results)
            {
                _processManager.CreateGenerateVerticesBasedOnOBJProcess(pathToOBJ);
            }
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

        private void RemoveMagneticTensionPoints()
        {
            MathematicManager.Instance.DestroyCalculations(CalculationType.MagneticTension);
        }

        private void RemoveElectricFieldPoints()
        {
            MathematicManager.Instance.DestroyCalculations(CalculationType.ElectricField);
        }

        private void EditWiring()
        {
            _wiringEditorDialog.StartWiringEditing(WiringManager.Instance.Wiring, WiringEditorDialog_OnWiringEdited);
        }
        #endregion

        #region Calculations context methods
        private void Calculate(CalculationType calculationType)
        {
            MathematicManager.Instance.DestroyCalculations(calculationType);
            MathematicManager.Instance.Calculate(calculationType);
            MathematicManager.Instance.Show(calculationType);

            UpdateTimeAndTensionSlider();
        }

        private void UpdateTimeAndTensionSlider()
        {
            TimeManager.Instance.TimeIndex = 0;

            UpdateTensionSlider(true);
        }

        private void UpdateTensionSlider(bool affectOnCurrent = true)
        {
            _tensionFilterSlider.SetRangeLimits(0f, MathematicManager.Instance.CurrentCalculationMethod.CurrentModeMaxCalculatedValue);

            if (affectOnCurrent)
            {
                _tensionFilterSlider.SetMin(0f);
                _tensionFilterSlider.SetMax(MathematicManager.Instance.CurrentCalculationMethod.CurrentModeMaxCalculatedValue);
            }
        }

        private void OpenParameters()
        {
            _calculationSettingsDialog.ShowModal();
        }
        #endregion

        #region Window context methods
        private void OpenProcessWindow()
        {
            _processWindow.ShowModal();
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
                    Log.WriteOperation("Starting_CreateNewProjectWithCheckToSave");
                    CreateNewProjectWithCheckToSave();
                    break;
                case FileContextMethods.ActionType.OpenProject:
                    Log.WriteOperation("Starting_OpenProjectWithCheckToSave");
                    OpenProjectWithCheckToSave();
                    break;
                case FileContextMethods.ActionType.SaveProject:
                    Log.WriteOperation("Starting_SaveProject");
                    SaveProject();
                    break;
                case FileContextMethods.ActionType.CloseProject:
                    Log.WriteOperation("Starting_CloseProjectWithCheckToSaveAndDo");
                    CloseProjectWithCheckToSaveAndDo(null, (action) => { OnSaveProjectDialog(action, null); });
                    break;
                case FileContextMethods.ActionType.ImportModel:
                    Log.WriteOperation("Starting_ImportModel");
                    ImportModel();
                    break;
                case FileContextMethods.ActionType.ImportWiring:
                    Log.WriteOperation("Starting_ImportWiring");
                    ImportWiring();
                    break;
                case FileContextMethods.ActionType.ExportMagneticTensionInSpace:
                    Log.WriteOperation("Starting_ExportMagneticTensionInSpace");
                    ExportMagneticTensionInSpace();
                    break;
                case FileContextMethods.ActionType.ExportWiring:
                    Log.WriteOperation("Starting_ExportWiring");
                    ExportWiring();
                    break;
                case FileContextMethods.ActionType.GenerateVerticesBasedOnOBJ:
                    Log.WriteOperation("Starting_GenerateVerticesBasedOnOBJ");
                    GenerateVerticesBasedOnOBJ();
                    break;
                case FileContextMethods.ActionType.Exit:
                    Log.WriteOperation("Starting_ExitApplication");
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
                case EditContextMethods.ActionType.RemoveMagneticTension:
                    RemoveMagneticTensionPoints();
                    break;
                case EditContextMethods.ActionType.RemoveElectricField:
                    RemoveElectricFieldPoints();
                    break;
                case EditContextMethods.ActionType.EditWiring:
                    EditWiring();
                    break;
            }

            editContextMethods.Panel.HideActiveContextAndStopAutoShow();
        }

        public void ViewContextMethods_Selected(ViewContextMethods viewContextMethods, ViewContextMethods.ActionType actionType)
        {
            //switch (actionType) { }

            viewContextMethods.Panel.HideActiveContextAndStopAutoShow();
        }

        public void CalculationsContextMethods_Selected(CalculationsContextMethods calculationsContextMethods, CalculationsContextMethods.ActionType actionType)
        {
            switch (actionType)
            {
                case CalculationsContextMethods.ActionType.MagneticTensionInSpace:
                    Log.WriteOperation("Starting_Calculate_MagneticTension");
                    Calculate(CalculationType.MagneticTension);
                    break;
                case CalculationsContextMethods.ActionType.ElectricField:
                    Log.WriteOperation("Starting_Calculate_ElectricField");
                    Calculate(CalculationType.ElectricField);
                    break;
                case CalculationsContextMethods.ActionType.Parameters:
                    OpenParameters();
                    break;
            }

            calculationsContextMethods.Panel.HideActiveContextAndStopAutoShow();
        }

        public void WindowContextMethods_Selected(WindowContextMethods windowContextMethods, WindowContextMethods.ActionType actionType)
        {
            switch (actionType)
            {
                case WindowContextMethods.ActionType.OpenProcessWindow:
                    OpenProcessWindow();
                    break;
            }

            windowContextMethods.Panel.HideActiveContextAndStopAutoShow();
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

        #region Magnetic tensions
        public void MagneticTensionInSpace_VisibilityChanged(MathematicBase mathematicBase, bool state)
        {
            if (state)
            {
                if (MathematicManager.Instance.AmperageMode == AmperageMode.Computational && MathematicManager.Instance.MagneticTension.IsCalculated)
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

        public void MagneticTensionInSpace_CurrentTensionFilterRangeChanged(MathematicBase mathematicBase, Range range)
        {
            _tensionFilterSlider.SetMin(range.Min);
            _tensionFilterSlider.SetMax(range.Max);
        }
        #endregion

        #region Electric field
        public void ElectricField_VisibilityChanged(MathematicBase mathematicBase, bool state)
        {
            if (state)
            {
                if (MathematicManager.Instance.AmperageMode == AmperageMode.Computational && MathematicManager.Instance.ElectricField.IsCalculated)
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

        public void ElectricField_CurrentTensionFilterRangeChanged(MathematicBase mathematicBase, Range range)
        {
            _tensionFilterSlider.SetMin(range.Min);
            _tensionFilterSlider.SetMax(range.Max);
        }
        #endregion

        public void TimeManager_TimeIndexChanged(TimeManager timeManager, int index)
        {
            MathematicManager.Instance.CurrentCalculationMethod.SetEntitiesToTime(index);
        }

        public void CalculationSettingsDialog_Applyed(CalculationSettingsWindow calculationSettingsDialog, CalculationSettingsWindow.Settings settings)
        {
            MathematicManager.Instance.RangeLength = settings.RangeLength;
            TimeManager.Instance.SetTimeParameters(settings.TimeRange, settings.TimeStepsCount);

            MathematicManager.Instance.DestroyAllCalculations();
        }

        public void FilterRangeSlider_OnValueChanged(RangeSlider rangeSlider, Range range)
        {
            ((IPointableCalculationMethod)MathematicManager.Instance.CurrentCalculationMethod).FilterPointsByValue(range);
        }

        public void MathematicManager_AmperageModeChanged(MathematicManager mathematicManager, AmperageMode amperageMode)
        {
            if (MathematicManager.Instance.CurrentCalculationMethod == null || !MathematicManager.Instance.CurrentCalculationMethod.IsCalculated)
            {
                _timeLine.StopAndHide();
                return;
            }

            if (amperageMode == AmperageMode.Computational) _timeLine.Show();
            else _timeLine.StopAndHide();
        }

        public void MathematicManager_CalculationShowed(MathematicManager mathematicManager, ICalculationMethod calculationMethod)
        {
            UpdateTensionSlider(true);
        }
        #endregion
        #endregion
    }
}