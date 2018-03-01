using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace HoloGroup.UI.Menu
{
    public class Context : MonoBehaviour
    {
        #region Entities
        #region Enums
        #endregion

        #region Delegates
        #endregion

        #region Structures
        #endregion

        #region Classes
        public class ShowedEvent : UnityEvent<Context> { }

        public class HidedEvent : UnityEvent<Context> { }
        #endregion

        #region Interfaces
        #endregion
        #endregion

        #region Fields
        [SerializeField]
        private CanvasGroup _canvasGroup;

        [SerializeField]
        private ContextContainer _contextContainer;

        [SerializeField]
        private List<Item> _items = new List<Item>();

        private bool _isShowed;

        private ItemGroup _activeItemGroup;
        #endregion

        #region Events
        public ShowedEvent Showed = new ShowedEvent();

        public HidedEvent Hided = new HidedEvent();
        #endregion

        #region Behaviour
        #region Properties
        public ContextContainer ContextContainer { get { return _contextContainer; } }

        public ReadOnlyCollection<Item> ItemGroup { get { return _items.AsReadOnly(); } }

        public bool IsShowed { get { return _isShowed; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void Show()
        {
            if (_isShowed)
            {
                return;
            }

            _canvasGroup.alpha = 1f;
            _canvasGroup.blocksRaycasts = true;

            _isShowed = true;

            Showed.Invoke(this);
        }

        public void Hide()
        {
            if (!_isShowed)
            {
                return;
            }

            _canvasGroup.alpha = 0f;
            _canvasGroup.blocksRaycasts = false;

            HideAdditionalContext();

            _isShowed = false;

            Hided.Invoke(this);
        }

        public void HideAdditionalContext()
        {
            if (_activeItemGroup != null)
            {
                _activeItemGroup.Context.Hide();
                _activeItemGroup = null;
            }
        }

        public Item GetItemWithMaxWidth()
        {
            if (_items.Count == 0)
            {
                return null;
            }

            Item widedItemGroup = _items[0];

            foreach (Item item in _items)
            {
                if (item.Text.preferredWidth > widedItemGroup.Text.preferredWidth)
                {
                    widedItemGroup = item;
                }
            }

            return widedItemGroup;
        }
        #endregion

        #region Events handlers
        public void Item_PointerEnter(Item item)
        {
            HideAdditionalContext();

            _activeItemGroup = item as ItemGroup;
        }
        #endregion
        #endregion
    }
}