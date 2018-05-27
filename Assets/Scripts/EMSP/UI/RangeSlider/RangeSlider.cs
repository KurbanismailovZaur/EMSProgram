using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

namespace EMSP.UI
{
    public class RangeSlider : MonoBehaviour, IScrollHandler
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
        public class RangeSliderEvent : UnityEvent<RangeSlider, Range> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private RectTransform _fillRect;

        [SerializeField]
        private Handle _handleMin;

        [SerializeField]
        private Handle _handleMax;

        [SerializeField]
        private Fill _fill;

        [SerializeField]
        private bool _wholeNumbers;

        [SerializeField]
        private float _minRangeValue;

        [SerializeField]
        private float _maxRangeValue;

        [SerializeField]
        private float _minRangeLenght;

        [SerializeField]
        private float _minValue;

        [SerializeField]
        private float _maxValue;

        [SerializeField]
        private float _scrollSensivity = 1;

        private Material _bgGradient = null;

        private float _valuesCount;

        private float _valuesPerPixel;

        private float _minHandlesDistance;

        private int _previousScreenHeight;

        private int _previousScreenWidth;

        #endregion

        #region Events
        public RangeSliderEvent OnValueChanged = new RangeSliderEvent();
        public RangeSliderEvent OnTryValueChanging = new RangeSliderEvent();
        #endregion

        #region Behaviour
        #region Properties

        public float Min { get { return _minValue; } set { SetMin(value); } }
        public float Max { get { return _maxValue; } set { SetMax(value); } }


        public Handle HandleMin { get { return _handleMin; } }
        public Handle HandleMax { get { return _handleMax; } }

        public float ValuesPerPixel { get { return _valuesPerPixel; } }
        public float MinHandlesDistance { get { return _minHandlesDistance; } }
        public float HandleMinYPosition { get { return _handleMin.RectTransform.anchoredPosition3D.y; } }
        public float HandleMaxYPosition { get { return _handleMax.RectTransform.anchoredPosition3D.y; } }


        public float MinRangeValue { get { return _minRangeValue; } }
        public float MaxRangeValue { get { return _maxRangeValue; } }



        public float CurrentRangeDistance { get { return HandleMaxYPosition - HandleMinYPosition; } }
        public float CurrentMinValue { get { return (_wholeNumbers) ? Convert.ToInt32(_minRangeValue + HandleMinYPosition * _valuesPerPixel) : _minRangeValue + HandleMinYPosition * _valuesPerPixel; } }
        public float CurrentMaxValue { get { return (_wholeNumbers) ? Convert.ToInt32(_minRangeValue + HandleMaxYPosition * _valuesPerPixel) : _minRangeValue + HandleMaxYPosition * _valuesPerPixel; } }
        public float CurrentRangeLenght { get { return Max - Min; } }

        private float _curDinamycRange = 0;
        public float CurDinamycRange { get { return _curDinamycRange; } set { _curDinamycRange = value * _valuesPerPixel; } }

        private Material BgGradient
        {
            get
            {
                if (_bgGradient == null)
                    _bgGradient = transform.GetChild(0).GetComponent<Image>().material;

                return _bgGradient;
            }
        }

        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void Start()
        {
            RecalculateAll();
            UpdateValues();
        }

        private void RecalculateAll()
        {
            _previousScreenHeight = Screen.height;
            _previousScreenWidth = Screen.width;

            CalculateInternalValues();
            RecalculateFillTransformation();
        }

        private void Update()
        {
            if (IsScreenSizeChanged())
            {
                RecalculateAll();

                _handleMin.RecalculateInternalValues();
                _handleMax.RecalculateInternalValues();

                return;
            }

            RecalculateFillTransformation();
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private bool IsScreenSizeChanged()
        {
            return Screen.height != _previousScreenHeight || Screen.width != _previousScreenWidth;
        }

        private void RecalculateFillTransformation()
        {
            float _height = _handleMax.RectTransform.anchoredPosition3D.y - _handleMin.RectTransform.anchoredPosition3D.y;// - _handleMax.RectTransform.rect.height;

            _fillRect.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, _height);
            _fillRect.anchoredPosition3D = new Vector3(_fillRect.anchoredPosition3D.x, _handleMin.RectTransform.anchoredPosition3D.y + _height / 2, _fillRect.anchoredPosition3D.z);

        }

