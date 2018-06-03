using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace EMSP.UI
{
    public class Handle : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
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
        public bool IsMin = true;

        [SerializeField]
        private Text _textField;

        [SerializeField]
        private RectTransform _rectTransform;

        private float _maxValue = -1;

        [SerializeField]
        private RangeSlider _rangeSlider;

        public bool IsDragByUser { get; set; }

        [SerializeField]
        private Image _image;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public RectTransform RectTransform { get { return (RectTransform)transform; } }

        public Image Image { get { return _image; } }

        private float MaxValue
        {
            get
            {
                if(_maxValue == -1)
                    _maxValue = _rectTransform.parent.GetComponent<RectTransform>().rect.height;

                return _maxValue;
            }
        }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            _maxValue = _rectTransform.parent.GetComponent<RectTransform>().rect.height;
            IsDragByUser = false;
        }


        public void RecalculateInternalValues()
        {
            _maxValue = _rectTransform.parent.GetComponent<RectTransform>().rect.height;
        }

        public void SetValue(float value)
        {
            ValidateAndSetNewYPosition((value - _rangeSlider.MinRangeValue) / _rangeSlider.ValuesPerPixel);
            _textField.text = value.ToString();
        }

        public void ValidateAndSetNewYPosition(float yPosition, float handleDistance = -1)
        {
            if (float.IsNaN(yPosition)) return;

            float distance = (handleDistance == -1) ? _rangeSlider.MinHandlesDistance : handleDistance;

            if (IsMin)
            {
                if (yPosition < 0)
                    yPosition = 0;
                else if (yPosition > MaxValue - distance)
                    yPosition = MaxValue - distance;
            }
            else
            {
                if (yPosition > MaxValue)
                    yPosition = MaxValue;
                else if (yPosition < distance)
                    yPosition = distance;
            }

            _rectTransform.anchoredPosition3D = new Vector3(_rectTransform.anchoredPosition3D.x, yPosition, _rectTransform.anchoredPosition3D.z);
            _rectTransform.ForceUpdateRectTransforms();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers

        private float offset;
        public void OnBeginDrag(PointerEventData eventData)
        {
            offset = _rangeSlider.GetComponent<RectTransform>().position.y - _rangeSlider.GetComponent<RectTransform>().rect.height / 2 + _rectTransform.rect.height;
            IsDragByUser = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            if (IsMin)
                _rangeSlider.TryChangeValue(Input.mousePosition.y - offset, _rangeSlider.HandleMaxYPosition);
            else
                _rangeSlider.TryChangeValue(_rangeSlider.HandleMinYPosition, Input.mousePosition.y - offset);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            IsDragByUser = false;
        }
        #endregion
        #endregion
    }
}