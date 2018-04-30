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

        private float _maxValue;

        [SerializeField]
        private RangeSlider _rangeSlider;

        private bool _isDragByUser = false;

        private float _lastValue;

        [SerializeField]
        private Image _image;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public RectTransform RectTransform { get { return (RectTransform)transform; } }

        public Image Image { get { return _image; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            _maxValue = _rectTransform.parent.GetComponent<RectTransform>().rect.height;
        }

        private void Update()
        {
            if (!_isDragByUser)
            {
                if (IsMin)
                {
                    if (_rangeSlider.HandleMaxYPosition - _rangeSlider.HandleMinYPosition < _rangeSlider.MinHandlesDistance)
                    {
                        ValidateAndSetNewYPosition(_rangeSlider.HandleMaxYPosition - _rangeSlider.MinHandlesDistance);
                    }
                }
                else
                {
                    if (_rangeSlider.HandleMaxYPosition - _rangeSlider.HandleMinYPosition < _rangeSlider.MinHandlesDistance)
                    {
                        ValidateAndSetNewYPosition(_rangeSlider.HandleMinYPosition + _rangeSlider.MinHandlesDistance);
                    }
                }
            }

            if(IsMin)
            {
                _lastValue = _rangeSlider.CurrentMinValue;
                _textField.text = _rangeSlider.CurrentMinValue.ToString();
            }
            else
            {
                _lastValue = _rangeSlider.CurrentMaxValue;
                _textField.text = _rangeSlider.CurrentMaxValue.ToString();
            }
        }

        public void RecalculateInternalValues()
        {
            _maxValue = _rectTransform.parent.GetComponent<RectTransform>().rect.height;
            SetValue(_lastValue);
        }

        public void SetValue(float value)
        {
            ValidateAndSetNewYPosition((value - _rangeSlider.MinRangeValue) / _rangeSlider.ValuesPerPixel);
        }

        public void ValidateAndSetNewYPosition(float yPosition, float handleDistance = -1)
        {
            float distance = (handleDistance == -1) ? _rangeSlider.MinHandlesDistance : handleDistance;

            if (IsMin)
            {
                if (yPosition < 0)
                    yPosition = 0;
                else if (yPosition > _maxValue - distance)
                    yPosition = _maxValue - distance;
            }
            else
            {
                if (yPosition > _maxValue)
                    yPosition = _maxValue;
                else if (yPosition < distance)
                    yPosition = distance;
            }

            _rectTransform.ForceUpdateRectTransforms();
            _rectTransform.anchoredPosition3D = new Vector3(_rectTransform.anchoredPosition3D.x, yPosition, _rectTransform.anchoredPosition3D.z);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers

        private float offset;
        public void OnBeginDrag(PointerEventData eventData)
        {
            offset = _rangeSlider.GetComponent<RectTransform>().position.y - _rangeSlider.GetComponent<RectTransform>().rect.height / 2 + _rectTransform.rect.height;
            _isDragByUser = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            //    ValidateAndSetNewYPosition(Input.mousePosition.y - offset);
            //    _rangeSlider.InvokeValueChanged();

            if (IsMin)
                _rangeSlider.TryChangeValue(Input.mousePosition.y - offset, _rangeSlider.HandleMaxYPosition);
            else
                _rangeSlider.TryChangeValue(_rangeSlider.HandleMinYPosition, Input.mousePosition.y - offset);
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragByUser = false;
        }
        #endregion
        #endregion
    }
}