using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace HoloGroup.UI.Menu
{
    public class ItemGroup : Item
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
        private Context _context;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Context Context { get { return _context; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        public void ShowContext()
        {
            _context.Show();
        }

        public void HideContext()
        {
            _context.Hide();
        }

        public override void EventTrigger_PointerEnter(BaseEventData baseEventData)
        {
            base.EventTrigger_PointerEnter(baseEventData);
            ShowContext();
        }
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}