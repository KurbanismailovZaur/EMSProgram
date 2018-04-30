using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;


namespace EMSP.UI
{
    public class Fill : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler, IPointerDownHandler
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
        private RangeSlider _rangeSlider;

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
        #endregion

        #region Indexers
        #endregion

        #region Events handlers

        private float offset;
        private float fromFillCenterOffset;

        public void OnPointerDown(PointerEventData eventData)
        {
            _currentRange = _rangeSlider.CurrentRangeDistance;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            offset = _rangeSlider.GetComponent<RectTransform>().position.y - _rangeSlider.GetComponent<RectTransform>().rect.height / 2 + _rangeSlider.HandleMin.GetComponent<RectTransform>().rect.height;
            fromFillCenterOffset = GetComponent<RectTransform>().position.y - Input.mousePosition.y;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //_rangeSlider.HandleMin.ValidateAndSetNewYPosition(Input.mousePosition.y - offset - _currentRange / 2, _currentRange);
            //_rangeSlider.HandleMax.ValidateAndSetNewYPosition(Input.mousePosition.y - offset + _currentRange / 2, _currentRange);

            //_rangeSlider.InvokeValueChanged();

            _rangeSlider.TryChangeValue(Input.mousePosition.y - offset - _currentRange / 2 + fromFillCenterOffset, Input.mousePosition.y - offset + _currentRange / 2 + fromFillCenterOffset, true);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnScroll(float deltaY)
        {
            //_rangeSlider.HandleMin.ValidateAndSetNewYPosition(_rangeSlider.HandleMinYPosition + deltaY, _currentRange);
            //_rangeSlider.HandleMax.ValidateAndSetNewYPosition(_rangeSlider.HandleMaxYPosition + deltaY, _currentRange);

            //_rangeSlider.InvokeValueChanged();

            _rangeSlider.TryChangeValue(_rangeSlider.HandleMinYPosition + deltaY, _rangeSlider.HandleMaxYPosition + deltaY, true);
        }

        #endregion
        #endregion
    }
}