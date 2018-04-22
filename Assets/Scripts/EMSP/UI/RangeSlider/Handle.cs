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
        private RectTransform _rectTransform;
        private float _maxValue;
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
            _rectTransform = GetComponent<RectTransform>();
            _maxValue = _rectTransform.parent.GetComponent<RectTransform>().rect.height;
        }

        private void Start()
        {
            _rangeSlider = GetComponentInParent<RangeSlider>();
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
            ValidateAndSetNewYPosition(_lastValue / _rangeSlider.ValuesPerPixel);
        }

        public void SetValue(float value)
        {
            ValidateAndSetNewYPosition(value / _rangeSlider.ValuesPerPixel);
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

            _rectTransform.anchoredPosition3D = new Vector3(_rectTransform.anchoredPosition3D.x, yPosition, _rectTransform.anchoredPosition3D.z);
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void OnBeginDrag(PointerEventData eventData)
        {
            _isDragByUser = true;
        }

        public void OnDrag(PointerEventData eventData)
        {
            float newYPos = _rectTransform.anchoredPosition3D.y + eventData.delta.y;
            ValidateAndSetNewYPosition(newYPos);
            _rangeSlider.InvokeValueChanged();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            _isDragByUser = false;
        }
        #endregion
        #endregion
    }
}