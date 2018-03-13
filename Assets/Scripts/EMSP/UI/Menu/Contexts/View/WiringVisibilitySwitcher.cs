using EMSP.Communication;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Menu.Contexts.View
{
	public class WiringVisibilitySwitcher : MonoBehaviour 
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
            WiringManager.Instance.Wiring.IsVisible = !WiringManager.Instance.Wiring.IsVisible;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void Button_OnClick()
        {
            TrySwitchVisibility();
        }

        public void Wiringmanager_WiringCreated(Wiring wiring)
        {
            WiringManager.Instance.Wiring.VisibilityChanged.AddListener(WiringManager_WiringVisibilityChanged);
            _stateImage.enabled = true;
        }

        public void Wiringmanager_WiringDestroyed(Wiring wiring)
        {
            WiringManager.Instance.Wiring.VisibilityChanged.RemoveListener(WiringManager_WiringVisibilityChanged);
            _stateImage.enabled = false;
        }

        private void WiringManager_WiringVisibilityChanged(Wiring wiring, bool state)
        {
            _stateImage.enabled = WiringManager.Instance.Wiring.IsVisible;
        }
        #endregion
        #endregion
    }
}
