using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace EMSP.UI.Control
{
    public class RepeatButton : MonoBehaviour
    {
        #region Entities
        #region Enums
        public enum Type
        {
            Left,
            Up,
            Right,
            Down,
            ZoomIn,
            ZoomOut
        }
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        [Serializable]
        public class RepeatedEvent : UnityEvent<RepeatButton, Type> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private Type _type;

        private Coroutine _repeatRoutine;
        #endregion

        #region Events
        public RepeatedEvent Repeated = new RepeatedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public Type ButtonType { get { return _type; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void StartRepeating()
        {
            if (_repeatRoutine != null)
            {
                return;
            }

            _repeatRoutine = StartCoroutine(RepeatRoutine());
        }

        private IEnumerator RepeatRoutine()
        {
            while (true)
            {
                Repeated.Invoke(this, _type);

                yield return null;
            }
        }

        public void StopRepeating()
        {
            if (_repeatRoutine == null)
            {
                return;
            }

            StopCoroutine(_repeatRoutine);
            _repeatRoutine = null;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void EventTrigger_PointerDown(BaseEventData eventData)
        {
            StartRepeating();
        }

        public void EventTrigger_PointerUp(BaseEventData eventData)
        {
            StopRepeating();
        }
        #endregion
        #endregion
    }
}