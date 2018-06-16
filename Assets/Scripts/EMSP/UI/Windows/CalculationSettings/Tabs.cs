﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace EMSP.UI.Windows.CalculationSettings
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

        [SerializeField]
        private Color _selectedColor = Color.white;

        [SerializeField]
        private Color _unselectedColor = new Color32(228, 228, 228, 255);
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

            _selectedTab.Color = _unselectedColor;
            _selectedTab.AssociatedPanel.Hide();

            _selectedTab = tab;
            _selectedTab.Color = _selectedColor;
            _selectedTab.AssociatedPanel.Show();
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