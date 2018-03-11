using Numba.UI.Menu;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.UI.Menu.Contexts
{
	public abstract class ContextMethodsBase : MonoBehaviour 
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
        protected Context _context;

        protected Panel _panel;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        public Panel Panel { get { return _panel; } }
        #endregion

        #region Constructors
        #endregion

        #region Methods
        protected virtual void Awake()
        {
            _context = GetComponent<Context>();

            _panel = ((Group)_context.ContextContainer).Panel;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        #endregion
        #endregion
    }
}