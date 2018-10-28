using Numba.UI.Menu;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Menu.Contexts
{
    public class FileContextMethods : ContextMethodsBase
    {
        #region Entities
        #region Enums
        public enum ActionType
        {
            NewProject,
            OpenProject,
            SaveProject,
            CloseProject,
            ImportModel,
            ImportWiring,
            ExportMagneticTensionInSpace,
            ExportWiring,
            ExportVertices,
            GenerateVerticesBasedOnOBJ,
            Exit
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class SelectedEvent : UnityEvent<FileContextMethods, ActionType> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        #endregion

        #region Events
        public SelectedEvent Selected = new SelectedEvent();
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void NewProject()
        {
            Selected.Invoke(this, ActionType.NewProject);
        }

        public void OpenProject()
        {
            Selected.Invoke(this, ActionType.OpenProject);
        }

        public void SaveProject()
        {
            Selected.Invoke(this, ActionType.SaveProject);
        }

        public void CloseProject()
        {
            Selected.Invoke(this, ActionType.CloseProject);
        }

        public void ImportModel()
        {
            Selected.Invoke(this, ActionType.ImportModel);
        }

        public void ImportWiring()
        {
            Selected.Invoke(this, ActionType.ImportWiring);
        }

        public void ExportMagneticTensionInSpace()
        {
            Selected.Invoke(this, ActionType.ExportMagneticTensionInSpace);
        }

        public void ExportWiring()
        {
            Selected.Invoke(this, ActionType.ExportWiring);
        }

        public void ExportVertices()
        {
            Selected.Invoke(this, ActionType.ExportVertices);
        }

        public void GenerateVerticesBasedOnOBJ()
        {
            Selected.Invoke(this, ActionType.GenerateVerticesBasedOnOBJ);
        }

        public void Exit()
        {
            Selected.Invoke(this, ActionType.Exit);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}