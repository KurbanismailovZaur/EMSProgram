using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Numba.UI.Menu
{
    public class FreeSpaceFitter : MonoBehaviour, ILayoutSelfController, ILayoutElement
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
        private ItemGroup _itemGroup;

        private float _minWidth;

        private float _preferredWidth;

        private float _flexibleWidth;

        private float _minHeight;

        private float _preferredHeight;

        private float _flexibleHeight;

        [SerializeField]
        private int _layoutPriority = 1;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public ItemGroup ItemGroup { get { return _itemGroup; } }

        public float minWidth { get { return _minWidth; } }

        public float preferredWidth { get { return _preferredWidth; } }

        public float flexibleWidth { get { return 0f; } }

        public float minHeight { get { return _minHeight; } }

        public float preferredHeight { get { return _preferredHeight; } }

        public float flexibleHeight { get { return 0f; } }

        public int layoutPriority { get { return _layoutPriority; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void CalculateLayoutInputHorizontal()
        {
            Item item = _itemGroup.BaseContext.GetItemWithMaxWidth();

            if (item == ItemGroup)
            {
                _minWidth = 0f;
                _preferredWidth = 0f;
            }
            else
            {
                RectTransform textRectTransform = (RectTransform)item.Text.transform;

                _minWidth = Mathf.Max(textRectTransform.anchoredPosition.x + item.Text.preferredWidth -(((RectTransform)ItemGroup.Text.transform).anchoredPosition.x + ItemGroup.Text.preferredWidth), 0f);
                _preferredWidth = _minWidth;
            }

            _flexibleWidth = 0f;
        }

        public void CalculateLayoutInputVertical()
        {
            _minHeight = ((RectTransform)transform).sizeDelta.y;
            _preferredHeight = _minHeight;
            _flexibleHeight = 0f;
        }

        public void SetLayoutHorizontal()
        {
            Vector2 sizeDelta = ((RectTransform)transform).sizeDelta;
            sizeDelta.x = _preferredWidth;
            ((RectTransform)transform).sizeDelta = sizeDelta;
        }

        public void SetLayoutVertical()
        {
            Vector2 sizeDelta = ((RectTransform)transform).sizeDelta;
            sizeDelta.y = _preferredHeight;
            ((RectTransform)transform).sizeDelta = sizeDelta;
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}