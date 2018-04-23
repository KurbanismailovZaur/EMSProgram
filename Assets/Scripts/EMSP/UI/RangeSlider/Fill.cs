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
        public void OnBeginDrag(PointerEventData eventData)
        {
            _currentRange = _rangeSlider.CurrentRangeDistance;
            offset = _rangeSlider.GetComponent<RectTransform>().position.y - _rangeSlider.GetComponent<RectTransform>().rect.height / 2 + _rangeSlider.HandleMin.GetComponent<RectTransform>().rect.height / 2;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rangeSlider.HandleMin.ValidateAndSetNewYPosition(Input.mousePosition.y - offset - _currentRange / 2, _currentRange);
            _rangeSlider.HandleMax.ValidateAndSetNewYPosition(Input.mousePosition.y - offset + _currentRange / 2, _currentRange);

            _rangeSlider.InvokeValueChanged();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
        }

        public void OnScroll(float deltaY)
        {
            _currentRange = _rangeSlider.CurrentRangeDistance;

            _rangeSlider.HandleMin.ValidateAndSetNewYPosition(_rangeSlider.HandleMinYPosition + deltaY, _currentRange);
            _rangeSlider.HandleMax.ValidateAndSetNewYPosition(_rangeSlider.HandleMaxYPosition + deltaY, _currentRange);

            _rangeSlider.InvokeValueChanged();
        }
        #endregion
        #endregion
    }
}