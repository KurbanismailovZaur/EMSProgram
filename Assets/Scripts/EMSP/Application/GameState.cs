using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.Application
{
    public abstract class GameState : MonoBehaviour
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
        protected Game _game;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            _game = GetComponentInParent<Game>();
        }

        public virtual void OnEnter() { }

        public virtual void OnExit() { }

        #region Voice Commands
        public virtual void ShowMenu() { }

        public virtual void ShowBuildings() { }

        public virtual void ShowFurnitures() { }

        public virtual void ShowTutorials() { }

        public virtual void Jump() { }

        public virtual void Fly() { }

        public virtual void FromInside() { }

        public virtual void FromOutside() { }

        public virtual void Replace() { }

        public virtual void ShowLayouts() { }

        public virtual void ShowViewPoints() { }

        public virtual void NextView() { }

        public virtual void WhereWindow() { }

        public virtual void ShowLog() { }

        public virtual void HideWindow() { }
        #endregion
        #endregion

        #region Event Handlers
        #endregion
        #endregion
    }
}