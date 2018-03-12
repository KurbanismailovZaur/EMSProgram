using EMSP.App;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Grid = EMSP.Environment.Grid;

namespace EMSP.UI.Menu.Contexts.View
{
	public class GridVisibilitySwitcher : MonoBehaviour 
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
        private Image _stateImage;
        #endregion

        #region Events
        #endregion

        #region Behaviour
        #region Properties
        #endregion

        #region Constructors
        #endregion

        #region Methods
        private void TrySwitchVisibility()
        {
            Grid.Instance.Visibility = !Grid.Instance.Visibility;
        }
		#endregion
		
		#region Indexers
		#endregion
			
		#region Events handlers
        public void Button_OnClick()
        {
            TrySwitchVisibility();
        }

        public void Grid_VisibilityChanged(Grid grid, bool state)
        {
            _stateImage.enabled = state;
        }
        #endregion
        #endregion
    }
}
