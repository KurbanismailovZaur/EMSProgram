using EMSP.Data.Serialization;
using EMSP.Data.Serialization.EMSP;
using Numba;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace EMSP.App
{
	public class ProjectManager : MonoSingleton<ProjectManager> 
	{
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class ProjectCreatedEvent : UnityEvent<Project> { }

        [Serializable]
        public class ProjectDestroyedEvent : UnityEvent<Project> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        private Project _project;
        #endregion

        #region Events
        public ProjectCreatedEvent ProjectCreated = new ProjectCreatedEvent();

        public ProjectDestroyedEvent ProjectDestroyed = new ProjectDestroyedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public Project Project { get { return _project; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void CreateNewProject()
        {
            _project = new Project();

            ProjectCreated.Invoke(_project);
        }

        public void CloseProject()
        {
            _project = null;

            ProjectDestroyed.Invoke(_project);
        }

        public void SaveProject(string path)
        {
            _project.Save(path);
        }

        public void ResaveProject()
        {
            _project.Resave();
        }

        public void OpenProject(string path)
        {
            CreateNewProject();

            _project.Load(path);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}