        private void CalculateInternalValues()
        {
            _valuesCount = _maxRangeValue - _minRangeValue;
            if (_minRangeLenght > _valuesCount) _minRangeLenght = _valuesCount;
            _valuesPerPixel = _valuesCount / ((RectTransform)_handleMin.transform.parent).rect.height;
            _minHandlesDistance = ((_minRangeLenght / (_valuesCount / 100)) / 100) * ((RectTransform)_handleMin.transform.parent).rect.height;
        }

        [ContextMenu("Update values")]
        private void UpdateValues()
        {
            SetMin(_minValue);
            SetMax(_maxValue);
        }

        public void TryChangeValue(float minYpos, float maxYpos, bool byFillDragging = false)
        {
            float newMinValue = _minRangeValue + minYpos * _valuesPerPixel;
            float newMaxValue = _minRangeValue + maxYpos * _valuesPerPixel;


            if(byFillDragging)
            {

                if (Min == MinRangeValue && newMaxValue <= Min + CurDinamycRange)
                {
                    newMinValue = Min;
                    newMaxValue = Min + CurDinamycRange;
                }
                if (Max == MaxRangeValue && newMinValue >= Max - CurDinamycRange)
                {
                    newMinValue = Max - CurDinamycRange;
                    newMaxValue = Max;
                }
            }

            if (_wholeNumbers)
            {
                OnTryValueChanging.Invoke(this, new Range(Convert.ToInt32(newMinValue), Convert.ToInt32(newMaxValue)));
            }
            else
            {
                OnTryValueChanging.Invoke(this, new Range(newMinValue, newMaxValue));
            }
        }

        public void SetRangeLimits(float min, float max, bool wholeNumbers = false, float minRangeLenght = 0)
        {
            if (minRangeLenght > max - min)
            {
                throw new Exception("Min range lenght can not be more big than all lenght");
            }

            _minRangeValue = min;
            _maxRangeValue = max;
            _wholeNumbers = wholeNumbers;
            _minRangeLenght = minRangeLenght;

            RecalculateAll();
        }

        public void SetMin(float min)
        {
            if (_wholeNumbers)
                min = Convert.ToInt32(min);

            _handleMin.SetValue(min);
            _minValue = min;
        }

        public void SetMax(float max)
        {
            if (_wholeNumbers)
                max = Convert.ToInt32(max);

            _handleMax.SetValue(max);
            _maxValue = max;

            if (!started && isActiveAndEnabled) StartCoroutine(WaitAndUpdateValues());
        }

        public void SetGradientColors(Color color0, Color color1, Color color2, Color color3, Color color4, Color color5, Color color6, Color color7)
        {
            BgGradient.SetColor("_Color0", color0);
            BgGradient.SetColor("_Color1", color1);
            BgGradient.SetColor("_Color2", color2);
            BgGradient.SetColor("_Color3", color3);
            BgGradient.SetColor("_Color4", color4);
            BgGradient.SetColor("_Color5", color5);
            BgGradient.SetColor("_Color6", color6);
            BgGradient.SetColor("_Color7", color7);
        }

        bool started = false;
        IEnumerator WaitAndUpdateValues()
        {
            started = true;
            yield return new WaitForEndOfFrame();
            yield return new WaitForEndOfFrame();

            UpdateValues();
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void OnScroll(PointerEventData eventData)
        {
            _fill.OnScroll(eventData.scrollDelta.y * _scrollSensivity);
        }
        #endregion
        #endregion
    }
}