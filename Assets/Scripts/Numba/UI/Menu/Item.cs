using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Numba.UI.Menu
{
    public class Item : ContextContainer
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
        public class PointerEnterEvent : UnityEvent<Item> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private Context _baseContext;

        [SerializeField]
        private Text _text;
        #endregion

        #region Events
        public PointerEnterEvent PointerEnter = new PointerEnterEvent();
        #endregion

        #region Behaviour
        #region Properties
        public Context BaseContext { get { return _baseContext; } }

        public Text Text { get { return _text; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        #endregion

        #region Events handlers
        public virtual void EventTrigger_PointerEnter(BaseEventData baseEventData)
        {
            PointerEnter.Invoke(this);
        }
        #endregion
        #endregion
    }
}