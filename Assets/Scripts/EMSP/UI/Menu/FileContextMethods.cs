using Numba.UI.Menu;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.UI.Menu
{
    [RequireComponent(typeof(Context))]
    public class FileContextMethods : MonoBehaviour
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
        private Context _context;

        private Panel _panel;
        #endregion

        #region Events
        public SelectedEvent Selected = new SelectedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public Panel Panel { get { return _panel; } }
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