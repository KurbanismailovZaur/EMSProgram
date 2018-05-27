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
        private void Update()
        {
            if(_beginDrag)
                OnDrag(null);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers

        private float offset;
        private float fromFillCenterOffset;
        private bool _beginDrag;

        public void OnPointerDown(PointerEventData eventData)
        {
            _currentRange = _rangeSlider.CurrentRangeDistance;
            _rangeSlider.CurDinamycRange = _currentRange;
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _beginDrag = true;
            _rangeSlider.HandleMin.IsDragByUser = true;
            _rangeSlider.HandleMax.IsDragByUser = true;

            offset = _rangeSlider.GetComponent<RectTransform>().position.y - _rangeSlider.GetComponent<RectTransform>().rect.height / 2 + _rangeSlider.HandleMin.GetComponent<RectTransform>().rect.height;
            fromFillCenterOffset = GetComponent<RectTransform>().position.y - Input.mousePosition.y;
        }

        public void OnDrag(PointerEventData eventData)
        {
            _rangeSlider.TryChangeValue(Input.mousePosition.y - offset - _currentRange / 2 + fromFillCenterOffset, Input.mousePosition.y - offset + _currentRange / 2 + fromFillCenterOffset, true);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _beginDrag = false;

            _rangeSlider.HandleMin.IsDragByUser = false;
            _rangeSlider.HandleMax.IsDragByUser = false;
        }

        public void OnScroll(float deltaY)
        {
            _rangeSlider.TryChangeValue(_rangeSlider.HandleMinYPosition + deltaY, _rangeSlider.HandleMaxYPosition + deltaY, true);
        }

        #endregion
        #endregion
    }
}