﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.UI.Dialogs.CalculationSettings
{
	public class Tabs : MonoBehaviour 
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
        private Tab _selectedTab;
		#endregion
		
		#region Events
		#endregion
		
		#region Behaviour
		#region Properties
		#endregion
		
		#region Constructors
		#endregion
		
		#region Methods
        private void SelectTab(Tab tab)
        {
            if (tab == _selectedTab)
            {
                return;
            }
        }
		#endregion
		
		#region Indexers
		#endregion
		
		#region Events handlers
        public void Tab_Clicked(Tab tab)
        {
            SelectTab(tab);
        }
		#endregion
		#endregion
	}
}