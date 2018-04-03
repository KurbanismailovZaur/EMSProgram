﻿using Coffee.UIExtensions;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace EMSP.UI
{
    public class RangeSlider : MonoBehaviour
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
        public class RangeSliderEvent : UnityEvent<RangeSlider, float, float> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private RectTransform _fillRect;
        [SerializeField]
        private RectTransform _handleMinRect;
        [SerializeField]
        private RectTransform _handleMaxRect;

        [SerializeField]
        private bool _wholeNumbers;
        [SerializeField]
        private float _minValue;
        [SerializeField]
        private float _maxValue;
        [SerializeField]
        private float _minRangeLenght;

        private UIGradient _bgGradient = null;

        private float _valuesCount;
        private float _valuesPerPixel;
        private float _minHandlesDistance;
        #endregion

        #region Events
        public RangeSliderEvent OnValueChanged = new RangeSliderEvent();
        #endregion

        #region Behaviour
        #region Properties
        public RectTransform HandleMinRect { get { return _handleMinRect; } }
        public RectTransform HandleMaxRect { get { return _handleMaxRect; } }

        public float MinHandlesDistance { get { return _minHandlesDistance; } }
        public float HandleMinYPosition { get { return _handleMinRect.anchoredPosition3D.y; } }
        public float HandleMaxYPosition { get { return _handleMaxRect.anchoredPosition3D.y; } }

        public float CurrentRangeDistance { get { return HandleMaxYPosition - HandleMinYPosition; } }
        public float CurrentMinValue { get { return (_wholeNumbers) ? Convert.ToInt32(_minValue + HandleMinYPosition * _valuesPerPixel) : _minValue + HandleMinYPosition * _valuesPerPixel; } }
        public float CurrentMaxValue { get { return (_wholeNumbers) ? Convert.ToInt32(_minValue + HandleMaxYPosition * _valuesPerPixel) : _minValue + HandleMaxYPosition * _valuesPerPixel; } }

        private UIGradient BgGradient
        {
            get
            {
                if (_bgGradient == null)
                    _bgGradient = GetComponentInChildren<UIGradient>();

                return _bgGradient;
            }
        }

        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Awake()
        {
            CalculateInternalValues();
            RecalculateFillTransformation();
        }

        private void Update()
        {
            RecalculateFillTransformation();
        }

        private void RecalculateFillTransformation()
        {
            float _height = _handleMaxRect.anchoredPosition3D.y - _handleMinRect.anchoredPosition3D.y - _handleMaxRect.rect.height;
            _fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _height);
            _fillRect.anchoredPosition3D = new Vector3(_fillRect.anchoredPosition3D.x, _handleMinRect.anchoredPosition3D.y + _handleMaxRect.rect.height / 2 + _height / 2, _fillRect.anchoredPosition3D.z);
        }

        private void CalculateInternalValues()
        {
            _valuesCount = _maxValue - _minValue;
            if (_minRangeLenght > _valuesCount) _minRangeLenght = _valuesCount;
            _valuesPerPixel = _valuesCount / _handleMinRect.parent.GetComponent<RectTransform>().rect.height;
            _minHandlesDistance = ((_minRangeLenght / (_valuesCount / 100)) / 100) * _handleMinRect.parent.GetComponent<RectTransform>().rect.height;
        }

        public void InvokeValueChanged()
        {
            if (_wholeNumbers)
                OnValueChanged.Invoke(this, Convert.ToInt32(_minValue + HandleMinYPosition * _valuesPerPixel), Convert.ToInt32(_minValue + HandleMaxYPosition * _valuesPerPixel));
            else
                OnValueChanged.Invoke(this, _minValue + HandleMinYPosition * _valuesPerPixel, _minValue + HandleMaxYPosition * _valuesPerPixel);

        }
        public void SetRangeLimits(float min, float max, bool wholeNumbers = false, float minRangeLenght = 0)
        {
            if (minRangeLenght > max - min)
                throw new Exception("Min range lenght can not be more big than all lenght");

            _minValue = min;
            _maxValue = max;
            _wholeNumbers = wholeNumbers;
            _minRangeLenght = minRangeLenght;

            Awake();
        }
        public void SetGradientColors(Color color1, Color color2)
        {
            BgGradient.color2 = color1;
            BgGradient.color1 = color2;

            HandleMinRect.GetComponent<Image>().color = color2;
            HandleMaxRect.GetComponent<Image>().color = color2;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}