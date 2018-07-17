using EMSP.Communication;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using UnityEngine.EventSystems;

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
            public WireRow Create(WireRow wireRowPrefab, RectTransform parent, Wire wire, float value)
            {
                WireRow wireRow = Instantiate(wireRowPrefab, parent, false);

                wireRow._nameField.text = wire.Name;
                wireRow._valueField.text = value.ToString();
                wireRow._representableWire = wire;

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
