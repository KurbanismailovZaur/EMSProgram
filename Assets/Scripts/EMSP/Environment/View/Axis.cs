using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace EMSP.Environment.View
{
	public class Axis : MonoBehaviour 
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
        public class SelectedEvent : UnityEvent<Axis, AxisDirection> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private AxisDirection _direction;

        private Renderer _renderer;

        private Color _selfColor;

        [SerializeField]
        private Color _highlightColor = Color.yellow;
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
        private void Awake()
        {
            _renderer = GetComponent<Renderer>();
            _selfColor = _renderer.material.color;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void EventTrigger_PointerClick(BaseEventData eventData)
        {
            Selected.Invoke(this, _direction);
        }

        public void EventTrigger_PointerEnter(BaseEventData eventData)
        {
            _renderer.sharedMaterial.color = _highlightColor;
        }

        public void EventTrigger_PointerExit(BaseEventData eventData)
        {
            _renderer.sharedMaterial.color = _selfColor;
        }
        #endregion
        #endregion
    }
}