using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace EMSP.UI
{
    public class Fill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        private RangeSlider _rangeSlider;
        private Handle _minHandle;
        private Handle _maxHandle;

        private float _currentRange;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Start()
        {
            _rangeSlider = GetComponentInParent<RangeSlider>();
            _minHandle = _rangeSlider.HandleMinRect.GetComponent<Handle>();
            _maxHandle = _rangeSlider.HandleMaxRect.GetComponent<Handle>();
        }

        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void OnBeginDrag(PointerEventData eventData)
        {
            _currentRange = _rangeSlider.CurrentRangeDistance;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _minHandle.ValidateAndSetNewYPosition(_rangeSlider.HandleMinYPosition + eventData.delta.y, _currentRange);
            _maxHandle.ValidateAndSetNewYPosition(_rangeSlider.HandleMaxYPosition + eventData.delta.y, _currentRange);
            _rangeSlider.InvokeValueChanged();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnScroll(float deltaY)
        {
            _currentRange = _rangeSlider.CurrentRangeDistance;

            _minHandle.ValidateAndSetNewYPosition(_rangeSlider.HandleMinYPosition + deltaY, _currentRange);
            _maxHandle.ValidateAndSetNewYPosition(_rangeSlider.HandleMaxYPosition + deltaY, _currentRange);
            _rangeSlider.InvokeValueChanged();
        }
        #endregion
        #endregion
    }
}