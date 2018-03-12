using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace EMSP.UI.Menu.Contexts.View
{
    public class ModelMaterialSwitcher : MonoBehaviour
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
        private void TryToSwitchModelMaterial()
        {
            ModelManager.Instance.Model.IsTransparent = !ModelManager.Instance.Model.IsTransparent;
        }
        #endregion

        #region Indexers
        #endregion

        #region Events handlers
        public void Button_OnClick()
        {
            TryToSwitchModelMaterial();
        }

        public void ModelManager_ModelCreated(Model model)
        {
            model.TransparentStateChanged.AddListener(Model_TransparentStateChanged);
        }

        public void ModelManager_ModelDestroyed(Model model)
        {
            _stateImage.enabled = false;
        }

        private void Model_TransparentStateChanged(Model model, bool state)
        {
            _stateImage.enabled = state;
        }
        #endregion
        #endregion
    }
}