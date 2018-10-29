using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using EMSP.Mathematic;

namespace EMSP.UI.Windows.CalculatedInduction
{
    public class WireRow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes

        public class Factory
        {
            public WireRow Create(WireRow wireRowPrefab, RectTransform parent, KeyValuePair<Wire, float> precomputed, VectorableCalculatedValueInTime[] calculated, AmperageMode mode, int currentTimeIndex)
            {
                WireRow wireRow = Instantiate(wireRowPrefab, parent, false);

                wireRow._nameField.text = precomputed.Key.Name;
                wireRow._representableWire = precomputed.Key;
                wireRow._precomputedValue = precomputed.Value;

                //for (int timeIndex = 0; timeIndex < calculated.Length; ++ timeIndex)
                //{
                //    foreach (var kvp in calculated[timeIndex].CalculatedValue)
                //    {
                //        if (kvp.Key == wireRow._representableWire)
                //        {
                //            wireRow._calculatedValues.Add(timeIndex, kvp.Value);
                //        }
                //    }
                //}


                for (int timeIndex = 0; timeIndex < Timing.TimeManager.Instance.StepsCount; ++timeIndex)
                {
                    wireRow._calculatedValues.Add(timeIndex, precomputed.Value);
                }



                if (mode == AmperageMode.Precomputed)
                {
                    wireRow._valueField.text = wireRow._precomputedValue.ToString();
                }
                else if (mode == AmperageMode.Computational)
                {
                    wireRow.SetTimeStep(currentTimeIndex);
                }
                else
                    Debug.LogError("Unexpected AmperageMode");


                return wireRow;
            }
        }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private Text _nameField;

        [SerializeField]
        private Text _valueField;

        private Image _backLightImage;

        private Color _defaultColor;

        private Wire _representableWire;

        private float _precomputedValue;

        private Dictionary<int, float> _calculatedValues = new Dictionary<int, float>();

        private int _currentTimeIndex;
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
            _backLightImage = GetComponent<Image>();
            _defaultColor = _backLightImage.color;
        }

        public void SetTimeStep(int timeIndex)
        {
            if (_valueField == null) return;

            _currentTimeIndex = timeIndex;
            _valueField.text = _calculatedValues[_currentTimeIndex].ToString();
        }

        public void SetAmperageMode(AmperageMode mode)
        {
            if (_valueField == null) return;

            if (mode == AmperageMode.Precomputed)
            {
                _valueField.text = _precomputedValue.ToString();
            }
            else if (mode == AmperageMode.Computational)
            {
                SetTimeStep(_currentTimeIndex);
            }
            else
                Debug.LogError("Unexpected AmperageMode");
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void OnPointerEnter(PointerEventData eventData)
        {
            _representableWire.SetWireHighlight(true);
            _backLightImage.color = Color.green;
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            _representableWire.SetWireHighlight(false);
            _backLightImage.color = _defaultColor;
        }
        #endregion
        #endregion
    }
}
